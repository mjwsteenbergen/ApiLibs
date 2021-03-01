using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
            return (await MakeRequest<Events>("/me/events", parameters: data.ConvertToParams())).Value;
        }
        
        public Task<List<Event>> GetEvents(Calendar calendar, OData data = null)
        {
            return GetEvents(calendar.Id, data);
        }

        public Task<Event> GetEvent(string id)
        {
            return MakeRequest<Event>($"/me/events/{id}");
        }

        public async Task<List<Event>> GetEvents(string calendarId, OData data = null)
        {
            data = data ?? new OData
            {
                Top = 20
            };
            return (await MakeRequest<Events>($"/me/calendars/{calendarId}/events", parameters: data.ConvertToParams())).Value;
        }


        public Task<List<Event>> GetEvents(Calendar calendar, DateTime startTime, DateTime endTime, OData data = null)
        {
            return GetEvents(calendar.Id, startTime, endTime, data);
        }

        public async Task<List<Event>> GetEvents(string calendarId, DateTime startTime, DateTime endTime, OData data = null)
        {
            data = data ?? new OData();
            Events events = await MakeRequest<Events>($"/me/calendars/{calendarId}/calendarView", parameters: new List<ApiLibs.Param>
            {
                new Param("endDateTime", endTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff", System.Globalization.CultureInfo.InvariantCulture)),
                new Param("startDateTime", startTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff", System.Globalization.CultureInfo.InvariantCulture))
            }.Union(data.ConvertToParams()).ToList());
            return events.Value;
        }

        public async Task<List<Event>> GetAllEventsOfAllCalendars(DateTime startTime, DateTime endTime, TimeZoneInfo timezone = null)
        {
            
            var myCals = await GetMyCalendars();
            var resses = new List<Event>();
            for (int i = 0; i < myCals.Count; i+=20)
            {
                var res = await MakeRequest<BatchResult>("$batch", Call.POST, content: new
                {
                    requests = myCals.Skip(i).Take(20).Select(i => new
                    {
                        url =
                            $"/me/calendars/{i.Id}/calendarView?startdatetime={startTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}&enddatetime={endTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}&$top=50",
                        method = "GET",
                        id = i.Name,
                        headers = new
                        {
                            Prefer = $"outlook.timezone=\"{(timezone?.StandardName) ?? "UTC"}\""
                        }
                    })
                });
                resses.AddRange(res.Responses.Select(i => i.Body).Where(i => i.Error == null).SelectMany(i => i.Value));
            }
            
            return resses;
        }

        public Task<Event> EditEvent(Event @event, EventChanges ev)
        {
            return EditEvent(@event.Id, ev);
        }

        public async Task<Event> EditEvent(string id, EventChanges ev)
        {
            return await MakeRequest<Event>($"/me/events/{id}", Call.PATCH, content: ev);
        }

        public Task<Event> CreateEvent(EventChanges ev)
        {
            return MakeRequest<Event>("/me/events", Call.POST, content: ev);
        }

        public Task<Event> CreateEvent(EventChanges ev, Calendar cal)
        {
            return CreateEvent(ev, cal.Id);
        }

        private Task<Event> CreateEvent(EventChanges ev, string id)
        {
            return MakeRequest<Event>($"me/calendars/{id}/events", Call.POST, content: ev);
        }

        public Task<Event> UpdateEvent(EventChanges ev)
        {
            return UpdateEvent(ev.Id, ev);
        }

        public Task<Event> UpdateEvent(string id, EventChanges ev)
        {
            return MakeRequest<Event>($"/me/events/{id}", Call.PATCH, content: ev);
        }

        public Task DeleteEvent(Event e)
        {
            return DeleteEvent(e.Id);
        }

        public Task DeleteEvent(string id)
        {
            return MakeRequest<string>($"me/events/{id}", Call.DELETE, statusCode: HttpStatusCode.NoContent);
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
        #endregion Caledars

    }
}
