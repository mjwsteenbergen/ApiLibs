using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.NotionRest
{
    public class Paragraph : NotionBlock
    {
        public Paragraph()
        {
            Type = "paragraph";
        }

        [JsonPropertyName("rich_text")]
        public List<RichText> Text { get; set;}

        [JsonPropertyName("children")]
        public List<NotionBlock> Children { get; set;}
    }
}