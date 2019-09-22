using ApiLibs.General;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace ApiLibs.Todoist
{
    public partial class SyncRoot
    {
        [JsonProperty("tooltips")]
        public Tooltips Tooltips { get; set; }

        [JsonProperty("filters")]
        public List<Filter> Filters { get; set; }

        [JsonProperty("temp_id_mapping")]
        public TempIdMapping TempIdMapping { get; set; }

        [JsonProperty("labels")]
        public List<Label> Labels { get; set; }

        [JsonProperty("locations")]
        public List<List<string>> Locations { get; set; }

        [JsonProperty("project_notes")]
        public List<Note> ProjectNotes { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("full_sync")]
        public bool FullSync { get; set; }

        [JsonProperty("sync_token")]
        public string SyncToken { get; set; }

        [JsonProperty("day_orders")]
        public Dictionary<string, long> DayOrders { get; set; }

        [JsonProperty("projects")]
        public List<Project> Projects { get; set; }

        [JsonProperty("collaborators")]
        public List<Collaborator> Collaborators { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("day_orders_timestamp")]
        public string DayOrdersTimestamp { get; set; }

        [JsonProperty("live_notifications_last_read_id")]
        public long LiveNotificationsLastReadId { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("incomplete_item_ids")]
        public List<object> IncompleteItemIds { get; set; }

        [JsonProperty("reminders")]
        public List<Reminder> Reminders { get; set; }

        [JsonProperty("user_settings")]
        public UserSettings UserSettings { get; set; }

        [JsonProperty("incomplete_project_ids")]
        public List<object> IncompleteProjectIds { get; set; }

        [JsonProperty("notes")]
        public List<Note> Notes { get; set; }

        [JsonProperty("live_notifications")]
        public List<LiveNotification> LiveNotifications { get; set; }

        [JsonProperty("sections")]
        public List<object> Sections { get; set; }

        [JsonProperty("collaborator_states")]
        public List<CollaboratorState> CollaboratorStates { get; set; }

        [JsonProperty("due_exceptions")]
        public List<object> DueExceptions { get; set; }
    }

    public partial class CollaboratorState
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }
    }

    public partial class Collaborator
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("image_id")]
        public object ImageId { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public long Color { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("item_order")]
        public long ItemOrder { get; set; }

        [JsonProperty("is_favorite")]
        public long IsFavorite { get; set; }

        [JsonProperty("legacy_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? LegacyId { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class Label
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public long Color { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("item_order")]
        public long ItemOrder { get; set; }

        [JsonProperty("is_favorite")]
        public long IsFavorite { get; set; }

        [JsonProperty("legacy_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? LegacyId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("day_order")]
        public long? DayOrder { get; set; }

        [JsonProperty("assigned_by_uid")]
        public long? AssignedByUid { get; set; }

        [JsonProperty("labels")]
        public List<long> Labels { get; set; }

        [JsonProperty("sync_id")]
        public object SyncId { get; set; }

        [JsonProperty("section_id")]
        public object SectionId { get; set; }

        [JsonProperty("in_history")]
        public bool? InHistory { get; set; }

        [JsonProperty("child_order")]
        public long? ChildOrder { get; set; }

        [JsonProperty("date_added")]
        public DateTimeOffset? DateAdded { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("checked")]
        public bool? Checked { get; set; }

        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        [JsonProperty("has_more_notes")]
        public bool? HasMoreNotes { get; set; }

        [JsonProperty("due")]
        public Due Due { get; set; }

        [JsonProperty("priority")]
        public long? Priority { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("is_deleted")]
        public bool? IsDeleted { get; set; }

        [JsonProperty("responsible_uid")]
        public object ResponsibleUid { get; set; }

        [JsonProperty("project_id")]
        public long? ProjectId { get; set; }

        [JsonProperty("date_completed")]
        public DateTimeOffset? DateCompleted { get; set; }

        [JsonProperty("collapsed")]
        public bool? Collapsed { get; set; }
    }

    public partial class Due
    {
        [JsonProperty("date")]
        public DateTimeOffset? Date { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("is_recurring")]
        public bool IsRecurring { get; set; }

        [JsonProperty("string")]
        public string String { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }
    }

    public partial class LiveNotification
    {
        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("is_unread")]
        public long IsUnread { get; set; }

        [JsonProperty("notification_key")]
        public string NotificationKey { get; set; }

        [JsonProperty("notification_type")]
        public string NotificationType { get; set; }

        [JsonProperty("plan", NullValueHandling = NullValueHandling.Ignore)]
        public string Plan { get; set; }

        [JsonProperty("quantity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Quantity { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("active_until", NullValueHandling = NullValueHandling.Ignore)]
        public long? ActiveUntil { get; set; }

        [JsonProperty("item_content", NullValueHandling = NullValueHandling.Ignore)]
        public string ItemContent { get; set; }

        [JsonProperty("item_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? ItemId { get; set; }

        [JsonProperty("responsible_uid")]
        public long? ResponsibleUid { get; set; }

        [JsonProperty("project_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? ProjectId { get; set; }

        [JsonProperty("assigned_by_uid", NullValueHandling = NullValueHandling.Ignore)]
        public long? AssignedByUid { get; set; }

        [JsonProperty("from_uid", NullValueHandling = NullValueHandling.Ignore)]
        public long? FromUid { get; set; }

        [JsonProperty("note_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? NoteId { get; set; }

        [JsonProperty("note_content", NullValueHandling = NullValueHandling.Ignore)]
        public string NoteContent { get; set; }

        [JsonProperty("project_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ProjectName { get; set; }

        [JsonProperty("invitation_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? InvitationId { get; set; }

        [JsonProperty("top_procent", NullValueHandling = NullValueHandling.Ignore)]
        public double? TopProcent { get; set; }

        [JsonProperty("completed_tasks", NullValueHandling = NullValueHandling.Ignore)]
        public long? CompletedTasks { get; set; }

        [JsonProperty("legacy_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? LegacyId { get; set; }

        [JsonProperty("promo_img", NullValueHandling = NullValueHandling.Ignore)]
        public Uri PromoImg { get; set; }

        [JsonProperty("date_reached", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DateReached { get; set; }

        [JsonProperty("karma_level", NullValueHandling = NullValueHandling.Ignore)]
        public long? KarmaLevel { get; set; }

        [JsonProperty("completed_last_month", NullValueHandling = NullValueHandling.Ignore)]
        public long? CompletedLastMonth { get; set; }

        [JsonProperty("completed_in_days", NullValueHandling = NullValueHandling.Ignore)]
        public long? CompletedInDays { get; set; }
    }

    public partial class Note
    {
        [JsonProperty("reactions")]
        public object Reactions { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("file_attachment")]
        public FileAttachment FileAttachment { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("posted_uid")]
        public long PostedUid { get; set; }

        [JsonProperty("uids_to_notify")]
        public object UidsToNotify { get; set; }

        [JsonProperty("item_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? ItemId { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("posted")]
        public DateTimeOffset Posted { get; set; }

        [JsonProperty("legacy_project_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? LegacyProjectId { get; set; }
    }

    public partial class FileAttachment
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Image { get; set; }

        [JsonProperty("image_width", NullValueHandling = NullValueHandling.Ignore)]
        public long? ImageWidth { get; set; }

        [JsonProperty("favicon", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Favicon { get; set; }

        [JsonProperty("image_height", NullValueHandling = NullValueHandling.Ignore)]
        public long? ImageHeight { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }
    }

    public partial class Project
    {
        [JsonProperty("is_favorite")]
        public bool? IsFavorite { get; set; }

        [JsonProperty("color")]
        public long? Color { get; set; }

        [JsonProperty("collapsed")]
        public bool? Collapsed { get; set; }

        [JsonProperty("inbox_project", NullValueHandling = NullValueHandling.Ignore)]
        public bool? InboxProject { get; set; }

        [JsonProperty("child_order")]
        public long? ChildOrder { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("has_more_notes")]
        public bool? HasMoreNotes { get; set; }

        [JsonProperty("is_deleted")]
        public bool? IsDeleted { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("legacy_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? LegacyId { get; set; }

        [JsonProperty("shared")]
        public bool? Shared { get; set; }

        [JsonProperty("is_archived")]
        public bool? IsArchived { get; set; }

        [JsonProperty("team_inbox", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TeamInbox { get; set; }
    }

    public partial class Reminder
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sync_id")]
        public object SyncId { get; set; }

        [JsonProperty("loc_long")]
        public string LocLong { get; set; }

        [JsonProperty("legacy_item_id")]
        public long LegacyItemId { get; set; }

        [JsonProperty("loc_lat")]
        public string LocLat { get; set; }

        [JsonProperty("radius")]
        public long Radius { get; set; }

        [JsonProperty("legacy_id")]
        public long LegacyId { get; set; }

        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("notify_uid")]
        public long NotifyUid { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("loc_trigger")]
        public string LocTrigger { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class Stats
    {
        [JsonProperty("days_items")]
        public List<DaysItem> DaysItems { get; set; }

        [JsonProperty("week_items")]
        public List<WeekItem> WeekItems { get; set; }

        [JsonProperty("completed_count")]
        public long CompletedCount { get; set; }
    }

    public partial class DaysItem
    {
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("total_completed")]
        public long TotalCompleted { get; set; }
    }

    public partial class WeekItem
    {
        [JsonProperty("to")]
        public DateTimeOffset To { get; set; }

        [JsonProperty("from")]
        public DateTimeOffset From { get; set; }

        [JsonProperty("total_completed")]
        public long TotalCompleted { get; set; }
    }

    public partial class TempIdMapping
    {
    }

    public partial class Tooltips
    {
        [JsonProperty("scheduled")]
        public List<string> Scheduled { get; set; }

        [JsonProperty("seen")]
        public List<string> Seen { get; set; }
    }

    public partial class User
    {
        [JsonProperty("start_page")]
        public string StartPage { get; set; }

        [JsonProperty("features")]
        public Features Features { get; set; }

        [JsonProperty("is_premium")]
        public bool IsPremium { get; set; }

        [JsonProperty("is_biz_admin")]
        public bool IsBizAdmin { get; set; }

        [JsonProperty("sort_order")]
        public long SortOrder { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("auto_reminder")]
        public long AutoReminder { get; set; }

        [JsonProperty("team_inbox")]
        public long TeamInbox { get; set; }

        [JsonProperty("daily_goal")]
        public long DailyGoal { get; set; }

        [JsonProperty("share_limit")]
        public long ShareLimit { get; set; }

        [JsonProperty("days_off")]
        public List<long> DaysOff { get; set; }

        [JsonProperty("magic_num_reached")]
        public bool MagicNumReached { get; set; }

        [JsonProperty("next_week")]
        public long NextWeek { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("unique_prefix")]
        public long UniquePrefix { get; set; }

        [JsonProperty("theme")]
        public long Theme { get; set; }

        [JsonProperty("tz_info")]
        public TzInfo TzInfo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("start_day")]
        public long StartDay { get; set; }

        [JsonProperty("legacy_inbox_project")]
        public long LegacyInboxProject { get; set; }

        [JsonProperty("websocket_url")]
        public string WebsocketUrl { get; set; }

        [JsonProperty("inbox_project")]
        public long InboxProject { get; set; }

        [JsonProperty("time_format")]
        public long TimeFormat { get; set; }

        [JsonProperty("image_id")]
        public object ImageId { get; set; }

        [JsonProperty("premium_until")]
        public object PremiumUntil { get; set; }

        [JsonProperty("business_account_id")]
        public long BusinessAccountId { get; set; }

        [JsonProperty("mobile_number")]
        public object MobileNumber { get; set; }

        [JsonProperty("mobile_host")]
        public object MobileHost { get; set; }

        [JsonProperty("date_format")]
        public long DateFormat { get; set; }

        [JsonProperty("karma_trend")]
        public string KarmaTrend { get; set; }

        [JsonProperty("dateist_lang")]
        public object DateistLang { get; set; }

        [JsonProperty("join_date")]
        public DateTimeOffset JoinDate { get; set; }

        [JsonProperty("karma")]
        public long Karma { get; set; }

        [JsonProperty("weekly_goal")]
        public long WeeklyGoal { get; set; }

        [JsonProperty("default_reminder")]
        public string DefaultReminder { get; set; }

        [JsonProperty("dateist_inline_disabled")]
        public bool DateistInlineDisabled { get; set; }
    }

    public partial class Features
    {
        [JsonProperty("karma_disabled")]
        public bool KarmaDisabled { get; set; }

        [JsonProperty("restriction")]
        public long Restriction { get; set; }

        [JsonProperty("karma_vacation")]
        public bool KarmaVacation { get; set; }

        [JsonProperty("dateist_lang")]
        public object DateistLang { get; set; }

        [JsonProperty("beta")]
        public long Beta { get; set; }

        [JsonProperty("has_push_reminders")]
        public bool HasPushReminders { get; set; }

        [JsonProperty("dateist_inline_disabled")]
        public bool DateistInlineDisabled { get; set; }
    }

    public partial class TzInfo
    {
        [JsonProperty("hours")]
        public long Hours { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("is_dst")]
        public long IsDst { get; set; }

        [JsonProperty("minutes")]
        public long Minutes { get; set; }

        [JsonProperty("gmt_string")]
        public string GmtString { get; set; }
    }

    public partial class UserSettings
    {
        [JsonProperty("reminder_push")]
        public bool ReminderPush { get; set; }

        [JsonProperty("reminder_desktop")]
        public bool ReminderDesktop { get; set; }

        [JsonProperty("legacy_pricing")]
        public bool LegacyPricing { get; set; }

        [JsonProperty("reminder_email")]
        public bool ReminderEmail { get; set; }
    }

    public class SearchResult
    {
        public string query { get; set; }
        public string type { get; set; }
        public List<Item> data { get; set; }
    }


    public class TodoistError
    {
        public string error_tag { get; set; }
        public int error_code { get; set; }
        public string error { get; set; }

        public int http_code { get; set; }
    }

    public class TodoistException : RequestException
    {
        public TodoistError Error { private set; get; }

        public TodoistException(TodoistError error, int statusCode, string statusDescription, string responseUri, string errorMessage, string content)
            : base(statusCode, statusDescription, responseUri, error.error, content, null)
        {
            Error = error;
        }
    }

    public partial class SyncResult
    {
        [JsonProperty("sync_status")]
        public Dictionary<string, object> SyncStatus { get; set; }

        [JsonProperty("temp_id_mapping", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, long> TempIdMapping { get; set; }

        [JsonProperty("full_sync")]
        public bool FullSync { get; set; }

        [JsonProperty("sync_token")]
        public string SyncToken { get; set; }
    }
}
