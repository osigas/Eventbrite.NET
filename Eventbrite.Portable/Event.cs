using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EventbriteNET
{
    /// <summary>
    /// Represents an Eventbrite Event <see cref="https://developer.eventbrite.com/docs/event-object/"/>
    /// </summary>
    public class Event : EventbriteObject
    {
        public Event()
        {
            Name = new MultipartTextField();
            Description = new MultipartTextField();
            Start = new DateTimeTimezoneField();
            End = new DateTimeTimezoneField();
            Created = DateTime.UtcNow;
            Changed = DateTime.UtcNow;
            Organizer = new Organizer();
            Venue = new Venue();
            Category = new Category();
            Subcategory = new Subcategory();
            TicketClasses = new List<TicketClass>();
        }

        [JsonProperty("name")]
        public MultipartTextField Name { get; set; }
        [JsonProperty("description")]
        public MultipartTextField Description { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }
        [JsonProperty("capacity")]
        public int Capacity { get; set; }
        [JsonProperty("status")]
        public StatusOptions Status { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("online_event")]
        public bool OnlineEvent { get; set; }
        [JsonProperty("start")]
        public DateTimeTimezoneField Start { get; set; }
        [JsonProperty("end")]
        public DateTimeTimezoneField End { get; set; }
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("changed")]
        public DateTime Changed { get; set; }
        [JsonProperty("shareable")]
        public bool Shareable { get; set; }
        [JsonProperty("listed")]
        public bool Listed { get; set; }
        [JsonProperty("invite_only")]
        public bool InviteOnly { get; set; }
        [JsonProperty("show_remaining")]
        public bool ShowRemaining { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("organizer_id")]
        public long? OrganizerId { get; set; }
        [JsonProperty("venue_id")]
        public long? VenueId { get; set; }
        [JsonProperty("category_id")]
        public long? CategoryId { get; set; }
        [JsonProperty("subcategory_id")]
        public long? SubcategoryId { get; set; }
        [JsonProperty("format_id")]
        public long? FormatId { get; set; }

        [JsonProperty("organizer")]
        public Organizer Organizer { get; set; }
        [JsonProperty("venue")]
        public Venue Venue { get; set; }
        [JsonProperty("category")]
        public Category Category { get; set; }
        [JsonProperty("subcategory")]
        public Subcategory Subcategory { get; set; }
        [JsonProperty("format")]
        public Subcategory Format { get; set; }
        [JsonProperty("ticket_classes")]
        public IList<TicketClass> TicketClasses { get; set; }
    }
}
