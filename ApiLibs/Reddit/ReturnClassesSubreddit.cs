using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ApiLibs.Reddit
{
    public partial class SubredditResult
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public SubredditResultData Data { get; set; }
    }

    public partial class SubredditResultData
    {
        [JsonProperty("modhash")]
        public object Modhash { get; set; }

        [JsonProperty("dist")]
        public long Dist { get; set; }

        [JsonProperty("children")]
        public List<Child> Children { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("before")]
        public object Before { get; set; }
    }

    public partial class Child
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public ChildData Data { get; set; }
    }

    public partial class ChildData
    {
        [JsonProperty("approved_at_utc")]
        public object ApprovedAtUtc { get; set; }

        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }

        [JsonProperty("selftext")]
        public string Selftext { get; set; }

        [JsonProperty("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonProperty("saved")]
        public bool Saved { get; set; }

        [JsonProperty("mod_reason_title")]
        public object ModReasonTitle { get; set; }

        [JsonProperty("gilded")]
        public long Gilded { get; set; }

        [JsonProperty("clicked")]
        public bool Clicked { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link_flair_richtext")]
        public List<object> LinkFlairRichtext { get; set; }

        [JsonProperty("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("pwls")]
        public object Pwls { get; set; }

        [JsonProperty("link_flair_css_class")]
        public string LinkFlairCssClass { get; set; }

        [JsonProperty("downs")]
        public long Downs { get; set; }

        [JsonProperty("thumbnail_height")]
        public long? ThumbnailHeight { get; set; }

        [JsonProperty("hide_score")]
        public bool HideScore { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quarantine")]
        public bool Quarantine { get; set; }

        [JsonProperty("link_flair_text_color")]
        public string LinkFlairTextColor { get; set; }

        [JsonProperty("author_flair_background_color")]
        public object AuthorFlairBackgroundColor { get; set; }

        [JsonProperty("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonProperty("ups")]
        public long Ups { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("media_embed")]
        public DataMediaEmbed MediaEmbed { get; set; }

        [JsonProperty("thumbnail_width")]
        public long? ThumbnailWidth { get; set; }

        [JsonProperty("author_flair_template_id")]
        public object AuthorFlairTemplateId { get; set; }

        [JsonProperty("is_original_content")]
        public bool IsOriginalContent { get; set; }

        [JsonProperty("user_reports")]
        public List<object> UserReports { get; set; }

        [JsonProperty("secure_media")]
        public DataMedia SecureMedia { get; set; }

        [JsonProperty("is_reddit_media_domain")]
        public bool IsRedditMediaDomain { get; set; }

        [JsonProperty("is_meta")]
        public bool IsMeta { get; set; }

        [JsonProperty("category")]
        public object Category { get; set; }

        [JsonProperty("secure_media_embed")]
        public DataMediaEmbed SecureMediaEmbed { get; set; }

        [JsonProperty("link_flair_text")]
        public string LinkFlairText { get; set; }

        [JsonProperty("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("approved_by")]
        public object ApprovedBy { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("edited")]
        public Edited Edited { get; set; }

        [JsonProperty("author_flair_css_class")]
        public object AuthorFlairCssClass { get; set; }

        [JsonProperty("author_flair_richtext")]
        public List<object> AuthorFlairRichtext { get; set; }

        [JsonProperty("gildings")]
        public Gildings Gildings { get; set; }

        [JsonProperty("content_categories")]
        public object ContentCategories { get; set; }

        [JsonProperty("is_self")]
        public bool IsSelf { get; set; }

        [JsonProperty("mod_note")]
        public object ModNote { get; set; }

        [JsonProperty("created")]
        public double Created { get; set; }

        [JsonProperty("link_flair_type")]
        public string LinkFlairType { get; set; }

        [JsonProperty("wls")]
        public object Wls { get; set; }

        [JsonProperty("banned_by")]
        public object BannedBy { get; set; }

        [JsonProperty("author_flair_type")]
        public string AuthorFlairType { get; set; }

        [JsonProperty("contest_mode")]
        public bool ContestMode { get; set; }

        [JsonProperty("selftext_html")]
        public string SelftextHtml { get; set; }

        [JsonProperty("likes")]
        public object Likes { get; set; }

        [JsonProperty("suggested_sort")]
        public object SuggestedSort { get; set; }

        [JsonProperty("banned_at_utc")]
        public object BannedAtUtc { get; set; }

        [JsonProperty("view_count")]
        public object ViewCount { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("no_follow")]
        public bool NoFollow { get; set; }

        [JsonProperty("is_crosspostable")]
        public bool IsCrosspostable { get; set; }

        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("over_18")]
        public bool Over18 { get; set; }

        [JsonProperty("media_only")]
        public bool MediaOnly { get; set; }

        [JsonProperty("link_flair_template_id")]
        public Guid? LinkFlairTemplateId { get; set; }

        [JsonProperty("can_gild")]
        public bool CanGild { get; set; }

        [JsonProperty("spoiler")]
        public bool Spoiler { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("author_flair_text")]
        public object AuthorFlairText { get; set; }

        [JsonProperty("visited")]
        public bool Visited { get; set; }

        [JsonProperty("num_reports")]
        public object NumReports { get; set; }

        [JsonProperty("distinguished")]
        public object Distinguished { get; set; }

        [JsonProperty("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonProperty("mod_reason_by")]
        public object ModReasonBy { get; set; }

        [JsonProperty("removal_reason")]
        public object RemovalReason { get; set; }

        [JsonProperty("link_flair_background_color")]
        public string LinkFlairBackgroundColor { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_robot_indexable")]
        public bool IsRobotIndexable { get; set; }

        [JsonProperty("report_reasons")]
        public object ReportReasons { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("num_crossposts")]
        public long NumCrossposts { get; set; }

        [JsonProperty("num_comments")]
        public long NumComments { get; set; }

        [JsonProperty("send_replies")]
        public bool SendReplies { get; set; }

        [JsonProperty("whitelist_status")]
        public object WhitelistStatus { get; set; }

        [JsonProperty("mod_reports")]
        public List<object> ModReports { get; set; }

        [JsonProperty("author_flair_text_color")]
        public object AuthorFlairTextColor { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("parent_whitelist_status")]
        public object ParentWhitelistStatus { get; set; }

        [JsonProperty("stickied")]
        public bool Stickied { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("subreddit_subscribers")]
        public long SubredditSubscribers { get; set; }

        [JsonProperty("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonProperty("media")]
        public DataMedia Media { get; set; }

        [JsonProperty("is_video")]
        public bool IsVideo { get; set; }

        [JsonProperty("post_hint", NullValueHandling = NullValueHandling.Ignore)]
        public string PostHint { get; set; }

        [JsonProperty("preview", NullValueHandling = NullValueHandling.Ignore)]
        public Preview Preview { get; set; }

        [JsonProperty("crosspost_parent_list", NullValueHandling = NullValueHandling.Ignore)]
        public List<CrosspostParentList> CrosspostParentList { get; set; }

        [JsonProperty("crosspost_parent", NullValueHandling = NullValueHandling.Ignore)]
        public string CrosspostParent { get; set; }
    }

    public partial class CrosspostParentList
    {
        [JsonProperty("approved_at_utc")]
        public object ApprovedAtUtc { get; set; }

        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }

        [JsonProperty("selftext")]
        public string Selftext { get; set; }

        [JsonProperty("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonProperty("saved")]
        public bool Saved { get; set; }

        [JsonProperty("mod_reason_title")]
        public object ModReasonTitle { get; set; }

        [JsonProperty("gilded")]
        public long Gilded { get; set; }

        [JsonProperty("clicked")]
        public bool Clicked { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link_flair_richtext")]
        public List<LinkFlairRichtext> LinkFlairRichtext { get; set; }

        [JsonProperty("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("pwls")]
        public long? Pwls { get; set; }

        [JsonProperty("link_flair_css_class")]
        public string LinkFlairCssClass { get; set; }

        [JsonProperty("downs")]
        public long Downs { get; set; }

        [JsonProperty("thumbnail_height")]
        public long? ThumbnailHeight { get; set; }

        [JsonProperty("hide_score")]
        public bool HideScore { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quarantine")]
        public bool Quarantine { get; set; }

        [JsonProperty("link_flair_text_color")]
        public string LinkFlairTextColor { get; set; }

        [JsonProperty("author_flair_background_color")]
        public object AuthorFlairBackgroundColor { get; set; }

        [JsonProperty("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonProperty("ups")]
        public long Ups { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("media_embed")]
        public CrosspostParentListMediaEmbed MediaEmbed { get; set; }

        [JsonProperty("thumbnail_width")]
        public long? ThumbnailWidth { get; set; }

        [JsonProperty("author_flair_template_id")]
        public object AuthorFlairTemplateId { get; set; }

        [JsonProperty("is_original_content")]
        public bool IsOriginalContent { get; set; }

        [JsonProperty("user_reports")]
        public List<object> UserReports { get; set; }

        [JsonProperty("secure_media")]
        public CrosspostParentListMedia SecureMedia { get; set; }

        [JsonProperty("is_reddit_media_domain")]
        public bool IsRedditMediaDomain { get; set; }

        [JsonProperty("is_meta")]
        public bool IsMeta { get; set; }

        [JsonProperty("category")]
        public object Category { get; set; }

        [JsonProperty("secure_media_embed")]
        public CrosspostParentListMediaEmbed SecureMediaEmbed { get; set; }

        [JsonProperty("link_flair_text")]
        public string LinkFlairText { get; set; }

        [JsonProperty("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("approved_by")]
        public object ApprovedBy { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("edited")]
        public bool Edited { get; set; }

        [JsonProperty("author_flair_css_class")]
        public object AuthorFlairCssClass { get; set; }

        [JsonProperty("author_flair_richtext")]
        public List<object> AuthorFlairRichtext { get; set; }

        [JsonProperty("gildings")]
        public Gildings Gildings { get; set; }

        [JsonProperty("post_hint", NullValueHandling = NullValueHandling.Ignore)]
        public string PostHint { get; set; }

        [JsonProperty("content_categories")]
        public object ContentCategories { get; set; }

        [JsonProperty("is_self")]
        public bool IsSelf { get; set; }

        [JsonProperty("mod_note")]
        public object ModNote { get; set; }

        [JsonProperty("created")]
        public double Created { get; set; }

        [JsonProperty("link_flair_type")]
        public string LinkFlairType { get; set; }

        [JsonProperty("wls")]
        public long? Wls { get; set; }

        [JsonProperty("banned_by")]
        public object BannedBy { get; set; }

        [JsonProperty("author_flair_type")]
        public string AuthorFlairType { get; set; }

        [JsonProperty("contest_mode")]
        public bool ContestMode { get; set; }

        [JsonProperty("selftext_html")]
        public string SelftextHtml { get; set; }

        [JsonProperty("likes")]
        public object Likes { get; set; }

        [JsonProperty("suggested_sort")]
        public object SuggestedSort { get; set; }

        [JsonProperty("banned_at_utc")]
        public object BannedAtUtc { get; set; }

        [JsonProperty("view_count")]
        public object ViewCount { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("no_follow")]
        public bool NoFollow { get; set; }

        [JsonProperty("is_crosspostable")]
        public bool IsCrosspostable { get; set; }

        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("over_18")]
        public bool Over18 { get; set; }

        [JsonProperty("preview", NullValueHandling = NullValueHandling.Ignore)]
        public Preview Preview { get; set; }

        [JsonProperty("media_only")]
        public bool MediaOnly { get; set; }

        [JsonProperty("link_flair_template_id")]
        public Guid? LinkFlairTemplateId { get; set; }

        [JsonProperty("can_gild")]
        public bool CanGild { get; set; }

        [JsonProperty("spoiler")]
        public bool Spoiler { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("author_flair_text")]
        public object AuthorFlairText { get; set; }

        [JsonProperty("visited")]
        public bool Visited { get; set; }

        [JsonProperty("num_reports")]
        public object NumReports { get; set; }

        [JsonProperty("distinguished")]
        public object Distinguished { get; set; }

        [JsonProperty("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonProperty("mod_reason_by")]
        public object ModReasonBy { get; set; }

        [JsonProperty("removal_reason")]
        public object RemovalReason { get; set; }

        [JsonProperty("link_flair_background_color")]
        public string LinkFlairBackgroundColor { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_robot_indexable")]
        public bool IsRobotIndexable { get; set; }

        [JsonProperty("report_reasons")]
        public object ReportReasons { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("num_crossposts")]
        public long NumCrossposts { get; set; }

        [JsonProperty("num_comments")]
        public long NumComments { get; set; }

        [JsonProperty("send_replies")]
        public bool SendReplies { get; set; }

        [JsonProperty("whitelist_status")]
        public string WhitelistStatus { get; set; }

        [JsonProperty("mod_reports")]
        public List<object> ModReports { get; set; }

        [JsonProperty("author_flair_text_color")]
        public object AuthorFlairTextColor { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("parent_whitelist_status")]
        public string ParentWhitelistStatus { get; set; }

        [JsonProperty("stickied")]
        public bool Stickied { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("subreddit_subscribers")]
        public long SubredditSubscribers { get; set; }

        [JsonProperty("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonProperty("media")]
        public CrosspostParentListMedia Media { get; set; }

        [JsonProperty("is_video")]
        public bool IsVideo { get; set; }
    }

    public partial class Gildings
    {
        [JsonProperty("gid_1")]
        public long Gid1 { get; set; }

        [JsonProperty("gid_2")]
        public long Gid2 { get; set; }

        [JsonProperty("gid_3")]
        public long Gid3 { get; set; }
    }

    public partial class LinkFlairRichtext
    {
        [JsonProperty("e")]
        public string E { get; set; }

        [JsonProperty("t")]
        public string T { get; set; }
    }

    public partial class CrosspostParentListMedia
    {
        [JsonProperty("reddit_video")]
        public RedditVideo RedditVideo { get; set; }
    }

    public partial class RedditVideo
    {
        [JsonProperty("fallback_url")]
        public Uri FallbackUrl { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("scrubber_media_url")]
        public Uri ScrubberMediaUrl { get; set; }

        [JsonProperty("dash_url")]
        public Uri DashUrl { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("hls_url")]
        public Uri HlsUrl { get; set; }

        [JsonProperty("is_gif")]
        public bool IsGif { get; set; }

        [JsonProperty("transcoding_status")]
        public string TranscodingStatus { get; set; }
    }

    public partial class CrosspostParentListMediaEmbed
    {
    }

    public partial class Preview
    {
        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("reddit_video_preview", NullValueHandling = NullValueHandling.Ignore)]
        public RedditVideo RedditVideoPreview { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("resolutions")]
        public List<Source> Resolutions { get; set; }

        [JsonProperty("variants")]
        public Variants Variants { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Source
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }

    public partial class Variants
    {
        [JsonProperty("gif", NullValueHandling = NullValueHandling.Ignore)]
        public Gif Gif { get; set; }

        [JsonProperty("mp4", NullValueHandling = NullValueHandling.Ignore)]
        public Gif Mp4 { get; set; }
    }

    public partial class Gif
    {
        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("resolutions")]
        public List<Source> Resolutions { get; set; }
    }

    public partial class DataMedia
    {
        [JsonProperty("oembed")]
        public Oembed Oembed { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Oembed
    {
        [JsonProperty("provider_url")]
        public Uri ProviderUrl { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("thumbnail_width")]
        public long ThumbnailWidth { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("author_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorName { get; set; }

        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty("thumbnail_url")]
        public Uri ThumbnailUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("thumbnail_height")]
        public long ThumbnailHeight { get; set; }

        [JsonProperty("author_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri AuthorUrl { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    public partial class DataMediaEmbed
    {
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long? Width { get; set; }

        [JsonProperty("scrolling", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Scrolling { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long? Height { get; set; }

        [JsonProperty("media_domain_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri MediaDomainUrl { get; set; }
    }

    public partial struct Edited
    {
        public bool? Bool;
        public double? Double;

        public static implicit operator Edited(bool Bool) => new Edited { Bool = Bool };
        public static implicit operator Edited(double Double) => new Edited { Double = Double };
    }
}