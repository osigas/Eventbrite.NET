using Newtonsoft.Json;
using System;

namespace EventbriteNET
{
    /// <summary>
    /// Represents an Eventbrite Ticket Class <see cref="https://developer.eventbrite.com/docs/ticket-class-object/"/>
    /// </summary>
    public class TicketClass : EventbriteObject
    {
        public TicketClass()
        {
            Cost = new CurrencyField();
            Fee = new CurrencyField();
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("donation")]
        public bool Donation { get; set; }
        [JsonProperty("free")]
        public bool Free { get; set; }
        [JsonProperty("minimum_quantity")]
        public int? MinimumQuantity { get; set; }
        [JsonProperty("maximum_quantity")]
        public int? MaximumQuantity { get; set; }
        [JsonProperty("event_id")]
        public long EventId { get; set; }
        [JsonProperty("cost")]
        public CurrencyField Cost { get; set; }
        [JsonProperty("fee")]
        public CurrencyField Fee { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }
        [JsonProperty("sales_start")]
        public DateTime? SalesStart { get; set; }
        [JsonProperty("sales_end")]
        public DateTime? SalesEnd { get; set; }
        [JsonProperty("sales_start_after")]
        public string SalesStartAfter { get; set; }
        [JsonProperty("quantity_total")]
        public int? QuantityTotal { get; set; }
        [JsonProperty("quantity_sold")]
        public int? QuantitySold { get; set; }
        [JsonProperty("include_fee")]
        public bool IncludeFee { get; set; }
        [JsonProperty("split_fee")]
        public bool SplitFee { get; set; }
        [JsonProperty("hide_description")]
        public bool HideDescription { get; set; }
        [JsonProperty("auto_hide")]
        public bool AutoHide { get; set; }
        [JsonProperty("auto_hide_before")]
        public DateTime? AutoHideBefore { get; set; }
        [JsonProperty("auto_hide_after")]
        public DateTime? AutoHideAfter { get; set; }
    }
}
