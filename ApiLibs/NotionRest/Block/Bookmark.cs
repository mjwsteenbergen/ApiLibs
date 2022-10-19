using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class Bookmark : NotionBlock
    {
        public Bookmark()
        {
        }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("caption")]
        public List<RichText> Caption { get; set; }
    }
}