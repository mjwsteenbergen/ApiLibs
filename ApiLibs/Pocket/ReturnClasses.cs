using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Pocket
{

    public class Rootobject
    {
        public int status { get; set; }
        public int complete { get; set; }
        public List[] list { get; set; }
        public object error { get; set; }
        public Search_Meta search_meta { get; set; }
        public int since { get; set; }
    }

    public class Search_Meta
    {
        public string search_type { get; set; }
    }

    public class List
    {
        public string item_id { get; set; }
        public string resolved_id { get; set; }
        public string given_url { get; set; }
        public string given_title { get; set; }
        public string favorite { get; set; }
        public string status { get; set; }
        public string time_added { get; set; }
        public string time_updated { get; set; }
        public string time_read { get; set; }
        public string time_favorited { get; set; }
        public int sort_id { get; set; }
        public string resolved_title { get; set; }
        public string resolved_url { get; set; }
        public string excerpt { get; set; }
        public string is_article { get; set; }
        public string is_index { get; set; }
        public string has_video { get; set; }
        public string has_image { get; set; }
        public string word_count { get; set; }
        public bool isFavorite { get { return favorite == "1"; } }
    }



}
