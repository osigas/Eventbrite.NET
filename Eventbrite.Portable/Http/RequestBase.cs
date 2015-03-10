using EventbriteNET.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventbriteNET.Http
{
    abstract class RequestBase<TEntity> : IRequestHandler where TEntity : EventbriteObject
    {
        protected EventbriteContext Context;

        public RequestBase(EventbriteContext context)
        {
            this.Context = context;
            this.BaseUrl = new Uri(context.Host);
            this.DefaultParameters = new List<Parameter>();
        }

        /// <summary>
        /// Combined with parameters to construct URL for request
        /// Should include scheme and domain without trailing slash.
        /// </summary>
        /// <example>
        /// client.BaseUrl = new Uri("http://example.com");
        /// </example>
        public virtual Uri BaseUrl { get; set; }

        /// <summary>
        /// Parameters included with every request made with this instance
        /// </summary>
        public IList<Parameter> DefaultParameters { get; private set; }

        protected abstract IList<TEntity> OnGet();
        protected abstract TEntity OnGet(long id);
        protected abstract void OnCreate(TEntity entity);
        protected abstract void OnUpdate(TEntity entity);
        protected abstract Task<IList<TEntity>> OnGetAsync();
        protected abstract Task<TEntity> OnGetAsync(long id);

        IList<T> IRequestHandler.Get<T>()
        {
            return (OnGet() as IList<T>);
        }

        T IRequestHandler.Get<T>(long id)
        {
            return (OnGet(id) as T);
        }

        void IRequestHandler.Create<T>(T entity)
        {
            OnCreate(entity as TEntity);
        }

        void IRequestHandler.Update<T>(T entity)
        {
            OnUpdate(entity as TEntity);
        }

        Task<IList<T>> IRequestHandler.GetAsync<T>()
        {
            return (OnGetAsync() as Task<IList<T>>);
        }

        Task<T> IRequestHandler.GetAsync<T>(long id)
        {
            return (OnGetAsync(id) as Task<T>);
        }

        Task IRequestHandler.CreateAsync<T>(T entity)
        {
            return Task.Run(() => OnCreate(entity as TEntity));
        }

        Task IRequestHandler.UpdateAsync<T>(T entity)
        {
            return Task.Run(() => OnUpdate(entity as TEntity));
        }

        /// <summary>
        /// Assembles URL to call based on parameters, method and resource
        /// </summary>
        /// <param name="request">RestRequest to execute</param>
        /// <returns>Assembled System.Uri</returns>
        public Uri BuildUri(IRestRequest request)
        {
            if (this.BaseUrl == null)
                throw new NullReferenceException("RestClient must contain a value for BaseUrl");

            var assembled = request.Resource;
            var urlParms = request.Parameters.Where(p => p.Type == ParameterType.UrlSegment);
            var builder = new UriBuilder(this.BaseUrl);

            foreach (var p in urlParms)
            {
                if (p.Value == null)
                {
                    throw new ArgumentException(
                        string.Format("Cannot build uri when url segment parameter '{0}' value is null.", p.Name),
                        "request");
                }

                if (!string.IsNullOrEmpty(assembled))
                    assembled = assembled.Replace("{" + p.Name + "}", p.Value.ToString().UrlEncode());

                builder.Path = builder.Path.UrlDecode().Replace("{" + p.Name + "}", p.Value.ToString().UrlEncode());
            }

            this.BaseUrl = new Uri(builder.ToString());

            if (!string.IsNullOrEmpty(assembled) && assembled.StartsWith("/"))
            {
                assembled = assembled.Substring(1);
            }

            if (this.BaseUrl != null && !string.IsNullOrEmpty(this.BaseUrl.AbsoluteUri))
            {
                if (!this.BaseUrl.AbsoluteUri.EndsWith("/") && !string.IsNullOrEmpty(assembled))
                    assembled = string.Concat("/", assembled);

                assembled = string.IsNullOrEmpty(assembled)
                    ? this.BaseUrl.AbsoluteUri
                    : string.Format("{0}{1}", this.BaseUrl, assembled);
            }

            IEnumerable<Parameter> parameters;

            if (request.Method != HttpMethod.Post && request.Method != HttpMethod.Put)
            {
                parameters = request.Parameters.Where(p => p.Type == ParameterType.GetOrPost || p.Type == ParameterType.QueryString).ToList();
            }
            else
            {
                parameters = request.Parameters.Where(p => p.Type == ParameterType.QueryString).ToList();
            }

            if (!parameters.Any())
                return new Uri(assembled);

            // build and attach querystring
            var data = EncodeParameters(parameters);
            var separator = assembled.Contains("?") ? "&" : "?";

            assembled = string.Concat(assembled, separator, data);

            return new Uri(assembled);
        }

        /// <summary>
        /// Executes the request and returns a response
        /// </summary>
        /// <param name="request">Request to be executed</param>
        /// <returns>HttpResponseMessage</returns>
        public virtual HttpResponseMessage Execute(IRestRequest request)
        {
            using(var client = new HttpClient())
            using(var httpRequest = ConfigureHttp(request))
            {
                try
                {
                    var response = client.SendAsync(httpRequest);
                    return response.Result;
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = ex.Message
                    };
                }
            }
        }

        /// <summary>
        /// Executes the specified request and deserializes the response content using the appropriate content handler
        /// </summary>
        /// <typeparam name="T">Target deserialization type</typeparam>
        /// <param name="request">Request to execute</param>
        /// <returns>T with deserialized data</returns>
        public virtual T Execute<T>(IRestRequest request)
        {
            var response = Execute(request);
            return response.As<T>();
        }

        /// <summary>
        /// Executes the request and returns a response
        /// </summary>
        /// <param name="request">Request to be executed</param>
        /// <returns>Task to call HttpResponseMessage</returns>
        public virtual async Task<HttpResponseMessage> ExecuteAsync(IRestRequest request)
        {
            using(var client = new HttpClient())
            using(var httpRequest = ConfigureHttp(request))
            {
                try
                {
                    return await client.SendAsync(httpRequest);
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = ex.Message
                    };
                }
            }
        }

        /// <summary>
        /// Executes the specified request and deserializes the response content using the appropriate content handler
        /// </summary>
        /// <typeparam name="T">Target deserialization type</typeparam>
        /// <param name="request">Request to execute</param>
        /// <returns>Task to return T with deserialized data</returns>
        public virtual async Task<T> ExecuteAsync<T>(IRestRequest request)
        {
            var response = await ExecuteAsync(request);
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                return response.As<T>();
            }
            else
            {
                var error = response.As<ErrorField>();
                throw new RestRequestException(error);
            }
        }

        protected void ThrowResponseError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = response.As<ErrorField>();
                if (error != null)
                    throw new RestRequestException(error);
            }

            throw new RestRequestException("No error message parsed");
        }


        private static string EncodeParameters(IEnumerable<Parameter> parameters)
        {
            return string.Join("&", parameters.Select(EncodeParameter).ToArray());
        }

        private static string EncodeParameter(Parameter parameter)
        {
            return parameter.Value == null
                ? string.Concat(parameter.Name.UrlEncode(), "=")
                : string.Concat(parameter.Name.UrlEncode(), "=", parameter.Value.ToString().UrlEncode());
        }

        private HttpRequestMessage ConfigureHttp(IRestRequest request)
        {
            foreach (var p in DefaultParameters)
            {
                if (request.Parameters.Any(p2 => p2.Name == p.Name && p2.Type == p.Type))
                    continue;

                request.AddParameter(p);
            }

            var httpRequest = new HttpRequestMessage(request.Method, BuildUri(request));
            var data = new Dictionary<string, string>();
            foreach (var parameter in request.Parameters.Where(p => p.Type == ParameterType.GetOrPost && p.Value != null))
            {
                var valueType = parameter.Value.GetType();

                if(valueType == typeof(DateTime) || valueType == typeof(DateTime?) || valueType == typeof(Nullable<DateTime>))
                {
                    data.Add(parameter.Name, ((DateTime)parameter.Value).ToString("yyyy-MM-ddThh:mm:ssK"));
                }
                else
                    data.Add(parameter.Name, parameter.Value.ToString());
            }

            if (data.Count > 0)
                httpRequest.Content = new FormUrlEncodedContent(data);

            return httpRequest;
        }
    }
}
