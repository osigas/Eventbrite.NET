using EventbriteNET.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventbriteNET.Http
{
    /// <summary>
    /// https://developer.eventbrite.com/docs/ticket-classes/
    /// </summary>
    class TicketClassRequestHander : RequestBase<TicketClass>
    {
        public TicketClassRequestHander(EventbriteContext context) : base(context) { }

        protected override IList<TicketClass> OnGet()
        {
            if (Context.EventId <= 0)
                throw new ArgumentException("EventId not set in Context", "entity");

            var request = new RestRequest("events/{id}/ticket_classes/");
            request.AddUrlSegment("id", Context.EventId.ToString());
            request.AddQueryParameter("token", Context.Token);

            return this.Execute<IList<TicketClass>>(request);
        }

        protected override TicketClass OnGet(long id)
        {
            if (Context.EventId <= 0)
                throw new ArgumentException("EventId not set in Context", "entity");

            var request = new RestRequest("events/{id}/ticket_classes/{ticketId}/");
            request.AddUrlSegment("id", Context.EventId.ToString());
            request.AddUrlSegment("ticketId", id.ToString());
            request.AddQueryParameter("token", Context.Token);

            return this.Execute<TicketClass>(request);
        }

        protected override void OnCreate(TicketClass entity)
        {
            if (entity.EventId <= 0)
                throw new ArgumentException("EventId not set in TicketClass", "entity");

            var request = new RestRequest("events/{id}/ticket_classes/", HttpMethod.Post);
            request.AddUrlSegment("id", entity.EventId.ToString());
            request.AddQueryParameter("token", Context.Token);
            request.AddObject(entity, "ticket_class", AcceptedPostFields());

            var response = this.Execute(request);
            if (response.IsSuccessStatusCode)
            {
                // persisted properties
                var persisted = response.As<TicketClass>();
                entity.Id = persisted.Id;
                entity.ResourceUri = persisted.ResourceUri;
            }
            else
                this.ThrowResponseError(response);
        }

        protected override void OnUpdate(TicketClass entity)
        {
            if (entity.Id <= 0)
                throw new ArgumentException("Id not set in TicketClass", "entity");

            if (entity.EventId <= 0)
                throw new ArgumentException("EventId not set in TicketClass", "entity");

            var request = new RestRequest("events/{id}/ticket_classes/{ticketId}/", HttpMethod.Post);
            request.AddUrlSegment("id", entity.EventId.ToString());
            request.AddUrlSegment("ticketId", entity.Id.ToString());
            request.AddQueryParameter("token", Context.Token);
            request.AddObject(entity, "ticket_class", AcceptedPostFields());

            var response = this.Execute(request);
            if (!response.IsSuccessStatusCode)
                this.ThrowResponseError(response);
        }

        protected override Task<IList<TicketClass>> OnGetAsync()
        {
            return Task.Run(() => OnGet());
        }

        protected override Task<TicketClass> OnGetAsync(long id)
        {
            if (Context.EventId <= 0)
                throw new ArgumentException("EventId not set in Context", "entity");

            var request = new RestRequest("events/{id}/ticket_classes/{ticketId}/");
            request.AddUrlSegment("id", Context.EventId.ToString());
            request.AddUrlSegment("ticketId", id.ToString());
            request.AddQueryParameter("token", Context.Token);

            return this.ExecuteAsync<TicketClass>(request);
        }

        private string[] AcceptedPostFields()
        {
            return new[] 
                {
                    "Name",
                    "Description",
                    "QuantityTotal",
                    "Cost",
                    "Currency",
                    "Value",
                    "Donation",
                    "Free", 
                    "IncludeFee",
                    "SplitFee",
                    "HideDescription",
                    "SalesStart",
                    "SalesStartAfter",
                    "MinimumQuantity",
                    "MaximumQuantity",
                    "AutoHide",
                    "AutoHideBefore",
                    "AutoHideAfter"
                };
        }
    }
}
