using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace ApiLibs.Pocket
{


    public class Rootobject
    {
        public int status { get; set; }
        public int complete { get; set; }
        public PocketList[] PocketList { get; set; }
        public object error { get; set; }
        public Search_Meta search_meta { get; set; }
        public int since { get; set; }
    }

    public class PocketList
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
        public Dictionary<string,Tag> tags { get; set; }
        public Author[] authors { get; set; }
        public Image image { get; set; }
        public Image1[] images { get; set; }
        public Video1[] videos { get; set; }
        public bool isFavorite { get { return favorite == "1"; } }
        public ReadingStatus ReadingStatus { get {
            switch (status)
            {
                    case "0": return ReadingStatus.Unread;
                    case "1": return ReadingStatus.Read;
                    case "2": return ReadingStatus.Deleted;
                    default: throw new Exception("status should be 0, 1 or 2");
            }
        } }
    }

    public enum ReadingStatus
    {
        Unread, Read, Deleted
    }

    public class Tag
    {
        public string item_id { get; set; }
        public string tag { get; set; }
    }

    public class Image
    {
        public string item_id { get; set; }
        public string src { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }

    public class Author
    {
        public string item_id { get; set; }
        public string author_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Image1
    {
        public string item_id { get; set; }
        public string image_id { get; set; }
        public string src { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string credit { get; set; }
        public string caption { get; set; }
    }

    public class Video1
    {
        public string item_id { get; set; }
        public string video_id { get; set; }
        public string src { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string type { get; set; }
        public string vid { get; set; }
    }


    public class ReadingList
    {
        public int status { get; set; }
        public int complete { get; set; }
        public PocketList[] list { get; set; }
        public object error { get; set; }
        public Search_Meta search_meta { get; set; }
        public int since { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention", Justification = "ReturnClass")]
    public class Search_Meta
    {
        public string search_type { get; set; }
    }

    


    public class AddItemResponse
    {
        public Item item { get; set; }
        public int status { get; set; }
    }

    public class Item
    {
        public string item_id { get; set; }
        public string normal_url { get; set; }
        public string resolved_id { get; set; }
        public string extended_item_id { get; set; }
        public string resolved_url { get; set; }
        public string domain_id { get; set; }
        public string origin_domain_id { get; set; }
        public string response_code { get; set; }
        public string mime_type { get; set; }
        public string content_length { get; set; }
        public string encoding { get; set; }
        public string date_resolved { get; set; }
        public string date_published { get; set; }
        public string title { get; set; }
        public string excerpt { get; set; }
        public string word_count { get; set; }
        public string innerdomain_redirect { get; set; }
        public string login_required { get; set; }
        public string has_image { get; set; }
        public string has_video { get; set; }
        public string is_index { get; set; }
        public string is_article { get; set; }
        public string used_fallback { get; set; }
        public string lang { get; set; }
        public string resolved_normal_url { get; set; }
        public string given_url { get; set; }

        public override int GetHashCode()
        {
            return int.Parse(item_id);
        }
    }




}
