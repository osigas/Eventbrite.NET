using EventbriteNET.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventbriteNET.Http
{
    /// <summary>
    /// https://developer.eventbrite.com/docs/user-details/
    /// </summary>
    class UserRequestHander : RequestBase<User>
    {
        public UserRequestHander(EventbriteContext context) : base(context) { }

        protected override IList<User> OnGet()
        {
            var request = new RestRequest("users/me/");
            request.AddQueryParameter("token", Context.Token);
            var user = this.Execute<User>(request);
            return new List<User> { user };
        }

        protected override User OnGet(long id)
        {
            var request = new RestRequest("users/{id}/");
            request.AddUrlSegment("id", id.ToString());
            request.AddQueryParameter("token", Context.Token);
            return this.Execute<User>(request);
        }

        protected override void OnCreate(User entity)
        {
            throw new NotImplementedException();
        }

        protected override void OnUpdate(User entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<IList<User>> OnGetAsync()
        {
            return Task.Run(() => OnGet());
        }

        protected override Task<User> OnGetAsync(long id)
        {
            var request = new RestRequest("users/{id}/");
            request.AddUrlSegment("id", id.ToString());
            request.AddQueryParameter("token", Context.Token);
            return this.ExecuteAsync<User>(request);
        }
    }
}
