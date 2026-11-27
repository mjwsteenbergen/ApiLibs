using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.Todoist
{
    public partial class SyncRoot
    {
        [JsonPropertyName("tooltips")]
        public Tooltips Tooltips { get; set; }

        [JsonPropertyName("filters")]
        public List<Filter> Filters { get; set; }

        [JsonPropertyName("temp_id_mapping")]
        public TempIdMapping TempIdMapping { get; set; }

        [JsonPropertyName("labels")]
        public List<Label> Labels { get; set; }

        [JsonPropertyName("locations")]
        public List<List<string>> Locations { get; set; }

        [JsonPropertyName("project_notes")]
        public List<Note> ProjectNotes { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; }

        [JsonPropertyName("full_sync")]
        public bool FullSync { get; set; }

        [JsonPropertyName("sync_token")]
        public string SyncToken { get; set; }

        [JsonPropertyName("day_orders")]
        public Dictionary<string, long> DayOrders { get; set; }

        [JsonPropertyName("projects")]
        public List<Project> Projects { get; set; }

        [JsonPropertyName("collaborators")]
        public List<Collaborator> Collaborators { get; set; }

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }

        [JsonPropertyName("day_orders_timestamp")]
        public string DayOrdersTimestamp { get; set; }

        [JsonPropertyName("live_notifications_last_read_id")]
        public long LiveNotificationsLastReadId { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("incomplete_item_ids")]
        public List<object> IncompleteItemIds { get; set; }

        [JsonPropertyName("reminders")]
        public List<Reminder> Reminders { get; set; }

        [JsonPropertyName("user_settings")]
        public UserSettings UserSettings { get; set; }

        [JsonPropertyName("incomplete_project_ids")]
        public List<object> IncompleteProjectIds { get; set; }

        [JsonPropertyName("notes")]
        public List<Note> Notes { get; set; }

        [JsonPropertyName("live_notifications")]
        public List<LiveNotification> LiveNotifications { get; set; }

        [JsonPropertyName("sections")]
        public List<object> Sections { get; set; }

        [JsonPropertyName("collaborator_states")]
        public List<CollaboratorState> CollaboratorStates { get; set; }

        [JsonPropertyName("due_exceptions")]
        public List<object> DueExceptions { get; set; }
    }

    public class CompletedRoot {
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("projects")]
        public Dictionary<string, Project> Projects { get; set; }
    }

    public partial class CollaboratorState
    {
        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("project_id")]
        public long ProjectId { get; set; }
    }

    public partial class Collaborator
    {
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("image_id")]
        public object ImageId { get; set; }
    }

    public partial class Filter
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public long Color { get; set; }

        [JsonPropertyName("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonPropertyName("item_order")]
        public long ItemOrder { get; set; }

        [JsonPropertyName("is_favorite")]
        public long IsFavorite { get; set; }

        [JsonPropertyName("legacy_id"), NullValueHandling(NullHandling.Ignore)]
        public long? LegacyId { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }

    public partial class Label
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public long Color { get; set; }

        [JsonPropertyName("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonPropertyName("item_order")]
        public long ItemOrder { get; set; }

        [JsonPropertyName("is_favorite")]
        public long IsFavorite { get; set; }

        [JsonPropertyName("legacy_id"), NullValueHandling(NullHandling.Ignore)]
        public long? LegacyId { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }

    public partial class Item
    {
        [JsonPropertyName("day_order")]
        public long? DayOrder { get; set; }

        [JsonPropertyName("assigned_by_uid")]
        public long? AssignedByUid { get; set; }

        [JsonPropertyName("labels")]
        public List<long> Labels { get; set; }

        [JsonPropertyName("sync_id")]
        public object SyncId { get; set; }

        [JsonPropertyName("section_id")]
        public object SectionId { get; set; }

        [JsonPropertyName("in_history")]
        public bool? InHistory { get; set; }

        [JsonPropertyName("child_order")]
        public long? ChildOrder { get; set; }

        [JsonPropertyName("date_added")]
        public DateTimeOffset? DateAdded { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("task_id")]
        public long? TaskId { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("checked")]
        public bool? Checked { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }

        [JsonPropertyName("has_more_notes")]
        public bool? HasMoreNotes { get; set; }

        [JsonPropertyName("due")]
        public Due Due { get; set; }

        [JsonPropertyName("priority")]
        public long? Priority { get; set; }

        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }

        [JsonPropertyName("is_deleted")]
        public bool? IsDeleted { get; set; }

        [JsonPropertyName("responsible_uid")]
        public object ResponsibleUid { get; set; }

        [JsonPropertyName("project_id")]
        public long? ProjectId { get; set; }

        [JsonPropertyName("completed_date")]
        public DateTimeOffset? DateCompleted { get; set; }

        [JsonPropertyName("collapsed")]
        public bool? Collapsed { get; set; }
    }

    public partial class Due
    {
        [JsonPropertyName("date")]
        public DateTimeOffset? Date { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("is_recurring")]
        public bool IsRecurring { get; set; }

        [JsonPropertyName("string")]
        public string String { get; set; }

        [JsonPropertyName("lang")]
        public string Lang { get; set; }
    }

    public partial class LiveNotification
    {
        [JsonPropertyName("created")]
        public DateTimeOffset Created { get; set; }

        [JsonPropertyName("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonPropertyName("is_unread")]
        public long IsUnread { get; set; }

        [JsonPropertyName("notification_key")]
        public string NotificationKey { get; set; }

        [JsonPropertyName("notification_type")]
        public string NotificationType { get; set; }

        [JsonPropertyName("plan"), NullValueHandling(NullHandling.Ignore)]
        public string Plan { get; set; }

        [JsonPropertyName("quantity"), NullValueHandling(NullHandling.Ignore)]
        public long? Quantity { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("active_until"), NullValueHandling(NullHandling.Ignore)]
        public long? ActiveUntil { get; set; }

        [JsonPropertyName("item_content"), NullValueHandling(NullHandling.Ignore)]
        public string ItemContent { get; set; }

        [JsonPropertyName("item_id"), NullValueHandling(NullHandling.Ignore)]
        public long? ItemId { get; set; }

        [JsonPropertyName("responsible_uid")]
        public long? ResponsibleUid { get; set; }

        [JsonPropertyName("project_id"), NullValueHandling(NullHandling.Ignore)]
        public long? ProjectId { get; set; }

        [JsonPropertyName("assigned_by_uid"), NullValueHandling(NullHandling.Ignore)]
        public long? AssignedByUid { get; set; }

        [JsonPropertyName("from_uid"), NullValueHandling(NullHandling.Ignore)]
        public long? FromUid { get; set; }

        [JsonPropertyName("note_id"), NullValueHandling(NullHandling.Ignore)]
        public long? NoteId { get; set; }

        [JsonPropertyName("note_content"), NullValueHandling(NullHandling.Ignore)]
        public string NoteContent { get; set; }

        [JsonPropertyName("project_name"), NullValueHandling(NullHandling.Ignore)]
        public string ProjectName { get; set; }

        [JsonPropertyName("invitation_id"), NullValueHandling(NullHandling.Ignore)]
        public long? InvitationId { get; set; }

        [JsonPropertyName("top_procent"), NullValueHandling(NullHandling.Ignore)]
        public double? TopProcent { get; set; }

        [JsonPropertyName("completed_tasks"), NullValueHandling(NullHandling.Ignore)]
        public long? CompletedTasks { get; set; }

        [JsonPropertyName("legacy_id"), NullValueHandling(NullHandling.Ignore)]
        public long? LegacyId { get; set; }

        [JsonPropertyName("promo_img"), NullValueHandling(NullHandling.Ignore)]
        public Uri PromoImg { get; set; }

        [JsonPropertyName("date_reached"), NullValueHandling(NullHandling.Ignore)]
        public DateTimeOffset? DateReached { get; set; }

        [JsonPropertyName("karma_level"), NullValueHandling(NullHandling.Ignore)]
        public long? KarmaLevel { get; set; }

        [JsonPropertyName("completed_last_month"), NullValueHandling(NullHandling.Ignore)]
        public long? CompletedLastMonth { get; set; }

        [JsonPropertyName("completed_in_days"), NullValueHandling(NullHandling.Ignore)]
        public long? CompletedInDays { get; set; }
    }

    public partial class Note
    {
        [JsonPropertyName("reactions")]
        public object Reactions { get; set; }

        [JsonPropertyName("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonPropertyName("file_attachment")]
        public FileAttachment FileAttachment { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("posted_uid")]
        public long PostedUid { get; set; }

        [JsonPropertyName("uids_to_notify")]
        public object UidsToNotify { get; set; }

        [JsonPropertyName("item_id"), NullValueHandling(NullHandling.Ignore)]
        public long? ItemId { get; set; }

        [JsonPropertyName("project_id")]
        public long ProjectId { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("posted")]
        public DateTimeOffset Posted { get; set; }

        [JsonPropertyName("legacy_project_id"), NullValueHandling(NullHandling.Ignore)]
        public long? LegacyProjectId { get; set; }
    }

    public partial class FileAttachment
    {
        [JsonPropertyName("description"), NullValueHandling(NullHandling.Ignore)]
        public string Description { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("image"), NullValueHandling(NullHandling.Ignore)]
        public Uri Image { get; set; }

        [JsonPropertyName("image_width"), NullValueHandling(NullHandling.Ignore)]
        public long? ImageWidth { get; set; }

        [JsonPropertyName("favicon"), NullValueHandling(NullHandling.Ignore)]
        public Uri Favicon { get; set; }

        [JsonPropertyName("image_height"), NullValueHandling(NullHandling.Ignore)]
        public long? ImageHeight { get; set; }

        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; }
    }

    public partial class Project
    {
        [JsonPropertyName("is_favorite")]
        public bool? IsFavorite { get; set; }

        [JsonPropertyName("color")]
        public long? Color { get; set; }

        [JsonPropertyName("collapsed")]
        public bool? Collapsed { get; set; }

        [JsonPropertyName("inbox_project"), NullValueHandling(NullHandling.Ignore)]
        public bool? InboxProject { get; set; }

        [JsonPropertyName("child_order")]
        public long? ChildOrder { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("has_more_notes")]
        public bool? HasMoreNotes { get; set; }

        [JsonPropertyName("is_deleted")]
        public bool? IsDeleted { get; set; }

        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }

        [JsonPropertyName("legacy_id"), NullValueHandling(NullHandling.Ignore)]
        public long? LegacyId { get; set; }

        [JsonPropertyName("shared")]
        public bool? Shared { get; set; }

        [JsonPropertyName("is_archived")]
        public bool? IsArchived { get; set; }

        [JsonPropertyName("team_inbox"), NullValueHandling(NullHandling.Ignore)]
        public bool? TeamInbox { get; set; }
    }

    public partial class Reminder
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sync_id")]
        public object SyncId { get; set; }

        [JsonPropertyName("loc_long")]
        public string LocLong { get; set; }

        [JsonPropertyName("legacy_item_id")]
        public long LegacyItemId { get; set; }

        [JsonPropertyName("loc_lat")]
        public string LocLat { get; set; }

        [JsonPropertyName("radius")]
        public long Radius { get; set; }

        [JsonPropertyName("legacy_id")]
        public long LegacyId { get; set; }

        [JsonPropertyName("item_id")]
        public long ItemId { get; set; }

        [JsonPropertyName("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonPropertyName("notify_uid")]
        public long NotifyUid { get; set; }

        [JsonPropertyName("project_id")]
        public long ProjectId { get; set; }

        [JsonPropertyName("loc_trigger")]
        public string LocTrigger { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }

    public partial class Stats
    {
        [JsonPropertyName("days_items")]
        public List<DaysItem> DaysItems { get; set; }

        [JsonPropertyName("week_items")]
        public List<WeekItem> WeekItems { get; set; }

        [JsonPropertyName("completed_count")]
        public long CompletedCount { get; set; }
    }

    public partial class DaysItem
    {
        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("total_completed")]
        public long TotalCompleted { get; set; }
    }

    public partial class WeekItem
    {
        [JsonPropertyName("to")]
        public DateTimeOffset To { get; set; }

        [JsonPropertyName("from")]
        public DateTimeOffset From { get; set; }

        [JsonPropertyName("total_completed")]
        public long TotalCompleted { get; set; }
    }

    public partial class TempIdMapping
    {
    }

    public partial class Tooltips
    {
        [JsonPropertyName("scheduled")]
        public List<string> Scheduled { get; set; }

        [JsonPropertyName("seen")]
        public List<string> Seen { get; set; }
    }

    public partial class User
    {
        [JsonPropertyName("start_page")]
        public string StartPage { get; set; }

        [JsonPropertyName("features")]
        public Features Features { get; set; }

        [JsonPropertyName("is_premium")]
        public bool IsPremium { get; set; }

        [JsonPropertyName("is_biz_admin")]
        public bool IsBizAdmin { get; set; }

        [JsonPropertyName("sort_order")]
        public long SortOrder { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("auto_reminder")]
        public long AutoReminder { get; set; }

        [JsonPropertyName("team_inbox")]
        public long TeamInbox { get; set; }

        [JsonPropertyName("daily_goal")]
        public long DailyGoal { get; set; }

        [JsonPropertyName("share_limit")]
        public long ShareLimit { get; set; }

        [JsonPropertyName("days_off")]
        public List<long> DaysOff { get; set; }

        [JsonPropertyName("magic_num_reached")]
        public bool MagicNumReached { get; set; }

        [JsonPropertyName("next_week")]
        public long NextWeek { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("unique_prefix")]
        public long UniquePrefix { get; set; }

        [JsonPropertyName("theme")]
        public long Theme { get; set; }

        [JsonPropertyName("tz_info")]
        public TzInfo TzInfo { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("start_day")]
        public long StartDay { get; set; }

        [JsonPropertyName("legacy_inbox_project")]
        public long LegacyInboxProject { get; set; }

        [JsonPropertyName("websocket_url")]
        public string WebsocketUrl { get; set; }

        [JsonPropertyName("inbox_project")]
        public long InboxProject { get; set; }

        [JsonPropertyName("time_format")]
        public long TimeFormat { get; set; }

        [JsonPropertyName("image_id")]
        public object ImageId { get; set; }

        [JsonPropertyName("premium_until")]
        public object PremiumUntil { get; set; }

        [JsonPropertyName("business_account_id")]
        public long? BusinessAccountId { get; set; }

        [JsonPropertyName("mobile_number")]
        public object MobileNumber { get; set; }

        [JsonPropertyName("mobile_host")]
        public object MobileHost { get; set; }

        [JsonPropertyName("date_format")]
        public long DateFormat { get; set; }

        [JsonPropertyName("karma_trend")]
        public string KarmaTrend { get; set; }

        [JsonPropertyName("dateist_lang")]
        public object DateistLang { get; set; }

        [JsonPropertyName("join_date")]
        public DateTimeOffset JoinDate { get; set; }

        [JsonPropertyName("karma")]
        public long Karma { get; set; }

        [JsonPropertyName("weekly_goal")]
        public long WeeklyGoal { get; set; }

        [JsonPropertyName("default_reminder")]
        public string DefaultReminder { get; set; }

        [JsonPropertyName("dateist_inline_disabled")]
        public bool DateistInlineDisabled { get; set; }
    }

    public partial class Features
    {
        [JsonPropertyName("karma_disabled")]
        public bool KarmaDisabled { get; set; }

        [JsonPropertyName("restriction")]
        public long Restriction { get; set; }

        [JsonPropertyName("karma_vacation")]
        public bool KarmaVacation { get; set; }

        [JsonPropertyName("dateist_lang")]
        public object DateistLang { get; set; }

        [JsonPropertyName("beta")]
        public long Beta { get; set; }

        [JsonPropertyName("has_push_reminders")]
        public bool HasPushReminders { get; set; }

        [JsonPropertyName("dateist_inline_disabled")]
        public bool DateistInlineDisabled { get; set; }
    }

    public partial class TzInfo
    {
        [JsonPropertyName("hours")]
        public long Hours { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("is_dst")]
        public long IsDst { get; set; }

        [JsonPropertyName("minutes")]
        public long Minutes { get; set; }

        [JsonPropertyName("gmt_string")]
        public string GmtString { get; set; }
    }

    public partial class UserSettings
    {
        [JsonPropertyName("reminder_push")]
        public bool ReminderPush { get; set; }

        [JsonPropertyName("reminder_desktop")]
        public bool ReminderDesktop { get; set; }

        [JsonPropertyName("legacy_pricing")]
        public bool LegacyPricing { get; set; }

        [JsonPropertyName("reminder_email")]
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

        public TodoistException(TodoistError error, RequestResponse response) : base(response)
        {
            Error = error;
        }
    }

    public partial class SyncResult
    {
        [JsonPropertyName("sync_status")]
        public Dictionary<string, object> SyncStatus { get; set; }

        [JsonPropertyName("temp_id_mapping"), NullValueHandling(NullHandling.Ignore)]
        public Dictionary<string, long> TempIdMapping { get; set; }

        [JsonPropertyName("full_sync")]
        public bool FullSync { get; set; }

        [JsonPropertyName("sync_token")]
        public string SyncToken { get; set; }
    }
}
