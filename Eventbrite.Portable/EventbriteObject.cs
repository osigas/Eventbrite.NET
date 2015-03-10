using Newtonsoft.Json;

namespace EventbriteNET
{
    /// <summary>
    /// Base Eventbrite Object <see cref="https://developer.eventbrite.com/docs/data-types/#object" />
    /// </summary>
    public abstract class EventbriteObject
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("resource_uri")]
        public string ResourceUri { get; set; }
    }
}
