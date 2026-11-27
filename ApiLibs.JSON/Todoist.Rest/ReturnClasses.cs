using System.Text.Json;
using System.Text.Json.Serialization;
using ApiLibs.JSON.General;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace ApiLibs.JSON.TodoistRest
{
    public partial class TodoistProject : ObjectSearcher<TodoistRestService>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("comment_count")]
        public long? CommentCount { get; set; }

        [JsonPropertyName("order")]
        public long Order { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("is_shared")]
        public bool? IsShared { get; set; }

        [JsonPropertyName("view_style")]
        public string ViewStyle { get; set; }

        [JsonPropertyName("is_favorite")]
        public bool? IsFavorite { get; set; }

        [JsonPropertyName("is_inbox_project")]
        public bool? IsInboxProject { get; set; }

        [JsonPropertyName("is_team_inbox")]
        public bool? IsTeamInbox { get; set; }

        public Task Delete() => Service.DeleteProject(this);
        public Task Update() => Service.UpdateProject(this);
    }

    public partial class TodoistSection : ObjectSearcher<TodoistRestService>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public Task Delete() => Service.DeleteSection(this);
        public Task Update() => Service.UpdateSection(this);
    }

    public partial class TodoistTask : ObjectSearcher<TodoistRestService>
    {
        [JsonPropertyName("assignee_id")]
        public string AssigneeId { get; set; }

        [JsonPropertyName("comment_count")]
        public long? CommentCount { get; set; }

        [JsonPropertyName("is_completed")]
        public bool? IsCompleted { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("due")]
        public TodoistDueDate Due { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("labels")]
        public List<string> LabelIds { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("priority")]
        public int? Priority { get; set; }

        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }

        [JsonPropertyName("section_id")]
        public string SectionId { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        public Task Update() => this.Service.UpdateTasks(this);
        public Task Delete() => this.Service.DeleteTask(this);
        public Task Close() => this.Service.Close(this);
        public Task Reopen() => this.Service.Reopen(this);
    }

    public class TodoistRequestTask : TodoistTask
    {
        [JsonPropertyName("due_string")]
        public string DueString { get; set; }

        [JsonPropertyName("due_date")]
        public DateTimeOffset? DueDate { get; set; }

        [JsonPropertyName("due_datetime")]
        public DateTimeOffset? DueDateTime { get; set; }

        [JsonPropertyName("due_lang")]
        public string DueLang { get; set; }

        public TodoistRequestTask(DateTime dateTime)
        {
            // DueDate = dateTime.Date;
            DueString = dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            DueLang = "en";


            if (!DueDate.Equals(dateTime))
            {
                // DueDateTime = dateTime;
                // DueDate = null;
                DueString = dateTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
        }

        public TodoistRequestTask() {}

        public TodoistRequestTask(string id) {
            Id = id;
        }

    }

    public partial class TodoistDueDate
    {

        [JsonPropertyName("date")]
        [JsonConverter(typeof(DateConverter))]
        public DateTimeOffset? Date { get; set; }

        [JsonPropertyName("recurring")]
        public bool? Recurring { get; set; }

        [JsonPropertyName("datetime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTimeOffset? Datetime { get; set; }

        [JsonPropertyName("string")]
        public string String { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
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
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
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

    public partial class TodoistComment
    {
        [JsonPropertyName("attachment")]
        public TodoistAttachment Attachment { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("posted")]
        public DateTimeOffset Posted { get; set; }

        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }

        [JsonPropertyName("task_id")]
        public string TaskId { get; set; }
    }

    public partial class TodoistAttachment
    {
        [JsonPropertyName("file_name")]
        public string FileName { get; set; }

        [JsonPropertyName("file_type")]
        public string FileType { get; set; }

        [JsonPropertyName("file_url")]
        public Uri FileUrl { get; set; }

        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; }
    }

    public partial class TodoistLabel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("favorite")]
        public bool? Favorite { get; set; }
    }

    public static class TodoistColor
    {
        public static string ToHex(int color) => color switch
        {
            30 => "#b8255f",
            31 => "#db4035",
            32 => "#ff9933",
            33 => "#fad000",
            34 => "#afb83b",
            35 => "#7ecc49",
            36 => "#299438",
            37 => "#6accbc",
            38 => "#158fad",
            39 => "#14aaf5",
            40 => "#96c3eb",
            41 => "#4073ff",
            42 => "#884dff",
            43 => "#af38eb",
            44 => "#eb96eb",
            45 => "#e05194",
            46 => "#ff8d85",
            47 => "#808080",
            48 => "#b8b8b8",
            49 => "#ccac93",
            _ => throw new KeyNotFoundException("The following color does not exist in todoist: " + color)
        };

        public static string ToHex(string color) => color switch
        {
            "berry_red" => "#b8256f",
            "red" => "#db4035",
            "orange" => "#ff9933",
            "yellow" => "#fad000",
            "olive_green" => "#afb83b",
            "lime_green" => "#7ecc49",
            "green" => "#299438",
            "mint_green" => "#6accbc",
            "teal" => "#158fad",
            "sky_blue" => "#14aaf5",
            "light_blue" => "#96c3eb",
            "blue" => "#4073ff",
            "grape" => "#884dff",
            "violet" => "#af38eb",
            "lavender" => "#eb96eb",
            "magenta" => "#e05194",
            "salmon" => "#ff8d85",
            "charcoal" => "#808080",
            "grey" => "#b8b8b8",
            "taupe" => "#ccac93",
            _ => throw new KeyNotFoundException("The following color does not exist in todoist: " + color)
        };
    }
}