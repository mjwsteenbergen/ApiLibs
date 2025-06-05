using System;
using System.Collections.Generic;
using System.Linq;
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
            return (await MakeRequest<Events>("me/events", parameters: data.ConvertToParams())).Value;
        }
        
        public Task<List<Event>> GetEvents(Calendar calendar, OData data = null)
        {
            return GetEvents(calendar.Id, data);
        }

        public Task<Event> GetEvent(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new ArgumentNullException(nameof(id)); }
            return MakeRequest<Event>($"me/events/{id}");
        }

        public async Task<List<Event>> GetEvents(string calendarId, OData data = null)
        {
            data = data ?? new OData
            {
                Top = 20
            };
            return (await MakeRequest<Events>($"me/calendars/{calendarId}/events", parameters: data.ConvertToParams())).Value;
        }


        public Task<List<Event>> GetCalendarView(Calendar calendar, DateTime startTime, DateTime endTime, OData data = null)
        {
            return GetCalendarView(calendar.Id, startTime, endTime, data);
        }

        public async Task<List<Event>> GetCalendarView(string calendarId, DateTime startTime, DateTime endTime, OData data = null)
        {
            data = data ?? new OData();
            Events events = await MakeRequest<Events>($"me/calendars/{calendarId}/calendarView", parameters: new List<ApiLibs.Param>
            {
                new Param("endDateTime", endTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff", System.Globalization.CultureInfo.InvariantCulture)),
                new Param("startDateTime", startTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff", System.Globalization.CultureInfo.InvariantCulture))
            }.Union(data.ConvertToParams()).ToList());
            return events.Value;
        }

        public async IAsyncEnumerable<Event> GetAllEventsOfAllCalendars(DateTime startTime, DateTime endTime, TimeZoneInfo timezone = null)
        {
            
            var myCals = await GetMyCalendars();

            foreach(var cal in myCals) {
                var events = await GetCalendarView(cal, startTime, endTime, new OData
                {
                    Top = 50
                });

                foreach (var ev in events)
                {
                    yield return ev;
                }
            }
            
        }

        public Task<Event> EditEvent(Event @event, EventChanges ev)
        {
            return EditEvent(@event.Id, ev);
        }

        public async Task<Event> EditEvent(string id, EventChanges ev)
        {
            return await MakeRequest<Event>($"me/events/{id}", Call.PATCH, content: ev);
        }

        public Task<Event> CreateEvent(EventChanges ev)
        {
            return MakeRequest<Event>("me/events", Call.POST, content: ev);
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
            return MakeRequest<Event>($"me/events/{id}", Call.PATCH, content: ev);
        }

        public Task DeleteEvent(Event e)
        {
            return DeleteEvent(e.Id);
        }

        public Task DeleteEvent(string id) => MakeRequest($"me/events/{id}", Call.DELETE);

        #endregion Events

        #region Calendars
        // We have to add fucking $select statements
        // Otherwise it breaks
        // https://feedbackportal.microsoft.com/feedback/idea/11e19b98-5f40-f011-a2d9-7c1e5299279a

        public Task<Calendar> GetCalendar(string id = null)
        {
            if (id == null)
            {
                return MakeRequest<Calendar>("me/calendar?$select=id,name,color,changeKey,canShare,canViewPrivateItems,hexColor,canEdit,isTallyingResponses,isRemovable,owner");
            }
            else
            {
                return MakeRequest<Calendar>($"me/calendars/{id}?$select=id,name,color,changeKey,canShare,canViewPrivateItems,hexColor,canEdit,isTallyingResponses,isRemovable,owner");
            }
        }

        public async Task<List<Calendar>> GetMyCalendars()
        {
            return (await MakeRequest<Calendars>("me/calendars?$top=100&$select=id,name,color,changeKey,canShare,canViewPrivateItems,hexColor,canEdit,isTallyingResponses,isRemovable,owner")).Value;
        }
        #endregion Caledars

    }
}
