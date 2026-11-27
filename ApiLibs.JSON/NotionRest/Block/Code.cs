using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.NotionRest
{
    public class Code : NotionBlock
    {
        public Code()
        {
            Type = "code";
        }

        [JsonPropertyName("text")]
        public List<RichText> RichText { get; set; }

        [JsonPropertyName("caption")]
        public List<RichText> Caption { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set;}
    }
}