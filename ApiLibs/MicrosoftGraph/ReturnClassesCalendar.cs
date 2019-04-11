using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.MicrosoftGraph
{
    public partial class Calendars
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("value")]
        public List<Calendar> Value { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string OdataNextLink { get; set; }
    }

    public partial class Calendar
    {
        [JsonProperty("@odata.id")]
        public string OdataId { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Color")]
        public string Color { get; set; }

        [JsonProperty("HexColor")]
        public string HexColor { get; set; }

        [JsonProperty("IsDefaultCalendar")]
        public bool IsDefaultCalendar { get; set; }

        [JsonProperty("ChangeKey")]
        public string ChangeKey { get; set; }

        [JsonProperty("CanShare")]
        public bool CanShare { get; set; }

        [JsonProperty("CanViewPrivateItems")]
        public bool CanViewPrivateItems { get; set; }

        [JsonProperty("IsShared")]
        public bool IsShared { get; set; }

        [JsonProperty("IsSharedWithMe")]
        public bool IsSharedWithMe { get; set; }

        [JsonProperty("CanEdit")]
        public bool CanEdit { get; set; }

        [JsonProperty("Owner")]
        public Owner Owner { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class Owner
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }
    }

    public partial class Events
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("value")]
        public List<Event> Value { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string OdataNextLink { get; set; }
    }

    public partial class Event
    {
        [JsonProperty("@odata.id")]
        public string OdataId { get; set; }

        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("CreatedDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("LastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("ChangeKey")]
        public string ChangeKey { get; set; }

        [JsonProperty("Categories")]
        public List<object> Categories { get; set; }

        [JsonProperty("OriginalStartTimeZone")]
        public string OriginalStartTimeZone { get; set; }

        [JsonProperty("OriginalEndTimeZone")]
        public string OriginalEndTimeZone { get; set; }

        [JsonProperty("iCalUId")]
        public string ICalUId { get; set; }

        [JsonProperty("ReminderMinutesBeforeStart")]
        public long ReminderMinutesBeforeStart { get; set; }

        [JsonProperty("IsReminderOn")]
        public bool IsReminderOn { get; set; }

        [JsonProperty("HasAttachments")]
        public bool HasAttachments { get; set; }

        [JsonProperty("Subject")]
        public string Subject { get; set; }

        [JsonProperty("BodyPreview")]
        public string BodyPreview { get; set; }

        [JsonProperty("Importance")]
        public string Importance { get; set; }

        [JsonProperty("Sensitivity")]
        public string Sensitivity { get; set; }

        [JsonProperty("IsAllDay")]
        public bool IsAllDay { get; set; }

        [JsonProperty("IsCancelled")]
        public bool IsCancelled { get; set; }

        [JsonProperty("IsOrganizer")]
        public bool IsOrganizer { get; set; }

        [JsonProperty("ResponseRequested")]
        public bool ResponseRequested { get; set; }

        [JsonProperty("SeriesMasterId")]
        public object SeriesMasterId { get; set; }

        [JsonProperty("ShowAs")]
        public string ShowAs { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("WebLink")]
        public string WebLink { get; set; }

        [JsonProperty("OnlineMeetingUrl")]
        public string OnlineMeetingUrl { get; set; }

        [JsonProperty("ResponseStatus")]
        public Status ResponseStatus { get; set; }

        [JsonProperty("Body")]
        public Body Body { get; set; }

        [JsonProperty("Start")]
        public EventTime Start { get; set; }

        [JsonProperty("End")]
        public EventTime EventTime { get; set; }

        [JsonProperty("Location")]
        public Location Location { get; set; }

        [JsonProperty("Locations")]
        public List<Location> Locations { get; set; }

        [JsonProperty("Recurrence")]
        public Recurrence Recurrence { get; set; }

        [JsonProperty("Attendees")]
        public List<Attendee> Attendees { get; set; }

        [JsonProperty("Organizer")]
        public Organizer Organizer { get; set; }

        [JsonProperty("OnlineMeeting")]
        public object OnlineMeeting { get; set; }

        [JsonProperty("Calendar@odata.associationLink")]
        public string CalendarOdataAssociationLink { get; set; }

        [JsonProperty("Calendar@odata.navigationLink")]
        public string CalendarOdataNavigationLink { get; set; }

        public override string ToString()
        {
            return Subject;
        }
    }

    public partial class Attendee
    {
        public Attendee()
        {
            Type = "required";
        }

        public Attendee(string name, string emailAdress) : this()
        {
            EmailAddress = new EmailAddress
            {
                Name = name,
                Address = emailAdress
            };
        }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Status")]
        [JsonIgnore]
        public Status Status { get; set; }

        [JsonProperty("EmailAddress")]
        public EmailAddress EmailAddress { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("Response")]
        public Response Response { get; set; }

        [JsonProperty("Time")]
        public DateTimeOffset Time { get; set; }
    }

    public partial class EventTime
    {
        public EventTime()
        {

        }

        public EventTime(DateTimeOffset Dt)
        {
            DateTime = Dt.UtcDateTime;
            TimeZone = "UTC";
        }

        [JsonIgnore]
        public DateTimeOffset LocalDateTimeOffset => DateTime.LocalDateTime; 

        [JsonProperty("DateTime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonProperty("TimeZone")]
        public string TimeZone { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("LocationType")]
        public string LocationType { get; set; }

        [JsonProperty("UniqueIdType")]
        public string UniqueIdType { get; set; }

        [JsonProperty("Address")]
        public PhysicalAddress Address { get; set; }

        [JsonProperty("Coordinates")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("LocationUri", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationUri { get; set; }

        [JsonProperty("UniqueId", NullValueHandling = NullValueHandling.Ignore)]
        public string UniqueId { get; set; }
    }

    public partial class PhysicalAddress
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Street", NullValueHandling = NullValueHandling.Ignore)]
        public string Street { get; set; }

        [JsonProperty("City", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty("State", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("CountryOrRegion", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryOrRegion { get; set; }

        [JsonProperty("PostalCode", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCode { get; set; }

        public override string ToString()
        {
            return $"{Street}, {PostalCode}, {City}, {CountryOrRegion}";
        }
    }

    public partial class Coordinates
    {
        public Coordinates(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [JsonProperty("Latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? Latitude { get; set; }

        [JsonProperty("Longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? Longitude { get; set; }
    }

    public partial class Organizer
    {
        [JsonProperty("EmailAddress")]
        public EmailAddress EmailAddress { get; set; }
    }

    public partial class Recurrence
    {
        [JsonProperty("Pattern")]
        public Pattern Pattern { get; set; }

        [JsonProperty("Range")]
        public Range Range { get; set; }
    }

    public partial class Pattern
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Interval")]
        public long Interval { get; set; }

        [JsonProperty("Month")]
        public long Month { get; set; }

        [JsonProperty("DayOfMonth")]
        public long DayOfMonth { get; set; }

        [JsonProperty("DaysOfWeek")]
        public List<string> DaysOfWeek { get; set; }

        [JsonProperty("FirstDayOfWeek")]
        public string FirstDayOfWeek { get; set; }

        [JsonProperty("Index")]
        public string Index { get; set; }
    }

    public partial class Range
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("EndDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("RecurrenceTimeZone")]
        public string RecurrenceTimeZone { get; set; }

        [JsonProperty("NumberOfOccurrences")]
        public long NumberOfOccurrences { get; set; }
    }

    public enum Response { Accepted, None, Organizer, NotResponded, TentativelyAccepted };

    public partial class NewEvent
    {
        [JsonProperty("attendees")]
        public List<Attendee> Attendees { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("showAs")]
        public string ShowAs { get; set; }

        [JsonProperty("importance")]
        public string Importance { get; set; }

        [JsonProperty("sensitivity")]
        public string Sensitivity { get; set; }

        [JsonProperty("reminderMinutesBeforeStart")]
        public string ReminderBeforeStart { get; set; }

        [JsonProperty("start")]
        public EventTime Start { get; set; }

        [JsonProperty("end")]
        public EventTime End { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonIgnore]
        public string Id { get; set; }
    }

    public partial class BatchResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("headers")]
        public Headers Headers { get; set; }

        [JsonProperty("body")]
        public ResponseBody Body { get; set; }
    }

    public partial class ResponseBody
    {
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public Error Error { get; set; }

        [JsonProperty("@odata.context", NullValueHandling = NullValueHandling.Ignore)]
        public Uri OdataContext { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public List<Event> Value { get; set; }
    }

    public partial class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("innerError")]
        public InnerError InnerError { get; set; }
    }

    public partial class InnerError
    {
        [JsonProperty("request-id")]
        public Guid RequestId { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }
    }

    public partial class Headers
    {
        [JsonProperty("Cache-Control")]
        public string CacheControl { get; set; }

        [JsonProperty("OData-Version", NullValueHandling = NullValueHandling.Ignore)]
        public string ODataVersion { get; set; }

        [JsonProperty("Content-Type", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; }
    }

    public partial class BatchResult
    {
        [JsonProperty("responses")]
        public List<BatchResponse> Responses { get; set; }
    }

    public partial class EmailAddress
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
