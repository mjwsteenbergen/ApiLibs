using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.Ntfy
{
    /// <summary>A message for ntfy's JSON publish endpoint.</summary>
    public class NtfyMessage
    {
        [JsonPropertyName("topic")]
        public string Topic { get; set; } = "";

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>1 to 5. Unset is 3, the default.</summary>
        [JsonPropertyName("priority")]
        public int? Priority { get; set; }

        /// <summary>At most three: ntfy refuses a message with more.</summary>
        [JsonPropertyName("actions")]
        public List<NtfyAction> Actions { get; set; }

        public const int UrgentPriority = 5;
    }

    public class NtfyAction
    {
        /// <summary>"http" makes the request in the background; "view" opens the URL.</summary>
        [JsonPropertyName("action")]
        public string Action { get; set; } = "";

        [JsonPropertyName("label")]
        public string Label { get; set; } = "";

        [JsonPropertyName("url")]
        public string Url { get; set; } = "";

        /// <summary>http only. ntfy's default is POST.</summary>
        [JsonPropertyName("method")]
        public string Method { get; set; }

        /// <summary>http only. An object of header names to values.</summary>
        [JsonPropertyName("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        /// <summary>Dismisses the notification once the action has run. ntfy keeps it if the request fails.</summary>
        [JsonPropertyName("clear")]
        public bool Clear { get; set; }

        public const string Http = "http";
        public const string View = "view";
    }
}
