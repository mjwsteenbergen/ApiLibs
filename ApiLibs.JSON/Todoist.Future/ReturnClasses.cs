using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.Todoist.Future
{

    /// <summary>
    /// Permission scopes
    /// <see cref="https://developer.todoist.com/api/v1/#permission-scopes"/>
    /// </summary>
    public enum TodoistScope
    {
        TaskAdd,
        DataRead,
        DataReadWrite,
        DataDelete,
        ProjectDelete,
        BackupsRead
    }

    public class TodoistFutureTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }

    public class TodoistFuturePaginatedResponse<T>
    {
        [JsonPropertyName("results")]
        public List<T> Results { get; set; }

        [JsonPropertyName("next_cursor")]
        public string NextCursor { get; set; }
    }

    public class TodoistProjectSummary
    {
        [JsonPropertyName("child_order")]
        public long ChildOrder { get; set; }

        [JsonPropertyName("collapsed")]
        public bool Collapsed { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_archived")]
        public bool IsArchived { get; set; }

        [JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("view_style")]
        public string ViewStyle { get; set; }
    }

    public class TodoistProject : TodoistProjectSummary
    {
        [JsonPropertyName("can_assign_tasks")]
        public bool CanAssignTasks { get; set; }

        [JsonPropertyName("creator_uid")]
        public string CreatorUid { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("is_favorite")]
        public bool IsFavorite { get; set; }

        [JsonPropertyName("is_frozen")]
        public bool IsFrozen { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("default_order")]
        public long DefaultOrder { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("access")]
        public ProjectAccess Access { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("inbox_project")]
        public bool InboxProject { get; set; }

        [JsonPropertyName("is_collapsed")]
        public bool IsCollapsed { get; set; }

        [JsonPropertyName("is_shared")]
        public bool IsShared { get; set; }
    }

    public partial class ProjectAccess
    {
        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }

        [JsonPropertyName("configuration")]
        public object Configuration { get; set; }
    }

    public class TodoistSection
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }

        [JsonPropertyName("added_at")]
        public string AddedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("archived_at")]
        public string ArchivedAt { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("section_order")]
        public long SectionOrder { get; set; }

        [JsonPropertyName("is_archived")]
        public bool IsArchived { get; set; }

        [JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("is_collapsed")]
        public bool IsCollapsed { get; set; }
    }

    public class TodoistTask
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }

        [JsonPropertyName("section_id")]
        public string SectionId { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("added_by_uid")]
        public string AddedByUid { get; set; }

        [JsonPropertyName("assigned_by_uid")]
        public string AssignedByUid { get; set; }

        [JsonPropertyName("responsible_uid")]
        public string ResponsibleUid { get; set; }

        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; }

        [JsonPropertyName("deadline")]
        public TodoistTaskDeadline Deadline { get; set; }

        [JsonPropertyName("duration")]
        public Dictionary<string, long> Duration { get; set; }

        [JsonPropertyName("checked")]
        public bool Checked { get; set; }

        [JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("added_at")]
        public string AddedAt { get; set; }

        [JsonPropertyName("completed_at")]
        public string CompletedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("due")]
        public TodoistDueDate Due { get; set; }

        [JsonPropertyName("priority")]
        public long Priority { get; set; }

        [JsonPropertyName("child_order")]
        public long ChildOrder { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("note_count")]
        public long NoteCount { get; set; }

        [JsonPropertyName("day_order")]
        public long DayOrder { get; set; }

        [JsonPropertyName("is_collapsed")]
        public bool IsCollapsed { get; set; }
    }

    public class TodoistTaskDeadline
    {
        [JsonPropertyName("date")]
        [JsonConverter(typeof(DateConverter))]
        public DateTimeOffset? Date { get; set; }

        [JsonPropertyName("lang")]
        public string Language { get; set; }
    }

    public class TodoistLabel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("order")]
        public long Order { get; set; }

        [JsonPropertyName("is_favorite")]
        public bool IsFavorite { get; set; }
    }

    public partial class TodoistDueDate
    {

        [JsonPropertyName("date")]
        [JsonConverter(typeof(DateConverter))]
        public DateTimeOffset? Date { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("recurring")]
        public bool? Recurring { get; set; }

        [JsonPropertyName("lang")]
        public string Language { get; set; }

        [JsonPropertyName("string")]
        public string String { get; set; }
    }

    /// <summary>
    /// The Newtonsoft version had CanRead => false: reading used the default DateTimeOffset
    /// parsing, and only writing is custom.
    /// </summary>
    internal class DateConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ((JsonConverter<DateTimeOffset?>)options.GetConverter(typeof(DateTimeOffset?))).Read(ref reader, typeof(DateTimeOffset?), options);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            var text = value?.ToString("yyyy-MM-dd");
            if (text == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(text);
            }
        }
    }

    /// <summary>
    /// The Newtonsoft version had CanRead => false: reading used the default DateTimeOffset
    /// parsing, and only writing is custom.
    /// </summary>
    internal class DateTimeConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ((JsonConverter<DateTimeOffset?>)options.GetConverter(typeof(DateTimeOffset?))).Read(ref reader, typeof(DateTimeOffset?), options);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }

    public class SyncResponse
    {
        [JsonPropertyName("sync_token")]
        public string SyncToken { get; set; }

        [JsonPropertyName("sync_status")]
        public Dictionary<string, string> SyncStatus { get; set; }

        [JsonPropertyName("temp_id_mapping")]
        public Dictionary<string, string> TempIdMapping { get; set; }
    }
}