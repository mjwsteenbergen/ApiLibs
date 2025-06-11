using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiLibs.NotionRest
{
    public class FileBlock : NotionBlock
    {
        public FileBlock()
        {
            Type = "file";
        }

        [JsonProperty("file")]
        public HostedNotionFile File { get; set; }
    }

    [JsonConverter(typeof(NotionFileConverter))]
    public class NotionFileWrapper : NotionUnionTypeWrapper<NotionFile>
    {
    }

    public class NotionFileConverter : NotionObjectWithTypeConverter<NotionFile, NotionFileWrapper>
    {
        public override NotionFile Read(string type, JToken jToken)
        {
            return type switch
            {
                "file" => new HostedNotionFile(),
                "external" => new ExternalNotionFile(),
                _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + jToken.ToString())
            };
        }
    }

    public abstract class NotionFile : WithType
    {
        public string Type { get; set; }
    }

    public class HostedNotionFile : NotionFile
        {
            public HostedNotionFile()
            {
                Type = "file";
            }

            [JsonProperty("url")]
            public Uri Url { get; set; }

            [JsonProperty("expiry_time")]
            public DateTime? ExpiryTime { get; set; }
        }

    public class ExternalNotionFile : NotionFile
    {
        public ExternalNotionFile()
        {
            Type = "external";
        }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}