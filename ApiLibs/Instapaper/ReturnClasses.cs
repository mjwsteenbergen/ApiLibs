using System.Collections.Generic;
using System.Linq;

namespace ApiLibs.Instapaper
{
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


    public class BookmarksObject
    {
        public List<Highlight> highlights { get; set; }
        public List<Bookmark> bookmarks { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string username { get; set; }
        public int user_id { get; set; }
        public string type { get; set; }
        public string subscription_is_active { get; set; }
    }

    public class Highlight
    {
        public int highlight_id { get; set; }
        public string note { get; set; }
        public string text { get; set; }
        public int bookmark_id { get; set; }
        public string time { get; set; }
        public int position { get; set; }
        public string type { get; set; }
    }

    public class Bookmark
    {
        public string hash { get; set; }
        public string description { get; set; }
        public int bookmark_id { get; set; }
        public string private_source { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public int progress_timestamp { get; set; }
        public string time { get; set; }
        public float progress { get; set; }
        public string starred { get; set; }
        public string type { get; set; }

        public IEnumerable<Highlight> GetCorrespondingHighlights(IEnumerable<Highlight> highlights)
        {
            return highlights.Where(i => i.bookmark_id == this.bookmark_id);
        }
    }



}
