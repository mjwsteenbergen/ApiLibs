using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class Quote : NotionBlock
    {
        public Quote()
        {
        }

        [JsonProperty("text")]
        public List<RichText> Text { get; internal set; }
    }
}