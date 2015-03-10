using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace EventbriteNET.Http
{
    public class RestRequest : IRestRequest
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RestRequest()
        {
            this.Parameters = new List<Parameter>();
        }

        /// <summary>
        /// Sets Method property to value of method
        /// </summary>
        /// <param name="method">Method to use for this request</param>
        public RestRequest(HttpMethod method)
            : this()
        {
            this.Method = method;
        }

        /// <summary>
        /// Sets Resource property
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        public RestRequest(string resource) : this(resource, HttpMethod.Get) { }

        /// <summary>
        /// Sets Resource and Method properties
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        /// <param name="method">Method to use for this request</param>
        public RestRequest(string resource, HttpMethod method)
            : this()
        {
            this.Resource = resource;
            this.Method = method;
        }

        /// <summary>
        /// Sets Resource property
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        public RestRequest(Uri resource) : this(resource, HttpMethod.Get) { }

        /// <summary>
        /// Sets Resource and Method properties
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        /// <param name="method">Method to use for this request</param>
        public RestRequest(Uri resource, HttpMethod method)
            : this(resource.IsAbsoluteUri ? resource.AbsolutePath + resource.Query : resource.OriginalString, method)
        {
            //resource.PathAndQuery not supported by Silverlight :(
        }

        /// <summary>
        /// Container of all HTTP parameters to be passed with the request. 
        /// See AddParameter() for explanation of the types of parameters that can be passed
        /// </summary>
        public List<Parameter> Parameters { get; private set; }


        private HttpMethod method = HttpMethod.Get;
        /// <summary>
        /// Determines what HTTP method to use for this request. Supported methods: GET, POST, PUT, DELETE, HEAD, OPTIONS
        /// Default is GET
        /// </summary>
        public HttpMethod Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        /// <summary>
        /// Calls AddParameter() for all public, readable properties of obj
        /// </summary>
        /// <param name="obj">The object with properties to add as parameters</param>
        /// <returns>This request</returns>
        public IRestRequest AddObject(object obj)
        {
            this.AddObject(obj, new string[] { });
            return this;
        }

        /// <summary>
        /// Calls AddParameter() for all public, readable properties specified in the includedProperties list
        /// </summary>
        /// <example>
        /// request.AddObject(product, "ProductId", "Price", ...);
        /// </example>
        /// <param name="obj">The object with properties to add as parameters</param>
        /// <param name="includedProperties">The names of the properties to include</param>
        /// <returns>This request</returns>
        public IRestRequest AddObject(object obj, params string[] includedProperties)
        {
            return AddObject(obj, null, includedProperties);
        }

        /// <summary>
        /// Calls AddParameter() for all public, readable properties specified in the includedProperties list
        /// </summary>
        /// <example>
        /// request.AddObject(product, "ProductId", "Price", ...);
        /// </example>
        /// <param name="obj">The object with properties to add as parameters</param>
        /// <param name="parent">The name of the parent object</param>
        /// <param name="includedProperties">The names of the properties to include</param>
        /// <returns>This request</returns>
        public IRestRequest AddObject(object obj, string parent, params string[] includedProperties)
        {
            // automatically create parameters from object props
            var type = obj.GetType();
            var props = type.GetRuntimeProperties();

            foreach (var prop in props)
            {
                bool isAllowed = includedProperties.Length == 0 ||
                                 (includedProperties.Length > 0 && includedProperties.Contains(prop.Name));

                if (!isAllowed)
                    continue;

                var propType = prop.PropertyType;
                var val = prop.GetValue(obj, null);

                if (val == null)
                    continue;

                if (propType.IsArray)
                {
                    var elementType = propType.GetElementType();

                    if (((Array)val).Length > 0 &&
                        elementType != null &&
                        (elementType.GetTypeInfo().IsPrimitive || elementType.GetTypeInfo().IsValueType || elementType == typeof(string)))
                    {
                        // convert the array to an array of strings
                        var values = (from object item in ((Array)val)
                                      select item.ToString())
                                     .ToArray<string>();

                        val = string.Join(",", values);
                    }
                    else
                    {
                        // try to cast it
                        val = string.Join(",", (string[])val);
                    }
                }

                // get JsonProperty Name if available
                string name = prop.Name;
                var jsonProperty = prop.GetCustomAttribute<JsonPropertyAttribute>(true);
                if (jsonProperty != null)
                    name = jsonProperty.PropertyName;

                // prepend parent name, if available
                if (parent != null)
                    name = string.Format("{0}.{1}", parent, name);

                // flatten class properties
                if (!propType.IsArray && !propType.GetTypeInfo().IsPrimitive && !propType.GetTypeInfo().IsValueType && propType != typeof(string))
                {
                    this.AddObject(val, name, includedProperties);
                }
                else
                    this.AddParameter(name, val);
            }

            return this;
        }

        /// <summary>
        /// The Resource URL to make the request against.
        /// Tokens are substituted with UrlSegment parameters and match by name.
        /// Should not include the scheme or domain. Do not include leading slash.
        /// Combined with RestClient.BaseUrl to assemble final URL:
        /// {BaseUrl}/{Resource} (BaseUrl is scheme + domain, e.g. http://example.com)
        /// </summary>
        /// <example>
        /// // example for url token replacement
        /// request.Resource = "Products/{ProductId}";
        /// request.AddParameter("ProductId", 123, ParameterType.UrlSegment);
        /// </example>
        public string Resource { get; set; }

        /// <summary>
        /// Add the parameter to the request
        /// </summary>
        /// <param name="p">Parameter to add</param>
        /// <returns></returns>
        public IRestRequest AddParameter(Parameter p)
        {
            this.Parameters.Add(p);
            return this;
        }

        /// <summary>
        /// Adds a HTTP parameter to the request (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <returns>This request</returns>
        public IRestRequest AddParameter(string name, object value)
        {
            return this.AddParameter(new Parameter
            {
                Name = name,
                Value = value,
                Type = ParameterType.GetOrPost
            });
        }

        /// <summary>
        /// Adds a parameter to the request. There are four types of parameters:
        /// - GetOrPost: Either a QueryString value or encoded form value based on method
        /// - HttpHeader: Adds the name/value pair to the HTTP request's Headers collection
        /// - UrlSegment: Inserted into URL if there is a matching url token e.g. {AccountId}
        /// - RequestBody: Used by AddBody() (not recommended to use directly)
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <param name="type">The type of parameter to add</param>
        /// <returns>This request</returns>
        public IRestRequest AddParameter(string name, object value, ParameterType type)
        {
            return this.AddParameter(new Parameter
            {
                Name = name,
                Value = value,
                Type = type
            });
        }

        /// <summary>
        /// Shortcut to AddParameter(name, value, UrlSegment) overload
        /// </summary>
        /// <param name="name">Name of the segment to add</param>
        /// <param name="value">Value of the segment to add</param>
        /// <returns></returns>
        public IRestRequest AddUrlSegment(string name, string value)
        {
            return this.AddParameter(name, value, ParameterType.UrlSegment);
        }

        /// <summary>
        /// Shortcut to AddParameter(name, value, QueryString) overload
        /// </summary>
        /// <param name="name">Name of the parameter to add</param>
        /// <param name="value">Value of the parameter to add</param>
        /// <returns></returns>
        public IRestRequest AddQueryParameter(string name, string value)
        {
            return this.AddParameter(name, value, ParameterType.QueryString);
        }
    }
}
