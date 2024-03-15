using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class Heading1 : NotionBlock
    {
        public Heading1()
        {
            Type = "heading_1";
        }

        [JsonProperty("rich_text")]
        public List<RichText> Text { get; set;}
    }
}