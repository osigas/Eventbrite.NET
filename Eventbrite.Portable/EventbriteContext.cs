using EventbriteNET.Http;
using System;
using System.Collections.Generic;

namespace EventbriteNET
{
    /// <summary>
    /// Context to communicate with the Eventbrite API <see cref="http://developer.eventbrite.com/docs/"/>
    /// </summary>
    public class EventbriteContext
    {
        private string _eventbriteApi = "https://www.eventbriteapi.com/v3/";
        private IDictionary<string, IRequestHandler> RequestHandlers = new Dictionary<string, IRequestHandler>();

        /// <summary>
        /// Default constructor that registers default request handlers
        /// </summary>
        public EventbriteContext()
        {
            this.RequestHandlers[typeof(Category).Name] = new CategoryRequestHander(this) as IRequestHandler;
            this.RequestHandlers[typeof(Event).Name] = new EventRequestHander(this) as IRequestHandler;
            this.RequestHandlers[typeof(TicketClass).Name] = new TicketClassRequestHander(this) as IRequestHandler;
            this.RequestHandlers[typeof(User).Name] = new UserRequestHander(this) as IRequestHandler;
        }

        /// <summary>
        /// Sets the OAuth Authentication Token for the request
        /// </summary>
        /// <param name="token">OAuth token <see cref="https://developer.eventbrite.com/docs/auth/"/></param>
        /// </example>
        public EventbriteContext(string token) : this()
        {
            if (String.IsNullOrEmpty(token))
                throw new ArgumentNullException("token");

            this.Token = token;
        }

        public string Host { get { return this._eventbriteApi; } set { this._eventbriteApi = value; } }
        public string Token { get; set; }
        public long UserId { get; set; }
        public long OrganizerId { get; set; }
        public long EventId { get; set; }

        /// <summary>
        /// Fetches all specified Objects
        /// </summary>
        /// <typeparam name="T">Eventbrite Object</typeparam>
        public IList<T> Get<T>() where T : EventbriteObject
        {
            var handler = GetHandler(typeof(T));
            return handler.Get<T>();
        }

        /// <summary>
        /// Fetches a specified Object
        /// </summary>
        /// <param name="id">ID for the Object</param>
        /// <typeparam name="T">Eventbrite Object</typeparam>
        public T Get<T>(long id) where T : EventbriteObject
        {
            var handler = GetHandler(typeof(T));
            return handler.Get<T>(id);
        }

        /// <summary>
        /// Creates a new Object on the Eventbrite website
        /// </summary>
        /// <param name="T">Eventbrite Object to create</param>
        /// <typeparam name="T">Eventbrite Object</typeparam>
        public void Create<T>(T entity) where T : EventbriteObject
        {
            var handler = GetHandler(typeof(T));
            handler.Create<T>(entity);
        }

        /// <summary>
        /// Updates an existing Object on the Eventbrite website
        /// </summary>
        /// <param name="T">Eventbrite Object to update</param>
        /// <typeparam name="T">Eventbrite Object</typeparam>
        public void Update<T>(T entity) where T : EventbriteObject
        {
            var handler = GetHandler(typeof(T));
            handler.Update<T>(entity);
        }

        /// <summary>
        /// Publishes an existing Event on the Eventbrite website
        /// </summary>
        /// <param name="id">ID of Event to publish</param>
        public void Publish(long id)
        {
            var handler = (EventRequestHander)GetHandler(typeof(Event));
            handler.Publish(id);
        }

        /// <summary>
        /// Unpublishes an existing Event on the Eventbrite website
        /// </summary>
        /// <param name="id">ID of Event to unpublish</param>
        public void Unpublish(long id)
        {
            var handler = (EventRequestHander)GetHandler(typeof(Event));
            handler.Unpublish(id);
        }

        /// <summary>
        /// Searches existing Events on the Eventbrite website
        /// </summary>
        /// <param name="id">ID of Event to unpublish</param>
        public void Search()
        {
            var handler = (EventRequestHander)GetHandler(typeof(Event));
            handler.Search();
        }

        /// <summary>
        /// Finds appropriate <see cref="IRequestHandler" />
        /// </summary>
        /// <param name="type"></param>
        private IRequestHandler GetHandler(Type type)
        {
            IRequestHandler handler;
            if(!this.RequestHandlers.TryGetValue(type.Name, out handler))
                throw new NotSupportedException(string.Format("Type \"{0}\" is not currently supported.", type.Name));

            return handler;
        }
    }
}
