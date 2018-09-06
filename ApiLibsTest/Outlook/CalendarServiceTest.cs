using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ApiLibs;
using ApiLibs.General;
using ApiLibs.Outlook;
using NUnit.Framework;

namespace ApiLibsTest.Outlook
{
    class CalendarServiceTest
    {
        GraphService _graph;
        private CalendarService calendar;


        [OneTimeSetUp]
        public void SetUp()
        {
            Passwords passwords =
                Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            _graph = new GraphService(passwords.OutlookRefreshToken, passwords.OutlookClientID,
                passwords.OutlookClientSecret, passwords.OutlookEmail);
            calendar = _graph.CalendarService;
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
