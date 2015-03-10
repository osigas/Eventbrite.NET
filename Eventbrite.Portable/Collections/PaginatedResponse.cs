using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventbriteNET.Collections
{
    public partial class PaginatedResponse<T> : IPaginatedResponse<T> where T : EventbriteObject
    {
        public PaginatedResponse()
        {
            Pagination = new Pagination();
        }

        public PaginatedResponse(string dataName) : this()
        {
            this.DataName = dataName;

            // set Attribute name
            var dataJsonProperty = this.GetType().GetRuntimeProperty("Data").GetCustomAttribute<JsonPropertyAttribute>(true);
            dataJsonProperty.PropertyName = this.DataName;
        }

        [JsonProperty("locale")]
        public string Locale { get; set; }
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
        [JsonProperty("data")]
        public IList<T> Data { get; set; }

        public string DataName { get; private set; }

        public bool HasPreviousPage
        {
            get { return (Pagination.PageNumber > 0); }
        }
        public bool HasNextPage
        {
            get { return (Pagination.PageNumber < Pagination.PageCount); }
        }
    }

    public class Pagination
    {
        [JsonProperty("object_count")]
        public int ObjectCount { get; set; }
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }
        [JsonProperty("page_size")]
        public int PageSize { get; set; }
        [JsonProperty("page_count")]
        public int PageCount { get; set; }
    }
}
