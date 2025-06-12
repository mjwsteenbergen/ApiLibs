using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace ApiLibs.Todoist.Future
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
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

    public class TodoistFuturePaginatedResponse<T>
    {
        [JsonProperty("results")]
        public List<T> Results { get; set; }

        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }
    }

    public class TodoistProjectSummary
    {
        [JsonProperty("child_order")]
        public long ChildOrder { get; set; }

        [JsonProperty("collapsed")]
        public bool Collapsed { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("view_style")]
        public string ViewStyle { get; set; }
    }

    public class TodoistProject : TodoistProjectSummary
    {
        [JsonProperty("can_assign_tasks")]
        public bool CanAssignTasks { get; set; }

        [JsonProperty("creator_uid")]
        public string CreatorUid { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }

        [JsonProperty("is_frozen")]
        public bool IsFrozen { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("default_order")]
        public long DefaultOrder { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("access")]
        public ProjectAccess Access { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("inbox_project")]
        public bool InboxProject { get; set; }

        [JsonProperty("is_collapsed")]
        public bool IsCollapsed { get; set; }

        [JsonProperty("is_shared")]
        public bool IsShared { get; set; }
    }

    public partial class ProjectAccess
    {
        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("configuration")]
        public object Configuration { get; set; }
    }

    public class TodoistSection
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("added_at")]
        public string AddedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("archived_at")]
        public string ArchivedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("section_order")]
        public long SectionOrder { get; set; }

        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("is_collapsed")]
        public bool IsCollapsed { get; set; }
    }

    public class TodoistTask
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("section_id")]
        public string SectionId { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("added_by_uid")]
        public string AddedByUid { get; set; }

        [JsonProperty("assigned_by_uid")]
        public string AssignedByUid { get; set; }

        [JsonProperty("responsible_uid")]
        public string ResponsibleUid { get; set; }

        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        [JsonProperty("deadline")]
        public TodoistTaskDeadline Deadline { get; set; }

        [JsonProperty("duration")]
        public Dictionary<string, long> Duration { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("added_at")]
        public string AddedAt { get; set; }

        [JsonProperty("completed_at")]
        public string CompletedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("due")]
        public TodoistDueDate Due { get; set; }

        [JsonProperty("priority")]
        public long Priority { get; set; }

        [JsonProperty("child_order")]
        public long ChildOrder { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("note_count")]
        public long NoteCount { get; set; }

        [JsonProperty("day_order")]
        public long DayOrder { get; set; }

        [JsonProperty("is_collapsed")]
        public bool IsCollapsed { get; set; }
    }

    public class TodoistTaskDeadline
    {
        [JsonProperty("date")]
        [JsonConverter(typeof(DateConverter))]
        public DateTimeOffset? Date { get; set; }

        [JsonProperty("lang")]
        public string Language { get; set; }
    }

    public class TodoistLabel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }
    }

    public partial class TodoistDueDate
    {

        [JsonProperty("date")]
        [JsonConverter(typeof(DateConverter))]
        public DateTimeOffset? Date { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("recurring")]
        public bool? Recurring { get; set; }

        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("string")]
        public string String { get; set; }
    }

    internal class DateConverter : JsonConverter<DateTimeOffset?>
    {
        public override bool CanRead => false;

        public override DateTimeOffset? ReadJson(JsonReader reader, Type objectType, DateTimeOffset? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value.Contains(":"))
            {
                if (value.Contains("Z"))
                {
                    return DateTimeOffset.ParseExact(value, "yyyy-MM-ddTHH:mm:ss.FFFFFFZ", CultureInfo.InvariantCulture);

                }
                return DateTimeOffset.ParseExact(value, "yyyy-MM-ddTHH:mm:ss.FFFFFF", CultureInfo.InvariantCulture);
            }
            return DateTimeOffset.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, DateTimeOffset? value, JsonSerializer serializer)
        {
            writer.WriteValue(value?.ToString("yyyy-MM-dd"));
        }
    }

    internal class DateTimeConverter : JsonConverter<DateTimeOffset>
    {
        public override bool CanRead => false;

        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }

    public class SyncResponse
    {
        [JsonProperty("sync_token")]
        public string SyncToken { get; set; }

        [JsonProperty("sync_status")]
        public Dictionary<string, string> SyncStatus { get; set; }

        [JsonProperty("temp_id_mapping")]
        public Dictionary<string, string> TempIdMapping { get; set; }
    }
}