using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace EventbriteNET.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="HttpClient"/> related calls
    /// </summary>
    public static class HttpExtensions
    {
        public static T As<T>(this HttpResponseMessage response)
        {
            if (response.Content != null)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(content);
            }
            else
                return default(T);
        }
    }
}
