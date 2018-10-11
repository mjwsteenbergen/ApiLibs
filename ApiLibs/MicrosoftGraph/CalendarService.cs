using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.MicrosoftGraph
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
            return (await MakeRequest<Events>("/me/events?$top=20")).Value;
        }

        public async Task CreateEvent(NewEvent ev)
        {
            await HandleRequest("/me/events", Call.POST, content: ev);
        }

        public async Task<List<Event>> GetAllEventsOfAllCalendars(DateTime startTime, DateTime endTime)
        {
            var myCals = await GetMyCalendars();
            var res = await MakeRequest<BatchResult>("$batch", Call.POST, content: new
            {
                requests = myCals.Select(i => new
                {
                    url =
                        $"/me/calendars/{i.Id}/calendarView?startdatetime={startTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}&enddatetime={endTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}",
                    method = "GET",
                    id = i.Name
                })
            });

            return res.Responses.Select(i => i.Body).Where(i => i.Error == null).SelectMany(i => i.Value).ToList();
        }

        public async Task EditEvent(string id, NewEvent ev)
        {
            var res = await MakeRequest<Event>($"/me/events/{id}", Call.PATCH, content: ev);

        }
    }
}
