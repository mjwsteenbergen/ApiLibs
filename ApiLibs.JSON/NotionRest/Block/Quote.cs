using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.NotionRest
{
    public class Quote : NotionBlock
    {
        public Quote()
        {
            Type = "quote";
        }

        [JsonPropertyName("text")]
        public List<RichText> Text { get; set; }
    }
}