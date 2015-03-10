using EventbriteNET.Extensions;
using NUnit.Framework;
using System;

namespace EventbriteNET.Tests
{
    [TestFixture]
    public class EventTests
    {
        private EventbriteContext Context;

        [SetUp]
        public void Init()
        {
            Context = new EventbriteContext(EventbriteVariables.Token);
        }

        [Test]
        public void Get_Event()
        {
            var ebEvent = Context.Get<Event>(EventbriteVariables.EventId);
            Assert.That(ebEvent, Is.Not.Null);
            Assert.AreEqual(ebEvent.Id, EventbriteVariables.EventId);
        }

        [Test]
        public void Create_Event()
        {
            var ebEvent = new Event
            {
                Name = new MultipartTextField { Html = "Test event" },
                Description = new MultipartTextField { Html = "Test event description" },
                Start = new DateTimeTimezoneField { Timezone = "Europe/London" },
                End = new DateTimeTimezoneField { Timezone = "Europe/London" },
                OnlineEvent = true,
                Currency = "GBP",
                CategoryId = 108,
                SubcategoryId = 8001
            };

            ebEvent.TicketClasses.Add(new TicketClass
                {
                    Name = "General admission ticket",
                    Cost = new CurrencyField { Currency = "GBP", Value = 10, Display = "£10".UrlEncode() },
                    Free = false,
                    Fee = new CurrencyField { Currency = "GBP", Value = 10, Display = "£10".UrlEncode()  },
                    QuantityTotal = 1000,
                    MinimumQuantity = 1,
                    MaximumQuantity = 10
                });

            // post
            Assert.DoesNotThrow(() => Context.Create<Event>(ebEvent));
            Assert.That(ebEvent, Is.Not.Null);
            Assert.Greater(ebEvent.Id, 0);
        }

        [Test]
        public void Get_Me()
        {
            var ebUser = Context.Get<User>(0);
            Assert.That(ebUser, Is.Not.Null);
        }

        [Test]
        public void Get_User()
        {
            var ebUser = Context.Get<User>(EventbriteVariables.UserId);
            Assert.That(ebUser, Is.Not.Null);
            Assert.AreEqual(ebUser.Id, EventbriteVariables.UserId);
        }

        [Test]
        public void Update_Event()
        {
            var ebEvent = Context.Get<Event>(EventbriteVariables.EventId);
            int oldCapacity = ebEvent.Capacity;
            ebEvent.Capacity++;
            
            // post
            Assert.DoesNotThrow(() => Context.Update<Event>(ebEvent));
            // recall
            ebEvent = Context.Get<Event>(EventbriteVariables.EventId);
            Assert.AreNotEqual(oldCapacity, ebEvent.Capacity);
        }

        [Test]
        public void NotSupportedException_On_Unhandled_Object_Type()
        {
            Assert.Throws<NotSupportedException>(() => Context.Get<MockObject>(0));
        }

        private class MockObject : EventbriteObject { }

        [Test]
        public void Get_Categories()
        {
            var categories = Context.Get<Category>();
            Assert.That(categories, Has.Count);
        }
    }
}
