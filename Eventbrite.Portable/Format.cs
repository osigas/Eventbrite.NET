using Newtonsoft.Json;
using System;

namespace EventbriteNET
{
    /// <summary>
    /// Represents an Eventbrite Format <see cref="https://developer.eventbrite.com/docs/format-object/"/>
    /// </summary>
    public class Format : EventbriteObject
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
}
