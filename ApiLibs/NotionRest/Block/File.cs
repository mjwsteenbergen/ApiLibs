using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class FileBlock : NotionBlock
    {
        public FileBlock()
        {
        }

        [JsonProperty("file")]
        public File File { get; set;}
    }
}