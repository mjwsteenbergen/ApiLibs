using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.MicrosoftGraph
{
    public partial class Calendars : ValueResult<Calendar> { }

    public partial class Calendar
    {
        [JsonPropertyName("@odata.id")]
        public string OdataId { get; set; }

        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Color")]
        public string Color { get; set; }

        [JsonPropertyName("HexColor")]
        public string HexColor { get; set; }

        [JsonPropertyName("IsDefaultCalendar")]
        public bool IsDefaultCalendar { get; set; }

        [JsonPropertyName("ChangeKey")]
        public string ChangeKey { get; set; }

        [JsonPropertyName("CanShare")]
        public bool CanShare { get; set; }

        [JsonPropertyName("CanViewPrivateItems")]
        public bool CanViewPrivateItems { get; set; }

        [JsonPropertyName("IsShared")]
        public bool IsShared { get; set; }

        [JsonPropertyName("IsSharedWithMe")]
        public bool IsSharedWithMe { get; set; }

        [JsonPropertyName("CanEdit")]
        public bool CanEdit { get; set; }

        [JsonPropertyName("Owner")]
        public Owner Owner { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public partial class Owner
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Address")]
        public string Address { get; set; }
    }

    public partial class Events : ValueResult<Event> { }

    public partial class TeamsMeeting
    {
        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("creationDateTime")]
        public DateTimeOffset? CreationDateTime { get; set; }

        [JsonPropertyName("startDateTime")]
        public DateTimeOffset? StartDateTime { get; set; }

        [JsonPropertyName("endDateTime")]
        public DateTimeOffset? EndDateTime { get; set; }

        [JsonPropertyName("joinUrl")]
        public Uri JoinUrl { get; set; }

        [JsonPropertyName("joinWebUrl")]
        public Uri JoinWebUrl { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("isBroadcast")]
        public bool? IsBroadcast { get; set; }

        [JsonPropertyName("autoAdmittedUsers")]
        public string AutoAdmittedUsers { get; set; }

        [JsonPropertyName("outerMeetingAutoAdmittedUsers")]
        public object OuterMeetingAutoAdmittedUsers { get; set; }

        [JsonPropertyName("capabilities")]
        public List<object> Capabilities { get; set; }

        [JsonPropertyName("videoTeleconferenceId")]
        public object VideoTeleconferenceId { get; set; }

        [JsonPropertyName("externalId")]
        public object ExternalId { get; set; }

        [JsonPropertyName("audioConferencing")]
        public object AudioConferencing { get; set; }

        [JsonPropertyName("meetingInfo")]
        public object MeetingInfo { get; set; }

        [JsonPropertyName("participants")]
        public Participants Participants { get; set; }

        [JsonPropertyName("chatInfo")]
        public ChatInfo ChatInfo { get; set; }

        [JsonPropertyName("lobbyBypassSettings")]
        public LobbyBypassSettings LobbyBypassSettings { get; set; }

        [JsonPropertyName("joinInformation")]
        public JoinInformation JoinInformation { get; set; }

        [JsonPropertyName("allowedPresenters")]
        public string AllowedPresenters { get; set; }
    }

    public class LobbyBypassSettings {
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("isDialInBypassEnabled")]
        public bool IsDialInBypassEnabled { get; set; }
    }

    public partial class ChatInfo
    {
        [JsonPropertyName("threadId")]
        public string ThreadId { get; set; }

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }

        [JsonPropertyName("replyChainMessageId")]
        public object ReplyChainMessageId { get; set; }
    }

    public partial class JoinInformation
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }
    }

    public partial class Participants
    {
        [JsonPropertyName("organizer")]
        public Organizer Organizer { get; set; }

        [JsonPropertyName("attendees")]
        public List<object> Attendees { get; set; }
    }

    public partial class Organizer
    {
        [JsonPropertyName("upn")]
        public string Upn { get; set; }

        [JsonPropertyName("identity")]
        public Dictionary<string, Identity> Identity { get; set; }
    }

    public partial class Identity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        public object DisplayName { get; set; }

        [JsonPropertyName("tenantId")]
        public string TenantId { get; set; }

        [JsonPropertyName("identityProvider")]
        public string IdentityProvider { get; set; }
    }

    public partial class Event
    {
        [JsonPropertyName("@odata.id")]
        public string OdataId { get; set; }

        [JsonPropertyName("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("CreatedDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonPropertyName("LastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonPropertyName("ChangeKey")]
        public string ChangeKey { get; set; }

        [JsonPropertyName("Categories")]
        public List<object> Categories { get; set; }

        [JsonPropertyName("OriginalStartTimeZone")]
        public string OriginalStartTimeZone { get; set; }

        [JsonPropertyName("OriginalEndTimeZone")]
        public string OriginalEndTimeZone { get; set; }

        [JsonPropertyName("iCalUId")]
        public string ICalUId { get; set; }

        [JsonPropertyName("ReminderMinutesBeforeStart")]
        public long ReminderMinutesBeforeStart { get; set; }

        [JsonPropertyName("IsReminderOn")]
        public bool IsReminderOn { get; set; }

        [JsonPropertyName("HasAttachments")]
        public bool HasAttachments { get; set; }

        [JsonPropertyName("Subject")]
        public string Subject { get; set; }

        [JsonPropertyName("BodyPreview")]
        public string BodyPreview { get; set; }

        [JsonPropertyName("Importance")]
        public string Importance { get; set; }

        [JsonPropertyName("Sensitivity")]
        public string Sensitivity { get; set; }

        [JsonPropertyName("IsAllDay")]
        public bool IsAllDay { get; set; }

        [JsonPropertyName("IsCancelled")]
        public bool IsCancelled { get; set; }

        [JsonPropertyName("IsOrganizer")]
        public bool IsOrganizer { get; set; }

        [JsonPropertyName("ResponseRequested")]
        public bool ResponseRequested { get; set; }

        [JsonPropertyName("SeriesMasterId")]
        public object SeriesMasterId { get; set; }

        [JsonPropertyName("ShowAs")]
        public string ShowAs { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("WebLink")]
        public string WebLink { get; set; }

        [JsonPropertyName("OnlineMeetingUrl")]
        public string OnlineMeetingUrl { get; set; }

        [JsonPropertyName("ResponseStatus")]
        public Status ResponseStatus { get; set; }

        [JsonPropertyName("Body")]
        public Body Body { get; set; }

        [JsonPropertyName("Start")]
        public EventTime Start { get; set; }

        [JsonPropertyName("End")]
        public EventTime End { get; set; }

        [JsonPropertyName("Location")]
        public Location Location { get; set; }

        [JsonPropertyName("Locations")]
        public List<Location> Locations { get; set; }

        [JsonPropertyName("Recurrence")]
        public Recurrence Recurrence { get; set; }

        [JsonPropertyName("Attendees")]
        public List<Attendee> Attendees { get; set; }

        [JsonPropertyName("Organizer")]
        public Organizer Organizer { get; set; }

        [JsonPropertyName("OnlineMeeting")]
        public object OnlineMeeting { get; set; }

        [JsonPropertyName("Calendar@odata.associationLink")]
        public string CalendarOdataAssociationLink { get; set; }

        [JsonPropertyName("Calendar@odata.navigationLink")]
        public string CalendarOdataNavigationLink { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Event @event &&
                   Id == @event.Id;
        }

        public override int GetHashCode()
        {
            int hashCode = 1854872293;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            return hashCode;
        }

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

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Status")]
        [JsonIgnore]
        public Status Status { get; set; }

        [JsonPropertyName("EmailAddress")]
        public EmailAddress EmailAddress { get; set; }
    }

    public partial class Status
    {
        [JsonPropertyName("Response")]
        public Response Response { get; set; }

        [JsonPropertyName("Time")]
        public DateTimeOffset Time { get; set; }
    }

    public partial class EventTime
    {
        public EventTime()
        {

        }

        public EventTime(DateTime dt)
        {
            DateTime = dt.ToUniversalTime();
            TimeZone = "UTC";
        }

        [JsonIgnore]
        public DateTimeOffset LocalDateTimeOffset => DateTime.LocalDateTime; 

        [JsonPropertyName("DateTime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonPropertyName("TimeZone")]
        public string TimeZone { get; set; }
    }

    public partial class Location
    {
        [JsonPropertyName("DisplayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("LocationType")]
        public string LocationType { get; set; }

        [JsonPropertyName("UniqueIdType")]
        public string UniqueIdType { get; set; }

        [JsonPropertyName("Address")]
        public PhysicalAddress Address { get; set; }

        [JsonPropertyName("Coordinates")]
        public Coordinates Coordinates { get; set; }

        [JsonPropertyName("LocationUri"), NullValueHandling(NullHandling.Ignore)]
        public string LocationUri { get; set; }

        [JsonPropertyName("UniqueId"), NullValueHandling(NullHandling.Ignore)]
        public string UniqueId { get; set; }
    }

    public partial class PhysicalAddress
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Street"), NullValueHandling(NullHandling.Ignore)]
        public string Street { get; set; }

        [JsonPropertyName("City"), NullValueHandling(NullHandling.Ignore)]
        public string City { get; set; }

        [JsonPropertyName("State"), NullValueHandling(NullHandling.Ignore)]
        public string State { get; set; }

        [JsonPropertyName("CountryOrRegion"), NullValueHandling(NullHandling.Ignore)]
        public string CountryOrRegion { get; set; }

        [JsonPropertyName("PostalCode"), NullValueHandling(NullHandling.Ignore)]
        public string PostalCode { get; set; }

        public override string ToString()
        {
            return $"{Street}, {PostalCode}, {City}, {CountryOrRegion}";
        }
    }

    public partial class Coordinates
    {
        public Coordinates() { }

        public Coordinates(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [JsonPropertyName("Latitude"), NullValueHandling(NullHandling.Ignore)]
        public double? Latitude { get; set; }

        [JsonPropertyName("Longitude"), NullValueHandling(NullHandling.Ignore)]
        public double? Longitude { get; set; }

        public override string ToString()
        {
            return $"{Latitude} {Longitude}";
        }
    }

    public partial class Organizer
    {
        [JsonPropertyName("EmailAddress")]
        public EmailAddress EmailAddress { get; set; }
    }

    public partial class Recurrence
    {
        [JsonPropertyName("Pattern")]
        public Pattern Pattern { get; set; }

        [JsonPropertyName("Range")]
        public Range Range { get; set; }
    }

    public partial class Pattern
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Interval")]
        public long Interval { get; set; }

        [JsonPropertyName("Month")]
        public long Month { get; set; }

        [JsonPropertyName("DayOfMonth")]
        public long DayOfMonth { get; set; }

        [JsonPropertyName("DaysOfWeek")]
        public List<string> DaysOfWeek { get; set; }

        [JsonPropertyName("FirstDayOfWeek")]
        public string FirstDayOfWeek { get; set; }

        [JsonPropertyName("Index")]
        public string Index { get; set; }
    }

    public partial class Range
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("RecurrenceTimeZone")]
        public string RecurrenceTimeZone { get; set; }

        [JsonPropertyName("NumberOfOccurrences")]
        public long NumberOfOccurrences { get; set; }
    }

    public enum Response { Accepted, None, Organizer, NotResponded, TentativelyAccepted };

    public partial class EventChanges
    {
        [JsonPropertyName("attendees")]
        public List<Attendee> Attendees { get; set; }

        [JsonPropertyName("body")]
        public Body Body { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("showAs")]
        public string ShowAs { get; set; }

        [JsonPropertyName("importance")]
        public string Importance { get; set; }

        [JsonPropertyName("sensitivity")]
        public string Sensitivity { get; set; }

        [JsonPropertyName("reminderMinutesBeforeStart")]
        public string ReminderBeforeStart { get; set; }

        [JsonPropertyName("start")]
        public EventTime Start { get; set; }

        [JsonPropertyName("end")]
        public EventTime End { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonIgnore]
        public string Id { get; set; }
    }

    public partial class BatchResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public long Status { get; set; }

        [JsonPropertyName("headers")]
        public Headers Headers { get; set; }

        [JsonPropertyName("body")]
        public ResponseBody Body { get; set; }
    }

    public partial class ResponseBody
    {
        [JsonPropertyName("error"), NullValueHandling(NullHandling.Ignore)]
        public Error Error { get; set; }

        [JsonPropertyName("@odata.context"), NullValueHandling(NullHandling.Ignore)]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("value"), NullValueHandling(NullHandling.Ignore)]
        public List<Event> Value { get; set; }
    }

    public partial class Error
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("innerError")]
        public InnerError InnerError { get; set; }
    }

    public partial class InnerError
    {
        [JsonPropertyName("request-id")]
        public Guid RequestId { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }
    }

    public partial class Headers
    {
        [JsonPropertyName("Cache-Control")]
        public string CacheControl { get; set; }

        [JsonPropertyName("OData-Version"), NullValueHandling(NullHandling.Ignore)]
        public string ODataVersion { get; set; }

        [JsonPropertyName("Content-Type"), NullValueHandling(NullHandling.Ignore)]
        public string ContentType { get; set; }
    }

    public partial class BatchResult
    {
        [JsonPropertyName("responses")]
        public List<BatchResponse> Responses { get; set; }
    }

    public partial class EmailAddress
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }
}
