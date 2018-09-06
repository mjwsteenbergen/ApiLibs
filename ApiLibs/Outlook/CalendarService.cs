﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Outlook
{
    public class CalendarService : SubService
    {
        public CalendarService(GraphService service) : base(service)
        {
        }

        public async Task<List<Calendar>> GetMyCalendars()
        {
            return (await MakeRequest<Calendars>("/me/calendars?$top=100")).Value;
        }

        public async Task<List<Event>> GetEvents()
        {
            return (await MakeRequest<Events>("/me/events")).Value;
        }

        public async Task CreateEvent(NewEvent ev)
        {
            await HandleRequest("/me/events", Call.POST, content: ev);
        }

        public async Task GetAllEventsOfAllCalendars(DateTime startTime, DateTime endTime)
        {
            var myCals = await GetMyCalendars();
            var res = await HandleRequest("$batch", Call.POST, content: new
            {
                requests = myCals.Select(i => new
                    {
                        url =
                            $"/me/calendars/{i.Id}/calendarView?startdatetime={startTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}&enddatetime={endTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}",
                        method = "GET",
                        id = i.Name
                    })
            });
        }
    }
}
