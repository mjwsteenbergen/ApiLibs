using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.Reddit
{
    public partial class SubredditResult
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("data")]
        public SubredditResultData Data { get; set; }
    }

    public partial class SubredditResultData
    {
        [JsonPropertyName("modhash")]
        public object Modhash { get; set; }

        [JsonPropertyName("dist")]
        public long Dist { get; set; }

        [JsonPropertyName("children")]
        public List<Child> Children { get; set; }

        [JsonPropertyName("after")]
        public string After { get; set; }

        [JsonPropertyName("before")]
        public object Before { get; set; }
    }

    public partial class Child
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("data")]
        public RedditPost Data { get; set; }
    }

    public partial class RedditPost
    {
        [JsonPropertyName("approved_at_utc")]
        public object ApprovedAtUtc { get; set; }

        [JsonPropertyName("subreddit")]
        public string Subreddit { get; set; }

        [JsonPropertyName("selftext")]
        public string Selftext { get; set; }

        [JsonPropertyName("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonPropertyName("saved")]
        public bool Saved { get; set; }

        [JsonPropertyName("mod_reason_title")]
        public object ModReasonTitle { get; set; }

        [JsonPropertyName("gilded")]
        public long Gilded { get; set; }

        [JsonPropertyName("clicked")]
        public bool Clicked { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("link_flair_richtext")]
        public List<object> LinkFlairRichtext { get; set; }

        [JsonPropertyName("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; }

        [JsonPropertyName("pwls")]
        public object Pwls { get; set; }

        [JsonPropertyName("link_flair_css_class")]
        public string LinkFlairCssClass { get; set; }

        [JsonPropertyName("downs")]
        public long Downs { get; set; }

        [JsonPropertyName("thumbnail_height")]
        public long? ThumbnailHeight { get; set; }

        [JsonPropertyName("hide_score")]
        public bool HideScore { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("quarantine")]
        public bool Quarantine { get; set; }

        [JsonPropertyName("link_flair_text_color")]
        public string LinkFlairTextColor { get; set; }

        [JsonPropertyName("author_flair_background_color")]
        public object AuthorFlairBackgroundColor { get; set; }

        [JsonPropertyName("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonPropertyName("ups")]
        public long Ups { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("media_embed")]
        public DataMediaEmbed MediaEmbed { get; set; }

        [JsonPropertyName("thumbnail_width")]
        public long? ThumbnailWidth { get; set; }

        [JsonPropertyName("author_flair_template_id")]
        public object AuthorFlairTemplateId { get; set; }

        [JsonPropertyName("is_original_content")]
        public bool IsOriginalContent { get; set; }

        [JsonPropertyName("user_reports")]
        public List<object> UserReports { get; set; }

        [JsonPropertyName("secure_media")]
        public DataMedia SecureMedia { get; set; }

        [JsonPropertyName("is_reddit_media_domain")]
        public bool IsRedditMediaDomain { get; set; }

        [JsonPropertyName("is_meta")]
        public bool IsMeta { get; set; }

        [JsonPropertyName("category")]
        public object Category { get; set; }

        [JsonPropertyName("secure_media_embed")]
        public DataMediaEmbed SecureMediaEmbed { get; set; }

        [JsonPropertyName("link_flair_text")]
        public string LinkFlairText { get; set; }

        [JsonPropertyName("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonPropertyName("score")]
        public long Score { get; set; }

        [JsonPropertyName("approved_by")]
        public object ApprovedBy { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("edited")]
        public Edited Edited { get; set; }

        [JsonPropertyName("author_flair_css_class")]
        public object AuthorFlairCssClass { get; set; }

        [JsonPropertyName("author_flair_richtext")]
        public List<object> AuthorFlairRichtext { get; set; }

        [JsonPropertyName("gildings")]
        public Gildings Gildings { get; set; }

        [JsonPropertyName("content_categories")]
        public object ContentCategories { get; set; }

        [JsonPropertyName("is_self")]
        public bool IsSelf { get; set; }

        [JsonPropertyName("mod_note")]
        public object ModNote { get; set; }

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("link_flair_type")]
        public string LinkFlairType { get; set; }

        [JsonPropertyName("wls")]
        public object Wls { get; set; }

        [JsonPropertyName("banned_by")]
        public object BannedBy { get; set; }

        [JsonPropertyName("author_flair_type")]
        public string AuthorFlairType { get; set; }

        [JsonPropertyName("contest_mode")]
        public bool ContestMode { get; set; }

        [JsonPropertyName("selftext_html")]
        public string SelftextHtml { get; set; }

        [JsonPropertyName("likes")]
        public object Likes { get; set; }

        [JsonPropertyName("suggested_sort")]
        public object SuggestedSort { get; set; }

        [JsonPropertyName("banned_at_utc")]
        public object BannedAtUtc { get; set; }

        [JsonPropertyName("view_count")]
        public object ViewCount { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        [JsonPropertyName("no_follow")]
        public bool NoFollow { get; set; }

        [JsonPropertyName("is_crosspostable")]
        public bool IsCrosspostable { get; set; }

        [JsonPropertyName("pinned")]
        public bool Pinned { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("media_only")]
        public bool MediaOnly { get; set; }

        [JsonPropertyName("link_flair_template_id")]
        public Guid? LinkFlairTemplateId { get; set; }

        [JsonPropertyName("can_gild")]
        public bool CanGild { get; set; }

        [JsonPropertyName("spoiler")]
        public bool Spoiler { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        [JsonPropertyName("author_flair_text")]
        public object AuthorFlairText { get; set; }

        [JsonPropertyName("visited")]
        public bool Visited { get; set; }

        [JsonPropertyName("num_reports")]
        public object NumReports { get; set; }

        [JsonPropertyName("distinguished")]
        public object Distinguished { get; set; }

        [JsonPropertyName("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonPropertyName("mod_reason_by")]
        public object ModReasonBy { get; set; }

        [JsonPropertyName("removal_reason")]
        public object RemovalReason { get; set; }

        [JsonPropertyName("link_flair_background_color")]
        public string LinkFlairBackgroundColor { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_robot_indexable")]
        public bool IsRobotIndexable { get; set; }

        [JsonPropertyName("report_reasons")]
        public object ReportReasons { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("num_crossposts")]
        public long NumCrossposts { get; set; }

        [JsonPropertyName("num_comments")]
        public long NumComments { get; set; }

        [JsonPropertyName("send_replies")]
        public bool SendReplies { get; set; }

        [JsonPropertyName("whitelist_status")]
        public object WhitelistStatus { get; set; }

        [JsonPropertyName("mod_reports")]
        public List<object> ModReports { get; set; }

        [JsonPropertyName("author_flair_text_color")]
        public object AuthorFlairTextColor { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("parent_whitelist_status")]
        public object ParentWhitelistStatus { get; set; }

        [JsonPropertyName("stickied")]
        public bool Stickied { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("subreddit_subscribers")]
        public long SubredditSubscribers { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }

        public DateTime CreatedDt => DateTimeOffset.FromUnixTimeSeconds((long)CreatedUtc).DateTime;

        [JsonPropertyName("media")]
        public DataMedia Media { get; set; }

        [JsonPropertyName("is_video")]
        public bool IsVideo { get; set; }

        [JsonPropertyName("post_hint"), NullValueHandling(NullHandling.Ignore)]
        public string PostHint { get; set; }

        [JsonPropertyName("preview"), NullValueHandling(NullHandling.Ignore)]
        public Preview Preview { get; set; }

        [JsonPropertyName("crosspost_parent_list"), NullValueHandling(NullHandling.Ignore)]
        public List<CrosspostParentList> CrosspostParentList { get; set; }

        [JsonPropertyName("crosspost_parent"), NullValueHandling(NullHandling.Ignore)]
        public string CrosspostParent { get; set; }
    }

    public partial class CrosspostParentList
    {
        [JsonPropertyName("approved_at_utc")]
        public object ApprovedAtUtc { get; set; }

        [JsonPropertyName("subreddit")]
        public string Subreddit { get; set; }

        [JsonPropertyName("selftext")]
        public string Selftext { get; set; }

        [JsonPropertyName("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonPropertyName("saved")]
        public bool Saved { get; set; }

        [JsonPropertyName("mod_reason_title")]
        public object ModReasonTitle { get; set; }

        [JsonPropertyName("gilded")]
        public long Gilded { get; set; }

        [JsonPropertyName("clicked")]
        public bool Clicked { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("link_flair_richtext")]
        public List<LinkFlairRichtext> LinkFlairRichtext { get; set; }

        [JsonPropertyName("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; }

        [JsonPropertyName("pwls")]
        public long? Pwls { get; set; }

        [JsonPropertyName("link_flair_css_class")]
        public string LinkFlairCssClass { get; set; }

        [JsonPropertyName("downs")]
        public long Downs { get; set; }

        [JsonPropertyName("thumbnail_height")]
        public long? ThumbnailHeight { get; set; }

        [JsonPropertyName("hide_score")]
        public bool HideScore { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("quarantine")]
        public bool Quarantine { get; set; }

        [JsonPropertyName("link_flair_text_color")]
        public string LinkFlairTextColor { get; set; }

        [JsonPropertyName("author_flair_background_color")]
        public object AuthorFlairBackgroundColor { get; set; }

        [JsonPropertyName("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonPropertyName("ups")]
        public long Ups { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("media_embed")]
        public CrosspostParentListMediaEmbed MediaEmbed { get; set; }

        [JsonPropertyName("thumbnail_width")]
        public long? ThumbnailWidth { get; set; }

        [JsonPropertyName("author_flair_template_id")]
        public object AuthorFlairTemplateId { get; set; }

        [JsonPropertyName("is_original_content")]
        public bool IsOriginalContent { get; set; }

        [JsonPropertyName("user_reports")]
        public List<object> UserReports { get; set; }

        [JsonPropertyName("secure_media")]
        public CrosspostParentListMedia SecureMedia { get; set; }

        [JsonPropertyName("is_reddit_media_domain")]
        public bool IsRedditMediaDomain { get; set; }

        [JsonPropertyName("is_meta")]
        public bool IsMeta { get; set; }

        [JsonPropertyName("category")]
        public object Category { get; set; }

        [JsonPropertyName("secure_media_embed")]
        public CrosspostParentListMediaEmbed SecureMediaEmbed { get; set; }

        [JsonPropertyName("link_flair_text")]
        public string LinkFlairText { get; set; }

        [JsonPropertyName("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonPropertyName("score")]
        public long Score { get; set; }

        [JsonPropertyName("approved_by")]
        public object ApprovedBy { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("edited")]
        public bool Edited { get; set; }

        [JsonPropertyName("author_flair_css_class")]
        public object AuthorFlairCssClass { get; set; }

        [JsonPropertyName("author_flair_richtext")]
        public List<object> AuthorFlairRichtext { get; set; }

        [JsonPropertyName("gildings")]
        public Gildings Gildings { get; set; }

        [JsonPropertyName("post_hint"), NullValueHandling(NullHandling.Ignore)]
        public string PostHint { get; set; }

        [JsonPropertyName("content_categories")]
        public object ContentCategories { get; set; }

        [JsonPropertyName("is_self")]
        public bool IsSelf { get; set; }

        [JsonPropertyName("mod_note")]
        public object ModNote { get; set; }

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("link_flair_type")]
        public string LinkFlairType { get; set; }

        [JsonPropertyName("wls")]
        public long? Wls { get; set; }

        [JsonPropertyName("banned_by")]
        public object BannedBy { get; set; }

        [JsonPropertyName("author_flair_type")]
        public string AuthorFlairType { get; set; }

        [JsonPropertyName("contest_mode")]
        public bool ContestMode { get; set; }

        [JsonPropertyName("selftext_html")]
        public string SelftextHtml { get; set; }

        [JsonPropertyName("likes")]
        public object Likes { get; set; }

        [JsonPropertyName("suggested_sort")]
        public object SuggestedSort { get; set; }

        [JsonPropertyName("banned_at_utc")]
        public object BannedAtUtc { get; set; }

        [JsonPropertyName("view_count")]
        public object ViewCount { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        [JsonPropertyName("no_follow")]
        public bool NoFollow { get; set; }

        [JsonPropertyName("is_crosspostable")]
        public bool IsCrosspostable { get; set; }

        [JsonPropertyName("pinned")]
        public bool Pinned { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("preview"), NullValueHandling(NullHandling.Ignore)]
        public Preview Preview { get; set; }

        [JsonPropertyName("media_only")]
        public bool MediaOnly { get; set; }

        [JsonPropertyName("link_flair_template_id")]
        public Guid? LinkFlairTemplateId { get; set; }

        [JsonPropertyName("can_gild")]
        public bool CanGild { get; set; }

        [JsonPropertyName("spoiler")]
        public bool Spoiler { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        [JsonPropertyName("author_flair_text")]
        public object AuthorFlairText { get; set; }

        [JsonPropertyName("visited")]
        public bool Visited { get; set; }

        [JsonPropertyName("num_reports")]
        public object NumReports { get; set; }

        [JsonPropertyName("distinguished")]
        public object Distinguished { get; set; }

        [JsonPropertyName("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonPropertyName("mod_reason_by")]
        public object ModReasonBy { get; set; }

        [JsonPropertyName("removal_reason")]
        public object RemovalReason { get; set; }

        [JsonPropertyName("link_flair_background_color")]
        public string LinkFlairBackgroundColor { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_robot_indexable")]
        public bool IsRobotIndexable { get; set; }

        [JsonPropertyName("report_reasons")]
        public object ReportReasons { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("num_crossposts")]
        public long NumCrossposts { get; set; }

        [JsonPropertyName("num_comments")]
        public long NumComments { get; set; }

        [JsonPropertyName("send_replies")]
        public bool SendReplies { get; set; }

        [JsonPropertyName("whitelist_status")]
        public string WhitelistStatus { get; set; }

        [JsonPropertyName("mod_reports")]
        public List<object> ModReports { get; set; }

        [JsonPropertyName("author_flair_text_color")]
        public object AuthorFlairTextColor { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("parent_whitelist_status")]
        public string ParentWhitelistStatus { get; set; }

        [JsonPropertyName("stickied")]
        public bool Stickied { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("subreddit_subscribers")]
        public long SubredditSubscribers { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonPropertyName("media")]
        public CrosspostParentListMedia Media { get; set; }

        [JsonPropertyName("is_video")]
        public bool IsVideo { get; set; }
    }

    public partial class Gildings
    {
        [JsonPropertyName("gid_1")]
        public long Gid1 { get; set; }

        [JsonPropertyName("gid_2")]
        public long Gid2 { get; set; }

        [JsonPropertyName("gid_3")]
        public long Gid3 { get; set; }
    }

    public partial class LinkFlairRichtext
    {
        [JsonPropertyName("e")]
        public string E { get; set; }

        [JsonPropertyName("t")]
        public string T { get; set; }
    }

    public partial class CrosspostParentListMedia
    {
        [JsonPropertyName("reddit_video")]
        public RedditVideo RedditVideo { get; set; }
    }

    public partial class RedditVideo
    {
        [JsonPropertyName("fallback_url")]
        public Uri FallbackUrl { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("scrubber_media_url")]
        public Uri ScrubberMediaUrl { get; set; }

        [JsonPropertyName("dash_url")]
        public Uri DashUrl { get; set; }

        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        [JsonPropertyName("hls_url")]
        public Uri HlsUrl { get; set; }

        [JsonPropertyName("is_gif")]
        public bool IsGif { get; set; }

        [JsonPropertyName("transcoding_status")]
        public string TranscodingStatus { get; set; }
    }

    public partial class CrosspostParentListMediaEmbed
    {
    }

    public partial class Preview
    {
        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("reddit_video_preview"), NullValueHandling(NullHandling.Ignore)]
        public RedditVideo RedditVideoPreview { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }

    public partial class Image
    {
        [JsonPropertyName("source")]
        public Source Source { get; set; }

        [JsonPropertyName("resolutions")]
        public List<Source> Resolutions { get; set; }

        [JsonPropertyName("variants")]
        public Variants Variants { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public partial class Source
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }
    }

    public partial class Variants
    {
        [JsonPropertyName("gif"), NullValueHandling(NullHandling.Ignore)]
        public Gif Gif { get; set; }

        [JsonPropertyName("mp4"), NullValueHandling(NullHandling.Ignore)]
        public Gif Mp4 { get; set; }
    }

    public partial class Gif
    {
        [JsonPropertyName("source")]
        public Source Source { get; set; }

        [JsonPropertyName("resolutions")]
        public List<Source> Resolutions { get; set; }
    }

    public partial class DataMedia
    {
        [JsonPropertyName("oembed")]
        public Oembed Oembed { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public partial class Oembed
    {
        [JsonPropertyName("provider_url")]
        public Uri ProviderUrl { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("thumbnail_width")]
        public long ThumbnailWidth { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("html")]
        public string Html { get; set; }

        [JsonPropertyName("author_name"), NullValueHandling(NullHandling.Ignore)]
        public string AuthorName { get; set; }

        [JsonPropertyName("provider_name")]
        public string ProviderName { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public Uri ThumbnailUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("thumbnail_height")]
        public long ThumbnailHeight { get; set; }

        [JsonPropertyName("author_url"), NullValueHandling(NullHandling.Ignore)]
        public Uri AuthorUrl { get; set; }

        [JsonPropertyName("description"), NullValueHandling(NullHandling.Ignore)]
        public string Description { get; set; }
    }

    public partial class DataMediaEmbed
    {
        [JsonPropertyName("content"), NullValueHandling(NullHandling.Ignore)]
        public string Content { get; set; }

        [JsonPropertyName("width"), NullValueHandling(NullHandling.Ignore)]
        public long? Width { get; set; }

        [JsonPropertyName("scrolling"), NullValueHandling(NullHandling.Ignore)]
        public bool? Scrolling { get; set; }

        [JsonPropertyName("height"), NullValueHandling(NullHandling.Ignore)]
        public long? Height { get; set; }

        [JsonPropertyName("media_domain_url"), NullValueHandling(NullHandling.Ignore)]
        public Uri MediaDomainUrl { get; set; }
    }

    public partial struct Edited
    {
        public bool? Bool;
        public double? Double;

        public static implicit operator Edited(bool Bool) => new Edited { Bool = Bool };
        public static implicit operator Edited(double Double) => new Edited { Double = Double };
    }

    public partial class CommentsResult
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("data")]
        public CommentsResultData Data { get; set; }
    }

    public partial class CommentsResultData
    {
        [JsonPropertyName("modhash")]
        public object Modhash { get; set; }

        [JsonPropertyName("dist")]
        public long? Dist { get; set; }

        [JsonPropertyName("children")]
        public List<CommentChild> Children { get; set; }

        [JsonPropertyName("after")]
        public object After { get; set; }

        [JsonPropertyName("before")]
        public object Before { get; set; }
    }

    public partial class CommentChild
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("data")]
        public ChildData Data { get; set; }
    }

    public partial class ChildData
    {
        [JsonPropertyName("approved_at_utc")]
        public object ApprovedAtUtc { get; set; }

        [JsonPropertyName("subreddit")]
        public string Subreddit { get; set; }

        [JsonPropertyName("selftext"), NullValueHandling(NullHandling.Ignore)]
        public string Selftext { get; set; }

        [JsonPropertyName("user_reports")]
        public List<object> UserReports { get; set; }

        [JsonPropertyName("saved")]
        public bool Saved { get; set; }

        [JsonPropertyName("mod_reason_title")]
        public object ModReasonTitle { get; set; }

        [JsonPropertyName("gilded")]
        public long Gilded { get; set; }

        [JsonPropertyName("clicked"), NullValueHandling(NullHandling.Ignore)]
        public bool? Clicked { get; set; }

        [JsonPropertyName("title"), NullValueHandling(NullHandling.Ignore)]
        public string Title { get; set; }

        [JsonPropertyName("link_flair_richtext"), NullValueHandling(NullHandling.Ignore)]
        public List<object> LinkFlairRichtext { get; set; }

        [JsonPropertyName("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonPropertyName("hidden"), NullValueHandling(NullHandling.Ignore)]
        public bool? Hidden { get; set; }

        [JsonPropertyName("pwls")]
        public object Pwls { get; set; }

        [JsonPropertyName("link_flair_css_class")]
        public object LinkFlairCssClass { get; set; }

        [JsonPropertyName("downs")]
        public long Downs { get; set; }

        [JsonPropertyName("thumbnail_height")]
        public object ThumbnailHeight { get; set; }

        [JsonPropertyName("parent_whitelist_status")]
        public object ParentWhitelistStatus { get; set; }

        [JsonPropertyName("hide_score"), NullValueHandling(NullHandling.Ignore)]
        public bool? HideScore { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("quarantine"), NullValueHandling(NullHandling.Ignore)]
        public bool? Quarantine { get; set; }

        [JsonPropertyName("link_flair_text_color"), NullValueHandling(NullHandling.Ignore)]
        public string LinkFlairTextColor { get; set; }

        [JsonPropertyName("upvote_ratio"), NullValueHandling(NullHandling.Ignore)]
        public double? UpvoteRatio { get; set; }

        [JsonPropertyName("author_flair_background_color")]
        public object AuthorFlairBackgroundColor { get; set; }

        [JsonPropertyName("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonPropertyName("ups")]
        public long Ups { get; set; }

        [JsonPropertyName("domain"), NullValueHandling(NullHandling.Ignore)]
        public string Domain { get; set; }

        [JsonPropertyName("media_embed"), NullValueHandling(NullHandling.Ignore)]
        public MediaEmbed MediaEmbed { get; set; }

        [JsonPropertyName("thumbnail_width")]
        public object ThumbnailWidth { get; set; }

        [JsonPropertyName("author_flair_template_id")]
        public object AuthorFlairTemplateId { get; set; }

        [JsonPropertyName("is_original_content"), NullValueHandling(NullHandling.Ignore)]
        public bool? IsOriginalContent { get; set; }

        [JsonPropertyName("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonPropertyName("secure_media")]
        public object SecureMedia { get; set; }

        [JsonPropertyName("is_reddit_media_domain"), NullValueHandling(NullHandling.Ignore)]
        public bool? IsRedditMediaDomain { get; set; }

        [JsonPropertyName("is_meta"), NullValueHandling(NullHandling.Ignore)]
        public bool? IsMeta { get; set; }

        [JsonPropertyName("category")]
        public object Category { get; set; }

        [JsonPropertyName("secure_media_embed"), NullValueHandling(NullHandling.Ignore)]
        public MediaEmbed SecureMediaEmbed { get; set; }

        [JsonPropertyName("link_flair_text")]
        public object LinkFlairText { get; set; }

        [JsonPropertyName("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonPropertyName("score")]
        public long Score { get; set; }

        [JsonPropertyName("approved_by")]
        public object ApprovedBy { get; set; }

        [JsonPropertyName("thumbnail"), NullValueHandling(NullHandling.Ignore)]
        public string Thumbnail { get; set; }

        [JsonPropertyName("edited")]
        public Edited Edited { get; set; }

        [JsonPropertyName("author_flair_css_class")]
        public object AuthorFlairCssClass { get; set; }

        [JsonPropertyName("author_flair_richtext")]
        public List<object> AuthorFlairRichtext { get; set; }

        [JsonPropertyName("gildings")]
        public Gildings Gildings { get; set; }

        [JsonPropertyName("content_categories")]
        public object ContentCategories { get; set; }

        [JsonPropertyName("is_self"), NullValueHandling(NullHandling.Ignore)]
        public bool? IsSelf { get; set; }

        [JsonPropertyName("mod_note")]
        public object ModNote { get; set; }

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("link_flair_type"), NullValueHandling(NullHandling.Ignore)]
        public string LinkFlairType { get; set; }

        [JsonPropertyName("wls")]
        public object Wls { get; set; }

        [JsonPropertyName("banned_by")]
        public object BannedBy { get; set; }

        [JsonPropertyName("author_flair_type")]
        public string AuthorFlairType { get; set; }

        [JsonPropertyName("contest_mode"), NullValueHandling(NullHandling.Ignore)]
        public bool? ContestMode { get; set; }

        [JsonPropertyName("selftext_html"), NullValueHandling(NullHandling.Ignore)]
        public string SelftextHtml { get; set; }

        [JsonPropertyName("likes")]
        public object Likes { get; set; }

        [JsonPropertyName("suggested_sort")]
        public object SuggestedSort { get; set; }

        [JsonPropertyName("banned_at_utc")]
        public object BannedAtUtc { get; set; }

        [JsonPropertyName("view_count")]
        public object ViewCount { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        [JsonPropertyName("no_follow")]
        public bool NoFollow { get; set; }

        [JsonPropertyName("is_crosspostable"), NullValueHandling(NullHandling.Ignore)]
        public bool? IsCrosspostable { get; set; }

        [JsonPropertyName("pinned"), NullValueHandling(NullHandling.Ignore)]
        public bool? Pinned { get; set; }

        [JsonPropertyName("over_18"), NullValueHandling(NullHandling.Ignore)]
        public bool? Over18 { get; set; }

        [JsonPropertyName("media")]
        public object Media { get; set; }

        [JsonPropertyName("media_only"), NullValueHandling(NullHandling.Ignore)]
        public bool? MediaOnly { get; set; }

        [JsonPropertyName("link_flair_template_id")]
        public object LinkFlairTemplateId { get; set; }

        [JsonPropertyName("can_gild")]
        public bool CanGild { get; set; }

        [JsonPropertyName("spoiler"), NullValueHandling(NullHandling.Ignore)]
        public bool? Spoiler { get; set; }

        [JsonPropertyName("locked"), NullValueHandling(NullHandling.Ignore)]
        public bool? Locked { get; set; }

        [JsonPropertyName("author_flair_text")]
        public object AuthorFlairText { get; set; }

        [JsonPropertyName("visited"), NullValueHandling(NullHandling.Ignore)]
        public bool? Visited { get; set; }

        [JsonPropertyName("num_reports")]
        public object NumReports { get; set; }

        [JsonPropertyName("distinguished")]
        public object Distinguished { get; set; }

        [JsonPropertyName("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonPropertyName("mod_reason_by")]
        public object ModReasonBy { get; set; }

        [JsonPropertyName("removal_reason")]
        public object RemovalReason { get; set; }

        [JsonPropertyName("link_flair_background_color"), NullValueHandling(NullHandling.Ignore)]
        public string LinkFlairBackgroundColor { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_robot_indexable"), NullValueHandling(NullHandling.Ignore)]
        public bool? IsRobotIndexable { get; set; }

        [JsonPropertyName("report_reasons")]
        public object ReportReasons { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("num_crossposts"), NullValueHandling(NullHandling.Ignore)]
        public long? NumCrossposts { get; set; }

        [JsonPropertyName("num_comments"), NullValueHandling(NullHandling.Ignore)]
        public long? NumComments { get; set; }

        [JsonPropertyName("send_replies")]
        public bool SendReplies { get; set; }

        [JsonPropertyName("author_flair_text_color")]
        public object AuthorFlairTextColor { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("whitelist_status")]
        public object WhitelistStatus { get; set; }

        [JsonPropertyName("stickied")]
        public bool Stickied { get; set; }

        [JsonPropertyName("url"), NullValueHandling(NullHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonPropertyName("subreddit_subscribers"), NullValueHandling(NullHandling.Ignore)]
        public long? SubredditSubscribers { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonPropertyName("mod_reports")]
        public List<object> ModReports { get; set; }

        [JsonPropertyName("is_video"), NullValueHandling(NullHandling.Ignore)]
        public bool? IsVideo { get; set; }

        [JsonPropertyName("link_id"), NullValueHandling(NullHandling.Ignore)]
        public string LinkId { get; set; }

        [JsonPropertyName("replies"), NullValueHandling(NullHandling.Ignore)]
        public string Replies { get; set; }

        [JsonPropertyName("parent_id"), NullValueHandling(NullHandling.Ignore)]
        public string ParentId { get; set; }

        [JsonPropertyName("body"), NullValueHandling(NullHandling.Ignore)]
        public string Body { get; set; }

        [JsonPropertyName("collapsed"), NullValueHandling(NullHandling.Ignore)]
        public bool? Collapsed { get; set; }

        [JsonPropertyName("is_submitter"), NullValueHandling(NullHandling.Ignore)]
        public bool? IsSubmitter { get; set; }

        [JsonPropertyName("collapsed_reason")]
        public object CollapsedReason { get; set; }

        [JsonPropertyName("body_html"), NullValueHandling(NullHandling.Ignore)]
        public string BodyHtml { get; set; }

        [JsonPropertyName("score_hidden"), NullValueHandling(NullHandling.Ignore)]
        public bool? ScoreHidden { get; set; }

        [JsonPropertyName("controversiality"), NullValueHandling(NullHandling.Ignore)]
        public long? Controversiality { get; set; }

        [JsonPropertyName("depth"), NullValueHandling(NullHandling.Ignore)]
        public long? Depth { get; set; }

        public DateTime CreatedDt => DateTimeOffset.FromUnixTimeSeconds((long) CreatedUtc).DateTime;
    }

    public partial class MediaEmbed
    {
    }
}