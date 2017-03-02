using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Reddit
{
    public class TokenObject
    {
        public string access_token;
        public string token_type;
        public string expires_in;
        public string scope;
        public string refresh_token;

    }


    public class SavedObject
    {
        public string kind { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public object modhash { get; set; }
        public List<PostWrapper> children { get; set; }
        public string after { get; set; }
        public object before { get; set; }
    }

    public class PostWrapper
    {
        public string kind { get; set; }
        public Post data { get; set; }
    }

    public class Post
    {
        public bool contest_mode { get; set; }
        public object banned_by { get; set; }
        public string domain { get; set; }
        public string subreddit { get; set; }
        public string selftext_html { get; set; }
        public string selftext { get; set; }
        public bool? likes { get; set; }
        public string suggested_sort { get; set; }
        public object[] user_reports { get; set; }
        public Secure_Media secure_media { get; set; }
        public bool saved { get; set; }
        public string id { get; set; }
        public int gilded { get; set; }
        public Secure_Media_Embed secure_media_embed { get; set; }
        public bool clicked { get; set; }
        public object report_reasons { get; set; }
        public string author { get; set; }
        public Media media { get; set; }
        public string name { get; set; }
        public int score { get; set; }
        public object approved_by { get; set; }
        public bool over_18 { get; set; }
        public object removal_reason { get; set; }
        public bool hidden { get; set; }
        public string thumbnail { get; set; }
        public string subreddit_id { get; set; }
        public object edited { get; set; }
        public string link_flair_css_class { get; set; }
        public string author_flair_css_class { get; set; }
        public int downs { get; set; }
        public object[] mod_reports { get; set; }
        public bool archived { get; set; }
        public Media_Embed media_embed { get; set; }
        public bool is_self { get; set; }
        public bool hide_score { get; set; }
        public bool spoiler { get; set; }
        public string permalink { get; set; }
        public bool locked { get; set; }
        public bool stickied { get; set; }
        public float created { get; set; }
        public string url { get; set; }
        public string author_flair_text { get; set; }
        public bool quarantine { get; set; }
        public string title { get; set; }
        public float created_utc { get; set; }
        public string link_flair_text { get; set; }
        public string distinguished { get; set; }
        public int num_comments { get; set; }
        public bool visited { get; set; }
        public object num_reports { get; set; }
        public int ups { get; set; }
        public Preview preview { get; set; }
        public string post_hint { get; set; }
    }

    public class Secure_Media
    {
        public string type { get; set; }
        public Oembed oembed { get; set; }
    }

    public class Oembed
    {
        public string provider_url { get; set; }
        public string version { get; set; }
        public string title { get; set; }
        public string author_name { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string html { get; set; }
        public int thumbnail_width { get; set; }
        public int thumbnail_height { get; set; }
        public string thumbnail_url { get; set; }
        public string type { get; set; }
        public string provider_name { get; set; }
        public string author_url { get; set; }
        public string description { get; set; }
    }

    public class Secure_Media_Embed
    {
        public string content { get; set; }
        public int width { get; set; }
        public bool scrolling { get; set; }
        public int height { get; set; }
    }

    public class Media
    {
        public string type { get; set; }
        public Oembed1 oembed { get; set; }
    }

    public class Oembed1
    {
        public string provider_url { get; set; }
        public string version { get; set; }
        public string title { get; set; }
        public string author_name { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string html { get; set; }
        public int thumbnail_width { get; set; }
        public int thumbnail_height { get; set; }
        public string thumbnail_url { get; set; }
        public string type { get; set; }
        public string provider_name { get; set; }
        public string author_url { get; set; }
        public string description { get; set; }
    }

    public class Media_Embed
    {
        public string content { get; set; }
        public int width { get; set; }
        public bool scrolling { get; set; }
        public int height { get; set; }
    }

    public class Preview
    {
        public Image[] images { get; set; }
    }

    public class Image
    {
        public Source source { get; set; }
        public Resolution2[] resolutions { get; set; }
        public Variants variants { get; set; }
        public string id { get; set; }
    }

    public class Source
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Variants
    {
        public Obfuscated obfuscated { get; set; }
        public Nsfw nsfw { get; set; }
    }

    public class Obfuscated
    {
        public Source1 source { get; set; }
        public Resolution[] resolutions { get; set; }
    }

    public class Source1
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Nsfw
    {
        public Source2 source { get; set; }
        public Resolution1[] resolutions { get; set; }
    }

    public class Source2
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution1
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Resolution2
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

}
