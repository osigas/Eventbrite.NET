using Newtonsoft.Json;
using System;

namespace EventbriteNET
{
    /// <summary>
    /// <see cref="https://developer.eventbrite.com/docs/data-types/#multiparttext" />
    /// </summary>
    public class MultipartTextField
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("html")]
        public string Html { get; set; }
    }

    /// <summary>
    /// <see cref="https://developer.eventbrite.com/docs/data-types/#datetimewithtimezone" />
    /// </summary>
    public class DateTimeTimezoneField
    {
        public DateTimeTimezoneField()
        {
            Local = DateTime.Now;
            Utc = DateTime.UtcNow;
        }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
        [JsonProperty("local")]
        public DateTime Local { get; set; }
        [JsonProperty("utc")]
        public DateTime Utc { get; set; }
    }

    /// <summary>
    /// <see cref="https://developer.eventbrite.com/docs/data-types/#address" />
    /// </summary>
    public class AddressField
    {
        [JsonProperty("address_1")]
        public string Address1 { get; set; }
        [JsonProperty("address_2")]
        public string Address2 { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }

    /// <summary>
    /// <see cref="https://developer.eventbrite.com/docs/data-types/#currency" />
    /// </summary>
    public class CurrencyField
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("display")]
        public string Display { get; set; }
        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class ImageField
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class EmailField
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("verified")]
        public bool Verified { get; set; }
        [JsonProperty("primary")]
        public bool Primary { get; set; }
    }

    public class ErrorField
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public enum StatusOptions
    {
        draft,
        cancelled,
        live,
        started,
        completed
    }

    public enum OrderOptions
    {
        start_asc,
        start_desc,
        created_asc,
        created_desc
    }

    public enum Gender
    {
        Male,
        Female
    }
}
