using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Instapaper
{
    public class Bookmark
    {
        public string type { get; set; }
        public string hash { get; set; }
        public string description { get; set; }
        public int bookmark_id { get; set; }
        public string private_source { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public int progress_timestamp { get; set; }
        public double time { get; set; }
        public float progress { get; set; }
        public string starred { get; set; }
    }


    public class Folder
    {
        public string title { get; set; }
        public string display_title { get; set; }
        public int sync_to_mobile { get; set; }
        public int folder_id { get; set; }
        public int position { get; set; }
        public string type { get; set; }
        public string slug { get; set; }
    }


}
