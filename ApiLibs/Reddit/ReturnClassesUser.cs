using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ApiLibs.Reddit
{
    public class SavedObject
    {
        public string kind { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public object modhash { get; set; }
        public List<ContentWrapper> children { get; set; }
        public string after { get; set; }
        public object before { get; set; }
    }

    public class ContentWrapper
    {
        public string kind { get; set; }
        public Content data { get; set; }
        public bool IsComment => kind.Contains("t1");
        public bool IsAccount => kind.Contains("t2");
        public bool IsLink => kind.Contains("t3");
        public bool IsMessage => kind.Contains("t4");
        public bool IsSubreddit => kind.Contains("t5");
        public bool IsAward => kind.Contains("t6");
        public bool IsPromoCampaign => kind.Contains("t8");
    }

    public class Content
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
        public string link_id { get; set; }
        public string link_author { get; set; }
        public string replies { get; set; }
        public string parent_id { get; set; }
        public string subreddit_name_prefixed { get; set; }
        public int controversiality { get; set; }
        public string body { get; set; }
        public string link_title { get; set; }
        public string body_html { get; set; }
        public bool score_hidden { get; set; }
        public string link_permalink { get; set; }
        public string link_url { get; set; }
        public string subreddit_type { get; set; }

        public static T Convert<T>(Content message) where T : new()
        {
            T t = new T();
            PropertyInfo[] propertyInfos = typeof(Content).GetProperties();
            var toSetProperties = typeof(T).GetProperties();
            foreach (PropertyInfo toSet in toSetProperties)
            {
                foreach (PropertyInfo info in propertyInfos)
                {
                    if (info.GetMethod.Name.Replace("get_", "") == toSet.SetMethod?.Name.Replace("set_", ""))
                    {
                        toSet.SetValue(t, info.GetValue(message));
                        break;
                    }
                }
            }

            return t;
        }

        public Comment ToComment()
        {
            return Convert<Comment>(this);
        }

        public Post ToPost()
        {
            return Convert<Post>(this);
        }
    }

    public class Comment
    {
        public string subreddit_id { get; set; }
        public object edited { get; set; }
        public object banned_by { get; set; }
        public object removal_reason { get; set; }
        public string link_id { get; set; }
        public string link_author { get; set; }
        public object likes { get; set; }
        public string replies { get; set; }
        public object[] user_reports { get; set; }
        public bool saved { get; set; }
        public string id { get; set; }
        public int gilded { get; set; }
        public bool archived { get; set; }
        public int score { get; set; }
        public object report_reasons { get; set; }
        public string author { get; set; }
        public int num_comments { get; set; }
        public string parent_id { get; set; }
        public string subreddit_name_prefixed { get; set; }
        public object approved_by { get; set; }
        public bool over_18 { get; set; }
        public int controversiality { get; set; }
        public string body { get; set; }
        public string link_title { get; set; }
        public object author_flair_css_class { get; set; }
        public int downs { get; set; }
        public string body_html { get; set; }
        public bool quarantine { get; set; }
        public string subreddit { get; set; }
        public string name { get; set; }
        public bool score_hidden { get; set; }
        public object num_reports { get; set; }
        public string link_permalink { get; set; }
        public bool stickied { get; set; }
        public float created { get; set; }
        public object author_flair_text { get; set; }
        public string link_url { get; set; }
        public float created_utc { get; set; }
        public object distinguished { get; set; }
        public object[] mod_reports { get; set; }
        public string subreddit_type { get; set; }
        public int ups { get; set; }
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
