using Newtonsoft.Json;
using System.Collections.Generic;

namespace EventbriteNET
{
    /// <summary>
    /// Represents an Eventbrite User <see cref="https://developer.eventbrite.com/docs/user-object/"/>
    /// </summary>
    public class User : EventbriteObject
    {
        public User()
        {
            Emails = new List<EmailField>();
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("emails")]
        public IList<EmailField> Emails { get; set; }
    }
}
