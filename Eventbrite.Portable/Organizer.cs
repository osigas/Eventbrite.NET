using Newtonsoft.Json;
using System.Collections.Generic;

namespace EventbriteNET
{
    /// <summary>
    /// Represents an Eventbrite Organizer <see cref="https://developer.eventbrite.com/docs/organizer-object/"/>
    /// </summary>
    public class Organizer : EventbriteObject
    {
        public Organizer()
        {
            Description = new MultipartTextField();
            Logo = new ImageField();
        }

        [JsonProperty("description")]
        public MultipartTextField Description { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("logo")]
        public ImageField Logo { get; set; }
        [JsonProperty("num_past_events")]
        public int NumPastEvents { get; set; }
        [JsonProperty("num_future_events")]
        public int NumFutureEvents { get; set; }
    }
}
