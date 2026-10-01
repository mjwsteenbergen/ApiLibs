using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.Instapaper.V2
{
    /// <summary>
    /// Which list of bookmarks to read.
    /// </summary>
    public enum InstapaperBookmarkSection
    {
        Home,
        Archive,
        Liked,
        Folder,
        Tag
    }

    /// <summary>
    /// Known values of <see cref="InstapaperBookmark.Category"/>. New categories may be added,
    /// so treat an unknown value as an article.
    /// </summary>
    public enum InstapaperBookmarkCategory
    {
        Article = 0,
        Email = 1,
        Video = 2,
        PDF = 3,
        Social = 4
    }

    public class InstapaperUser
    {
        /// <summary>
        /// Stable numeric ID. Store this as your reference to the user.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Usually an email address. Display only; it can change.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Whether the account has an active Instapaper Premium subscription.
        /// Not included in the user returned with an access token.
        /// </summary>
        [JsonProperty("premium")]
        public bool Premium { get; set; }
    }

    public class InstapaperAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("user")]
        public InstapaperUser User { get; set; }
    }

    public class InstapaperTag
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        /// <summary>
        /// Reserved for Instapaper's own clients; always null.
        /// </summary>
        [JsonProperty("baton")]
        public string Baton { get; set; }
    }

    public class InstapaperProgress
    {
        /// <summary>
        /// Reading progress from 0.0 to 1.0.
        /// </summary>
        [JsonProperty("percentage")]
        public double Percentage { get; set; }

        /// <summary>
        /// When the progress was recorded, as a Unix timestamp.
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }

    public class InstapaperBookmark
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Null for private content saved without a URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("progress")]
        public InstapaperProgress Progress { get; set; }

        [JsonProperty("liked")]
        public bool Liked { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        /// <summary>
        /// When the bookmark was saved, as a Unix timestamp.
        /// </summary>
        [JsonProperty("time")]
        public long Time { get; set; }

        /// <summary>
        /// When the article was published, as a Unix timestamp, if known.
        /// </summary>
        [JsonProperty("pubtime")]
        public long? Pubtime { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// The folder it's in, or null when it's in the home list.
        /// </summary>
        [JsonProperty("folder_id")]
        public long? FolderId { get; set; }

        [JsonProperty("tags")]
        public List<InstapaperTag> Tags { get; set; }

        [JsonProperty("private_source")]
        public string PrivateSource { get; set; }

        /// <summary>
        /// See <see cref="InstapaperBookmarkCategory"/>. May hold values that enum doesn't know yet.
        /// </summary>
        [JsonProperty("category")]
        public int Category { get; set; }

        [JsonIgnore]
        public DateTimeOffset SavedAt => DateTimeOffset.FromUnixTimeSeconds(Time);
    }

    public class InstapaperFolder
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }
    }

    public class InstapaperHighlight
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("bookmark_id")]
        public long BookmarkId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        /// <summary>
        /// Which occurrence of the text in the article body this highlight marks, counting from 0.
        /// </summary>
        [JsonProperty("position")]
        public int Position { get; set; }

        /// <summary>
        /// When it was created, as a Unix timestamp.
        /// </summary>
        [JsonProperty("time")]
        public long Time { get; set; }
    }

    public class InstapaperArticleAuthor
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class InstapaperArticleMetadata
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public InstapaperArticleAuthor Author { get; set; }

        [JsonProperty("pubtime")]
        public long? Pubtime { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("private_source")]
        public string PrivateSource { get; set; }

        [JsonProperty("category")]
        public int Category { get; set; }
    }

    public class InstapaperArticleContent
    {
        /// <summary>
        /// Article HTML, UTF-8, with scripts stripped.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("images")]
        public List<string> Images { get; set; }

        [JsonProperty("words")]
        public long? Words { get; set; }

        [JsonProperty("paywalled")]
        public bool Paywalled { get; set; }

        /// <summary>
        /// ltr or rtl.
        /// </summary>
        [JsonProperty("direction")]
        public string Direction { get; set; }
    }

    public class InstapaperParsedArticle
    {
        [JsonProperty("metadata")]
        public InstapaperArticleMetadata Metadata { get; set; }

        [JsonProperty("content")]
        public InstapaperArticleContent Content { get; set; }
    }

    public class InstapaperBookmarkList
    {
        [JsonProperty("bookmarks")]
        public List<InstapaperBookmark> Bookmarks { get; set; }

        /// <summary>
        /// Size of the whole section, not of this page.
        /// </summary>
        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public class InstapaperBookmarkChanges
    {
        [JsonProperty("bookmarks")]
        public List<InstapaperBookmark> Bookmarks { get; set; } = new();

        /// <summary>
        /// IDs of bookmarks deleted since the timestamp.
        /// </summary>
        [JsonProperty("deleted_ids")]
        public List<long> DeletedIds { get; set; } = new();

        /// <summary>
        /// Number of changed bookmarks.
        /// </summary>
        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public class InstapaperTagChanges
    {
        /// <summary>
        /// Tags this call created.
        /// </summary>
        [JsonProperty("created_tags")]
        public List<InstapaperTag> CreatedTags { get; set; }

        /// <summary>
        /// The bookmark's full tag list afterwards.
        /// </summary>
        [JsonProperty("tags")]
        public List<InstapaperTag> Tags { get; set; }
    }

    internal class InstapaperFolderList
    {
        [JsonProperty("folders")]
        public List<InstapaperFolder> Folders { get; set; }
    }

    internal class InstapaperTagList
    {
        [JsonProperty("tags")]
        public List<InstapaperTag> Tags { get; set; }
    }

    internal class InstapaperHighlightList
    {
        [JsonProperty("highlights")]
        public List<InstapaperHighlight> Highlights { get; set; }
    }

    /// <summary>
    /// A new bookmark. Set either <see cref="Url"/> or <see cref="PrivateSource"/> together with <see cref="Content"/>.
    /// Saving a URL twice updates the existing bookmark.
    /// </summary>
    public class InstapaperNewBookmark
    {
        /// <summary>
        /// Required unless <see cref="PrivateSource"/> is set.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Send it if you have it; otherwise the title is looked up, which is slower.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("folder_id")]
        public long? FolderId { get; set; }

        [JsonProperty("archived")]
        public bool? Archived { get; set; }

        /// <summary>
        /// False skips resolving redirects and canonicalizing the URL.
        /// </summary>
        [JsonProperty("canonicalize")]
        public bool? Canonicalize { get; set; }

        /// <summary>
        /// Tag names. Tags that don't exist yet are created.
        /// </summary>
        [JsonIgnore]
        public List<string> Tags { get; set; }

        [JsonProperty("tags")]
        private List<InstapaperTagReference> TagReferences => Tags?.ConvertAll(InstapaperTagReference.ByName);

        /// <summary>
        /// Full article HTML, if you already have it. Required with <see cref="PrivateSource"/>.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// A short label naming where private content came from. Used instead of <see cref="Url"/>.
        /// </summary>
        [JsonProperty("private_source")]
        public string PrivateSource { get; set; }
    }

    internal class InstapaperTagReference
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public static InstapaperTagReference ByName(string name) => new() { Name = name };
        public static InstapaperTagReference ById(long id) => new() { Id = id };
    }

    internal class InstapaperBookmarkEdit
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("progress")]
        public InstapaperProgress Progress { get; set; }
    }

    internal class InstapaperTagEdit
    {
        [JsonProperty("add_tags")]
        public List<InstapaperTagReference> AddTags { get; set; }

        [JsonProperty("remove_tags")]
        public List<InstapaperTagReference> RemoveTags { get; set; }
    }

    internal class InstapaperParseRequest
    {
        [JsonProperty("use_cache")]
        public bool UseCache { get; set; }

        [JsonProperty("force")]
        public bool Force { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("instaparser_api_key")]
        public string InstaparserApiKey { get; set; }
    }

    internal class InstapaperFolderPosition
    {
        [JsonProperty("folder_id")]
        public long FolderId { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }
    }

    internal class InstapaperFolderReorder
    {
        [JsonProperty("order")]
        public List<InstapaperFolderPosition> Order { get; set; }
    }
}
