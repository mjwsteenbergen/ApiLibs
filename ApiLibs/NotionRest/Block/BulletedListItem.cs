using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class BulletedListItem : NotionBlock
    {
        public BulletedListItem()
        {
            Type = "bulleted_list_item";
        }

        [JsonProperty("text")]
        public List<RichText> Text { get; set;}
    }
}