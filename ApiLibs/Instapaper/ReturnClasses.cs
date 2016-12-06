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
        public int bookmark_id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

}
