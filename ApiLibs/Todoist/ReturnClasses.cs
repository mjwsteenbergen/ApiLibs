using ApiLibs.General;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ApiLibs.Todoist
{
    public partial class SyncRoot
    {
        [JsonProperty("full_sync")]
        public bool FullSync { get; set; }

        [JsonProperty("temp_id_mapping")]
        public TempIdMapping TempIdMapping { get; set; }

        [JsonProperty("labels")]
        public List<Label> Labels { get; set; }

        [JsonProperty("locations")]
        public List<List<string>> Locations { get; set; }

        [JsonProperty("project_notes")]
        public List<object> ProjectNotes { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("filters")]
        public List<Filter> Filters { get; set; }

        [JsonProperty("sync_token")]
        public string SyncToken { get; set; }

        [JsonProperty("day_orders")]
        public Dictionary<long, long> DayOrders { get; set; }

        [JsonProperty("projects")]
        public List<Project> Projects { get; set; }

        [JsonProperty("collaborators")]
        public List<object> Collaborators { get; set; }

        [JsonProperty("day_orders_timestamp")]
        public string DayOrdersTimestamp { get; set; }

        [JsonProperty("live_notifications_last_read_id")]
        public long LiveNotificationsLastReadId { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("notes")]
        public List<Note> Notes { get; set; }

        [JsonProperty("reminders")]
        public List<Reminder> Reminders { get; set; }

        [JsonProperty("live_notifications")]
        public List<LiveNotification> LiveNotifications { get; set; }

        [JsonProperty("collaborator_states")]
        public List<object> CollaboratorStates { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public long Color { get; set; }

        [JsonProperty("item_order")]
        public long ItemOrder { get; set; }

        [JsonProperty("is_favorite")]
        public long IsFavorite { get; set; }

        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public string Query { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }


    public partial class Label
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public long Color { get; set; }

        [JsonProperty("item_order")]
        public long ItemOrder { get; set; }

        [JsonProperty("is_favorite")]
        public long IsFavorite { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("day_order")]
        public long DayOrder { get; set; }

        [JsonProperty("assigned_by_uid")]
        public object AssignedByUid { get; set; }

        [JsonProperty("is_archived")]
        public long IsArchived { get; set; }

        [JsonProperty("labels")]
        public List<long> Labels { get; set; }

        [JsonProperty("sync_id")]
        public object SyncId { get; set; }

        [JsonProperty("date_completed")]
        public DateTime? DateCompleted { get; set; }

        [JsonProperty("all_day")]
        public bool AllDay { get; set; }

        [JsonProperty("in_history")]
        public long InHistory { get; set; }

        [JsonProperty("date_added")]
        public string DateAdded { get; set; }

        [JsonProperty("indent")]
        public long Indent { get; set; }

        [JsonProperty("date_lang")]
        public string DateLang { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("priority")]
        public long Priority { get; set; }

        [JsonProperty("checked")]
        public long Checked { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("has_more_notes")]
        public bool HasMoreNotes { get; set; }

        [JsonProperty("due_date_utc")]
        public DateTime? DueDateUtc { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("parent_id")]
        public object ParentId { get; set; }

        [JsonProperty("item_order")]
        public long ItemOrder { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("responsible_uid")]
        public object ResponsibleUid { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("collapsed")]
        public long Collapsed { get; set; }

        [JsonProperty("date_string")]
        public string DateString { get; set; }
    }

    public partial class LiveNotification
    {
        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("top_procent", NullValueHandling = NullValueHandling.Ignore)]
        public double? TopProcent { get; set; }

        [JsonProperty("completed_tasks", NullValueHandling = NullValueHandling.Ignore)]
        public long? CompletedTasks { get; set; }

        [JsonProperty("notification_key")]
        public string NotificationKey { get; set; }

        [JsonProperty("notification_type")]
        public string NotificationType { get; set; }

        [JsonProperty("promo_img")]
        public Uri PromoImg { get; set; }

        [JsonProperty("date_reached", NullValueHandling = NullValueHandling.Ignore)]
        public long? DateReached { get; set; }

        [JsonProperty("karma_level")]
        public long KarmaLevel { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("is_unread")]
        public long IsUnread { get; set; }

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

        [JsonProperty("is_archived")]
        public long IsArchived { get; set; }

        [JsonProperty("file_attachment")]
        public FileAttachment FileAttachment { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("posted_uid")]
        public long PostedUid { get; set; }

        [JsonProperty("uids_to_notify")]
        public object UidsToNotify { get; set; }

        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("posted")]
        public string Posted { get; set; }
    }

    public partial class FileAttachment
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class Project
    {
        [JsonProperty("color")]
        public long Color { get; set; }

        [JsonProperty("collapsed")]
        public long Collapsed { get; set; }

        [JsonProperty("inbox_project", NullValueHandling = NullValueHandling.Ignore)]
        public bool? InboxProject { get; set; }

        [JsonProperty("is_favorite")]
        public long IsFavorite { get; set; }

        [JsonProperty("indent")]
        public long Indent { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("has_more_notes")]
        public bool HasMoreNotes { get; set; }

        [JsonProperty("parent_id")]
        public object ParentId { get; set; }

        [JsonProperty("item_order")]
        public long ItemOrder { get; set; }

        [JsonProperty("shared")]
        public bool Shared { get; set; }

        [JsonProperty("is_archived")]
        public long IsArchived { get; set; }
    }

    public partial class Reminder
    {
        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("service", NullValueHandling = NullValueHandling.Ignore)]
        public string Service { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("due_date_utc", NullValueHandling = NullValueHandling.Ignore)]
        public string DueDateUtc { get; set; }

        [JsonProperty("minute_offset", NullValueHandling = NullValueHandling.Ignore)]
        public long? MinuteOffset { get; set; }

        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("notify_uid")]
        public long NotifyUid { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date_lang", NullValueHandling = NullValueHandling.Ignore)]
        public string DateLang { get; set; }

        [JsonProperty("date_string", NullValueHandling = NullValueHandling.Ignore)]
        public string DateString { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("sync_id")]
        public object SyncId { get; set; }

        [JsonProperty("loc_long", NullValueHandling = NullValueHandling.Ignore)]
        public string LocLong { get; set; }

        [JsonProperty("loc_lat", NullValueHandling = NullValueHandling.Ignore)]
        public string LocLat { get; set; }

        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public long? Radius { get; set; }

        [JsonProperty("project_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? ProjectId { get; set; }

        [JsonProperty("loc_trigger", NullValueHandling = NullValueHandling.Ignore)]
        public string LocTrigger { get; set; }
    }

    public partial class TempIdMapping
    {
    }

    public partial class User
    {
        [JsonProperty("start_page")]
        public string StartPage { get; set; }

        [JsonProperty("features")]
        public Features Features { get; set; }

        [JsonProperty("completed_today")]
        public long CompletedToday { get; set; }

        [JsonProperty("is_premium")]
        public bool IsPremium { get; set; }

        [JsonProperty("sort_order")]
        public long SortOrder { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("auto_reminder")]
        public long AutoReminder { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("share_limit")]
        public long ShareLimit { get; set; }

        [JsonProperty("days_off")]
        public List<long> DaysOff { get; set; }

        [JsonProperty("magic_num_reached")]
        public bool MagicNumReached { get; set; }

        [JsonProperty("next_week")]
        public long NextWeek { get; set; }

        [JsonProperty("completed_count")]
        public long CompletedCount { get; set; }

        [JsonProperty("daily_goal")]
        public long DailyGoal { get; set; }

        [JsonProperty("theme")]
        public long Theme { get; set; }

        [JsonProperty("tz_info")]
        public TzInfo TzInfo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("start_day")]
        public long StartDay { get; set; }

        [JsonProperty("weekly_goal")]
        public long WeeklyGoal { get; set; }

        [JsonProperty("date_format")]
        public long DateFormat { get; set; }

        [JsonProperty("websocket_url")]
        public string WebsocketUrl { get; set; }

        [JsonProperty("inbox_project")]
        public long InboxProject { get; set; }

        [JsonProperty("time_format")]
        public long TimeFormat { get; set; }

        [JsonProperty("image_id")]
        public object ImageId { get; set; }

        [JsonProperty("karma_trend")]
        public string KarmaTrend { get; set; }

        [JsonProperty("business_account_id")]
        public object BusinessAccountId { get; set; }

        [JsonProperty("mobile_number")]
        public object MobileNumber { get; set; }

        [JsonProperty("mobile_host")]
        public object MobileHost { get; set; }

        [JsonProperty("premium_until")]
        public string PremiumUntil { get; set; }

        [JsonProperty("karma_vacation")]
        public long KarmaVacation { get; set; }

        [JsonProperty("dateist_lang")]
        public object DateistLang { get; set; }

        [JsonProperty("join_date")]
        public string JoinDate { get; set; }

        [JsonProperty("karma")]
        public long Karma { get; set; }

        [JsonProperty("is_biz_admin")]
        public bool IsBizAdmin { get; set; }

        [JsonProperty("default_reminder")]
        public string DefaultReminder { get; set; }

        [JsonProperty("dateist_inline_disabled")]
        public bool DateistInlineDisabled { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public partial class Features
    {
        [JsonProperty("karma_disabled")]
        public bool KarmaDisabled { get; set; }

        [JsonProperty("restriction")]
        public long Restriction { get; set; }

        [JsonProperty("karma_vacation")]
        public long KarmaVacation { get; set; }

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
    }

    public class TodoistException : Exception
    {
        public TodoistError Error { private set; get; }

        public TodoistException(TodoistError error, RequestException requestException)
            : base(requestException.Message, requestException)
        {
            Error = error;
        }
    }

    public partial class SyncResult
    {
        [JsonProperty("sync_status")]
        public Dictionary<string, string> SyncStatus { get; set; }

        [JsonProperty("temp_id_mapping", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, long> TempIdMapping { get; set; }

        [JsonProperty("full_sync")]
        public bool FullSync { get; set; }

        [JsonProperty("sync_token")]
        public string SyncToken { get; set; }
    }
}
