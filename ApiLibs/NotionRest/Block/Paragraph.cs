using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class Paragraph : NotionBlock
    {
        public Paragraph()
        {
        }

        [JsonProperty("text")]
        public List<RichText> Text { get; set;}

        [JsonProperty("children")]
        public List<NotionBlock> Children { get; set;}
    }
}