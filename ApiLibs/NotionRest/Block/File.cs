using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class FileBlock : NotionBlock
    {
        public FileBlock()
        {
            Type = "file";
        }

        [JsonProperty("file")]
        public File File { get; set;}
    }
}