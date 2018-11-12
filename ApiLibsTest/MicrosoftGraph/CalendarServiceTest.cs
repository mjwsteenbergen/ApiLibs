using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.MicrosoftGraph;
using NUnit.Framework;

namespace ApiLibsTest.MicrosoftGraph
{
    class CalendarServiceTest
    {
        private CalendarService calendar;


        [OneTimeSetUp]
        public void SetUp()
        {
            calendar = GraphTest.GetGraphService().CalendarService;
        }

        [Test]
        public async Task GetAllCalendars()
        {
            var s = await calendar.GetMyCalendars();
        }

        [Test]
        public async Task GetMyEvents()
        {
            var s = await calendar.GetEvents();
        }

        [Test]
        public async Task GetAllEventsOfAllCalendars()
        {
            await calendar.GetAllEventsOfAllCalendars(DateTime.Now, DateTime.Now.AddDays(7));
        }

        [Test]
        [Ignore("Changes prod")]
        public async Task CreateEvent()
        {
            await calendar.CreateEvent(new NewEvent
            {
                Subject = "ApiLibs test",
                Body = new Body
                {
                    Content = "Hello"
                },
                Start = new EventTime
                {
                    DateTime = DateTimeOffset.Now
                },
                End = new EventTime
                {
                    DateTime = DateTimeOffset.Now.AddHours(1)
                }
            });
        }

        [Test]
        [Ignore("Changes prod")]
        public async Task CreateEventWithInvite()
        {
            await calendar.CreateEvent(new NewEvent
            {
                Subject = "ApiLibs test",
                Body = new Body
                {
                    Content = "Hello"
                },
                Start = new EventTime
                {
                    DateTime = DateTimeOffset.Now
                },
                End = new EventTime
                {
                    DateTime = DateTimeOffset.Now.AddHours(1)
                },
                Attendees = new List<Attendee>
                {
                    new Attendee
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = "Your Email",
                            Name = "Martijn"
                        }
                    }
                }
            });
        }
    }
}
