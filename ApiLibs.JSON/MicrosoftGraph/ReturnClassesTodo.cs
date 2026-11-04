using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.MicrosoftGraph
{
    public partial class FolderResult : ValueResult<TaskFolder> { }

    public partial class TaskFolder
    {
        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("isOwner")]
        public bool IsOwner { get; set; }

        [JsonPropertyName("isShared")]
        public bool IsShared { get; set; }

        [JsonPropertyName("wellknownListName")]
        public string WellknownListName { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public partial class TaskResult : ValueResult<Todo> { }

    public partial class Todo
    {
        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonPropertyName("importance")]
        public string Importance { get; set; }

        [JsonPropertyName("isReminderOn")]
        public bool IsReminderOn { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonPropertyName("completedDateTime")]
        public DatetimeTimeZone CompletedDateTime { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("body")]
        public Body Body { get; set; }

        [JsonPropertyName("dueDateTime")]
        public DatetimeTimeZone DueDateTime { get; set; }

        [JsonPropertyName("reminderDateTime")]
        public DatetimeTimeZone ReminderDateTime { get; set; }

        [JsonPropertyName("linkedResources")]
        public List<LinkedResource> LinkedResources { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Todo todo &&
                   Id == todo.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public partial class LinkedResource
    {
        [JsonPropertyName("webUrl")]
        public Uri WebUrl { get; set; }

        [JsonPropertyName("applicationName")]
        public string ApplicationName { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public partial class TaskBody
    {
        public TaskBody(string body)
        {
            Content = body;
            ContentType = "html";
        }

        public TaskBody() { }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public partial class DatetimeTimeZone
    {
        public DatetimeTimeZone()
        {
            TimeZone = "UTC";
        }

        [JsonPropertyName("dateTime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }
    }
}
