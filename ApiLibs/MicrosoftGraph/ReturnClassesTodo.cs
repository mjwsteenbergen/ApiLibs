using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ApiLibs.MicrosoftGraph
{
    public partial class FolderResult
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("@odata.nextLink")]
        public Uri OdataNextLink { get; set; }

        [JsonProperty("value")]
        public List<TaskFolder> Value { get; set; }
    }

    public partial class TaskFolder
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changeKey")]
        public string ChangeKey { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isDefaultFolder")]
        public bool IsDefaultFolder { get; set; }

        [JsonProperty("parentGroupKey")]
        public Guid ParentGroupKey { get; set; }
    }

    public partial class TaskResult
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("@odata.nextLink")]
        public Uri OdataNextLink { get; set; }

        [JsonProperty("value")]
        public List<Todo> Value { get; set; }
    }

    public partial class Todo
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset? CreatedDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        [JsonProperty("changeKey")]
        public string ChangeKey { get; set; }

        [JsonProperty("categories")]
        public List<object> Categories { get; set; }

        [JsonProperty("assignedTo")]
        public string AssignedTo { get; set; }

        [JsonProperty("hasAttachments")]
        public bool? HasAttachments { get; set; }

        [JsonProperty("importance")]
        public string Importance { get; set; }

        [JsonProperty("isReminderOn")]
        public bool? IsReminderOn { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("parentFolderId")]
        public string ParentFolderId { get; set; }

        [JsonProperty("sensitivity")]
        public string Sensitivity { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("completedDateTime")]
        public DatetimeTimeZone CompletedDateTime { get; set; }

        [JsonProperty("dueDateTime")]
        public DatetimeTimeZone DueDateTime { get; set; }

        [JsonProperty("recurrence")]
        public object Recurrence { get; set; }

        [JsonProperty("reminderDateTime")]
        public DatetimeTimeZone ReminderDateTime { get; set; }

        [JsonProperty("startDateTime")]
        public DatetimeTimeZone StartDateTime { get; set; }

        [JsonProperty("body")]
        public TaskBody Body { get; set; }
    }

    public partial class TaskBody
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public partial class DatetimeTimeZone
    {
        [JsonProperty("dateTime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }
    }
}
