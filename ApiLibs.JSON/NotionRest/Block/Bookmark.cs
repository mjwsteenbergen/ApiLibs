using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.NotionRest
{
    public class Bookmark : NotionBlock
    {
        public Bookmark()
        {
            Type = "bookmark";
        }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("caption")]
        public List<RichText> Caption { get; set; }
    }
}