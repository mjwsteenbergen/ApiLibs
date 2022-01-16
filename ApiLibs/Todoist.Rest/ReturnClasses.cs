using Newtonsoft.Json;
using ApiLibs.General;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace ApiLibs.TodoistRest
{
    public partial class TodoistProject : ObjectSearcher<TodoistRestService>
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("comment_count")]
        public long? CommentCount { get; set; }

        [JsonProperty("order")]
        public long? Order { get; set; }

        [JsonProperty("color")]
        public long? Color { get; set; }

        [JsonProperty("shared")]
        public bool? Shared { get; set; }

        [JsonProperty("sync_id")]
        public long? SyncId { get; set; }

        [JsonProperty("favorite")]
        public bool? Favorite { get; set; }

        [JsonProperty("inbox_project")]
        public bool? InboxProject { get; set; }

        public Task Delete() => Service.DeleteProject(this);
        public Task Update() => Service.UpdateProject(this);
    }

    public partial class TodoistSection : ObjectSearcher<TodoistRestService>
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("project_id")]
        public long? ProjectId { get; set; }

        [JsonProperty("order")]
        public int? Order { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Task Delete() => Service.DeleteSection(this);
        public Task Update() => Service.UpdateSection(this);
    }

    public partial class TodoistTask : ObjectSearcher<TodoistRestService>
    {
        [JsonProperty("assignee")]
        public long? Assignee { get; set; }

        [JsonProperty("comment_count")]
        public long? CommentCount { get; set; }

        [JsonProperty("completed")]
        public bool? Completed { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("due")]
        public TodoistDueDate Due { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("label_ids")]
        public List<long> LabelIds { get; set; }

        [JsonProperty("order")]
        public int? Order { get; set; }

        [JsonProperty("priority")]
        public int? Priority { get; set; }

        [JsonProperty("project_id")]
        public long? ProjectId { get; set; }

        [JsonProperty("section_id")]
        public long? SectionId { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

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

        public TodoistRequestTask(long id) {
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
        public long? Id { get; set; }

        [JsonProperty("posted")]
        public DateTimeOffset Posted { get; set; }

        [JsonProperty("project_id")]
        public long ProjectId { get; set; }

        [JsonProperty("task_id")]
        public long TaskId { get; set; }
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
        public long? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }

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
    }
}