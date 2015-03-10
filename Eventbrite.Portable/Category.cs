using Newtonsoft.Json;
using System;

namespace EventbriteNET
{
    /// <summary>
    /// Represents an Eventbrite Category <see cref="https://developer.eventbrite.com/docs/category-object/"/>
    /// </summary>
    public class Category : EventbriteObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("name_localized")]
        public string NameLocalized { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        [JsonProperty("short_name_localized")]
        public string ShortNameLocalized { get; set; }
    }

    /// <summary>
    /// Represents an Eventbrite Subcategory <see cref="https://developer.eventbrite.com/docs/subcategory-object/"/>
    /// </summary>
    public class Subcategory : EventbriteObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
