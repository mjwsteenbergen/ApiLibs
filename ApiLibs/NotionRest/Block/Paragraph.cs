using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class Paragraph : NotionBlock
    {
        public Paragraph()
        {
            Type = "paragraph";
        }

        [JsonProperty("rich_text")]
        public List<RichText> Text { get; set;}

        [JsonProperty("children")]
        public List<NotionBlock> Children { get; set;}
    }
}