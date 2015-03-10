using EventbriteNET.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventbriteNET.Http
{
    /// <summary>
    /// http://developer.eventbrite.com/docs/event-details/
    /// </summary>
    class EventRequestHander : RequestBase<Event>
    {
        public EventRequestHander(EventbriteContext context) : base(context) { }

        public IList<Event> Search()
        {

            return new List<Event>();
        }

        protected override IList<Event> OnGet()
        {
            throw new NotImplementedException();
        }

        protected override Event OnGet(long id)
        {
            var request = new RestRequest("events/{id}/");
            request.AddUrlSegment("id", id.ToString());
            request.AddQueryParameter("token", Context.Token);

            Context.EventId = id;
            return this.Execute<Event>(request);
        }

        protected override void OnCreate(Event entity)
        {
            var request = new RestRequest("events/", HttpMethod.Post);
            request.AddQueryParameter("token", Context.Token);
            request.AddObject(entity, "event", AcceptedPostFields());

            // execute Event create
            var response = this.Execute(request);

            if(response.IsSuccessStatusCode)
            {
                // get updated response
                var persisted = response.As<Event>();
                // persist base properties onto entity
                entity.Id = persisted.Id;
                entity.ResourceUri = persisted.ResourceUri;
                Context.EventId = persisted.Id;

                // Ticket create
                if (entity.TicketClasses.Count > 0)
                {
                    IRequestHandler ticketHandler = new TicketClassRequestHander(Context);

                    foreach (var ticket in entity.TicketClasses)
                    {
                        // set associated Event ID
                        ticket.EventId = entity.Id;
                        ticketHandler.Create(ticket);
                    }
                }

                // Venue create
            }
            else
            {
                this.ThrowResponseError(response);
            }
        }

        protected override void OnUpdate(Event entity)
        {
            if (entity.Id <= 0)
                throw new ArgumentException("Id not set in Event", "entity");

            var request = new RestRequest("events/{id}/", HttpMethod.Post);
            request.AddUrlSegment("id", entity.Id.ToString());
            request.AddQueryParameter("token", Context.Token);
            request.AddObject(entity, "event", AcceptedPostFields());

            // Execute Event update
            var response = this.Execute(request);

            if (response.IsSuccessStatusCode)
            {
                Context.EventId = entity.Id;

                // Ticket update
                if (entity.TicketClasses.Count > 0)
                {
                    IRequestHandler ticketHandler = new TicketClassRequestHander(Context);

                    foreach (var ticket in entity.TicketClasses)
                    {
                        ticket.EventId = entity.Id;
                        ticketHandler.Update(ticket);
                    }
                }
            }
            else 
            {
                this.ThrowResponseError(response);
            }
        }

        protected override Task<IList<Event>> OnGetAsync()
        {
            return Task.Run(() => OnGet());
        }

        protected override Task<Event> OnGetAsync(long id)
        {
            var request = new RestRequest("events/{id}/");
            request.AddUrlSegment("id", id.ToString());
            request.AddQueryParameter("token", Context.Token);

            Context.EventId = id;
            return this.ExecuteAsync<Event>(request);
        }

        public void Publish(long id)
        {
            var request = new RestRequest("events/{id}/publish/", HttpMethod.Post);
            request.AddUrlSegment("id", id.ToString());
            request.AddQueryParameter("token", Context.Token);

            Context.EventId = id;
            this.Execute(request);
        }

        public void Unpublish(long id)
        {
            var request = new RestRequest("events/{id}/unpublish/", HttpMethod.Post);
            request.AddUrlSegment("id", id.ToString());
            request.AddQueryParameter("token", Context.Token);

            Context.EventId = id;
            this.Execute(request);
        }

        private string[] AcceptedPostFields()
        {
            return new[] 
                {
                    "Name",
                    "Html",
                    "Description",
                    "OrganizerId",
                    "Start",
                    "End",
                    "Utc", 
                    "Timezone",
                    "Currency",
                    "VenueId",
                    "OnlineEvent",
                    "Listed",
                    "CategoryId",
                    "SubcategoryId",
                    "FormatId",
                    "Shareable",
                    "InviteOnly",
                    "Password",
                    "Capacity",
                    "ShowRemaining"
                };
        }
    }
}
