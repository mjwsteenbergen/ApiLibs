using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class BulletedListItem : NotionBlock
    {
        public BulletedListItem()
        {
        }

        [JsonProperty("text")]
        public List<RichText> Text { get; set;}
    }
}