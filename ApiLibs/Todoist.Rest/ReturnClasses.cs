using Newtonsoft.Json;
using ApiLibs.General;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace ApiLibs.TodoistRest
{
    public partial class TodoistProject : ObjectSearcher<TodoistRestService>
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("comment_count")]
        public long? CommentCount { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("is_shared")]
        public bool? IsShared { get; set; }

        [JsonProperty("view_style")]
        public string ViewStyle { get; set; }

        [JsonProperty("is_favorite")]
        public bool? IsFavorite { get; set; }

        [JsonProperty("is_inbox_project")]
        public bool? IsInboxProject { get; set; }

        [JsonProperty("is_team_inbox")]
        public bool? IsTeamInbox { get; set; }

        public Task Delete() => Service.DeleteProject(this);
        public Task Update() => Service.UpdateProject(this);
    }

    public partial class TodoistSection : ObjectSearcher<TodoistRestService>
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("order")]
        public int? Order { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Task Delete() => Service.DeleteSection(this);
        public Task Update() => Service.UpdateSection(this);
    }

    public partial class TodoistTask : ObjectSearcher<TodoistRestService>
    {
        [JsonProperty("assignee_id")]
        public string AssigneeId { get; set; }

        [JsonProperty("comment_count")]
        public long? CommentCount { get; set; }

        [JsonProperty("is_completed")]
        public bool? IsCompleted { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("due")]
        public TodoistDueDate Due { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("labels")]
        public List<string> LabelIds { get; set; }

        [JsonProperty("order")]
        public int? Order { get; set; }

        [JsonProperty("priority")]
        public int? Priority { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("section_id")]
        public string SectionId { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        public Task Update() => this.Service.UpdateTasks(this);
        public Task Delete() => this.Service.DeleteTask(this);
        public Task Close() => this.Service.Close(this);
        public Task Reopen() => this.Service.Reopen(this);
    }

    public class TodoistRequestTask : TodoistTask
    {
        [JsonProperty("due_string")]
        public string DueString { get; set; }

        [JsonProperty("due_date")]
        public DateTimeOffset? DueDate { get; set; }

        [JsonProperty("due_datetime")]
        public DateTimeOffset? DueDateTime { get; set; }

        [JsonProperty("due_lang")]
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

        [JsonProperty("date")]
        [JsonConverter(typeof(DateConverter))]
        public DateTimeOffset? Date { get; set; }

        [JsonProperty("recurring")]
        public bool? Recurring { get; set; }

        [JsonProperty("datetime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTimeOffset? Datetime { get; set; }

        [JsonProperty("string")]
        public string String { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }

    internal class DateConverter : JsonConverter<DateTimeOffset>
    {
        public override bool CanRead => false;

        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("yyyy-MM-dd"));
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

    public partial class TodoistComment
    {
        [JsonProperty("attachment")]
        public TodoistAttachment Attachment { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("posted")]
        public DateTimeOffset Posted { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("task_id")]
        public string TaskId { get; set; }
    }

    public partial class TodoistAttachment
    {
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("file_type")]
        public string FileType { get; set; }

        [JsonProperty("file_url")]
        public Uri FileUrl { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }
    }

    public partial class TodoistLabel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("order")]
        public int? Order { get; set; }

        [JsonProperty("favorite")]
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