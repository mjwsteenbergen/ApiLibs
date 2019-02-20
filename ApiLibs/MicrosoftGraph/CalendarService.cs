using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class CalendarService : GraphSubService
    {
        public CalendarService(GraphService service) : base(service, "v1.0")
        {
        }

        #region Events

        public async Task<List<Event>> GetEvents(OData data = null)
        {
            data = data ?? new OData
            {
                Top = 20
            };
            return (await MakeRequest<Events>("/me/events" + data.ConvertToUrl())).Value;
        }

        public Task<Event> GetEvent(string id)
        {
            return MakeRequest<Event>($"/me/events/{id}");
        }

        public Task<Event> CreateEvent(NewEvent ev)
        {
            return MakeRequest<Event>("/me/events", Call.POST, content: ev);
        }

        public Task<Event> CreateEvent(NewEvent ev, Calendar cal)
        {
            return CreateEvent(ev, cal.Id);
        }

        private Task<Event> CreateEvent(NewEvent ev, string id)
        {
            return MakeRequest<Event>($"me/calendars/{id}/events", Call.POST, content: ev);
        }

        public Task<Event> UpdateEvent(NewEvent ev)
        {
            return UpdateEvent(ev.Id, ev);
        }

        public Task<Event> UpdateEvent(string id, NewEvent ev)
        {
            return MakeRequest<Event>($"/me/events/{id}", Call.PATCH, content: ev);
        }

        public Task DeleteEvent(Event e)
        {
            return DeleteEvent(e.Id);
        }

        public Task DeleteEvent(string id)
        {
            return HandleRequest($"me/events/{id}", Call.DELETE, statusCode: HttpStatusCode.NoContent);
        }

        #endregion Events

        #region Calendars

        public Task<Calendar> GetCalendar(string id = null)
        {
            if(id == null)
            {
                return MakeRequest<Calendar>("/me/calendar");
            } else
            {
                return MakeRequest<Calendar>($"/me/calendars/{id}");
            }
        }

        public async Task<List<Calendar>> GetMyCalendars()
        {
            return (await MakeRequest<Calendars>("/me/calendars?$top=100")).Value;
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

        #endregion Calendars

    }
}
