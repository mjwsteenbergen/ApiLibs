using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.NotionRest
{
    public class BulletedListItem : NotionBlock
    {
        public BulletedListItem()
        {
            Type = "bulleted_list_item";
        }

        [JsonPropertyName("text")]
        public List<RichText> Text { get; set;}
    }
}