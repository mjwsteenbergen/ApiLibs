using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class Code : NotionBlock
    {
        public Code()
        {
            Type = "code";
        }

        [JsonProperty("text")]
        public List<RichText> RichText { get; set; }

        [JsonProperty("caption")]
        public List<RichText> Caption { get; set; }

        [JsonProperty("language")]
        public string Language { get; set;}
    }
}