using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class Image : NotionBlock
    {
        public Image() 
        {
            Type = "image";
        }

        private File _imageFile;

        [JsonProperty("image")]
        public File ImageFile
        {
            get
            {
                if (_imageFile?.Url == null && _imageFile?.External == null)
                {
                    return File;
                }
                return _imageFile;
            }
            set
            {
                _imageFile = value;
            }
        }

        [JsonProperty("type")]
        public string ImageSourceType { get; set; }

        [JsonProperty("caption")]
        public List<RichText> Caption { get; set; }

        [JsonProperty("file")]
        public File File { get; set; }

        [JsonProperty("external")]
        public External External { get; set; }
    }

    public interface ImageSource {
        
    }
}