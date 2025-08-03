using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class NotionImage : NotionFileWrapper, INotionBlock
    {
        public NotionImage()
        {
            Type = "image";
        }

        public bool? HasChildren { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
        public Guid? Id { get; set; }

        [JsonProperty("caption")]
        public List<RichText> Caption { get; set; }
    }
}