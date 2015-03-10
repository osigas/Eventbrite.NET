using EventbriteNET.Collections;
using EventbriteNET.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventbriteNET.Http
{
    /// <summary>
    /// http://developer.eventbrite.com/docs/event-categories/
    /// </summary>
    class CategoryRequestHander : RequestBase<Category>
    {
        public CategoryRequestHander(EventbriteContext context) : base(context) { }

        protected override IList<Category> OnGet()
        {
            var request = new RestRequest("categories/");
            request.AddQueryParameter("token", Context.Token);
            return this.Execute<IList<Category>>(request);
        }

        protected override Category OnGet(long id)
        {
            throw new NotImplementedException();
        }

        protected override void OnCreate(Category entity)
        {
            throw new NotImplementedException();
        }

        protected override void OnUpdate(Category entity)
        {
            throw new NotImplementedException();
        }

        protected override Task<IList<Category>> OnGetAsync()
        {
            return Task.Run(() => OnGet());
        }

        protected override Task<Category> OnGetAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
