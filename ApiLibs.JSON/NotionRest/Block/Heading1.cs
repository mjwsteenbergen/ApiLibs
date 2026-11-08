using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.NotionRest
{
    public class Heading1 : NotionBlock
    {
        public Heading1()
        {
            Type = "heading_1";
        }

        [JsonPropertyName("rich_text")]
        public List<RichText> Text { get; set;}
    }
}