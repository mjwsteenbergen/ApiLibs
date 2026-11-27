using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ApiLibs.JSON.NotionRest
{
    public class FileBlock : NotionBlock
    {
        public FileBlock()
        {
            Type = "file";
        }

        [JsonPropertyName("file")]
        public HostedNotionFile File { get; set; }
    }

    [JsonConverter(typeof(NotionFileConverter))]
    public class NotionFileWrapper : NotionUnionTypeWrapper<NotionFile>
    {
    }

    public class NotionFileConverter : NotionObjectWithTypeConverter<NotionFile, NotionFileWrapper>
    {
        public override NotionFile Read(string type, JsonElement jToken)
        {
            return type switch
            {
                "file" => new HostedNotionFile(),
                "file_upload" => new UploadedNotionFile(),
                "external" => new ExternalNotionFile(),
                _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + jToken.ToString())
            };
        }
    }

    public abstract class NotionFile : WithType
    {
        public string Type { get; set; }
    }

    public class UploadedNotionFile : NotionFile
    {
        public UploadedNotionFile()
        {
            Type = "file_upload";
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class HostedNotionFile : NotionFile
    {
        public HostedNotionFile()
        {
            Type = "file";
        }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("expiry_time")]
        public DateTime? ExpiryTime { get; set; }
    }

    public class ExternalNotionFile : NotionFile
    {
        public ExternalNotionFile()
        {
            Type = "external";
        }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }
    }
}