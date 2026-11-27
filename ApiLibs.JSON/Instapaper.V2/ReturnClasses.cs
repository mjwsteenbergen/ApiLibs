using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.Instapaper.V2
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
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Usually an email address. Display only; it can change.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Whether the account has an active Instapaper Premium subscription.
        /// Not included in the user returned with an access token.
        /// </summary>
        [JsonPropertyName("premium")]
        public bool Premium { get; set; }
    }

    public class InstapaperAccessToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("user")]
        public InstapaperUser User { get; set; }
    }

    public class InstapaperTag
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("count")]
        public long Count { get; set; }

        /// <summary>
        /// Reserved for Instapaper's own clients; always null.
        /// </summary>
        [JsonPropertyName("baton")]
        public string Baton { get; set; }
    }

    public class InstapaperProgress
    {
        /// <summary>
        /// Reading progress from 0.0 to 1.0.
        /// </summary>
        [JsonPropertyName("percentage")]
        public double Percentage { get; set; }

        /// <summary>
        /// When the progress was recorded, as a Unix timestamp.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }

    public class InstapaperBookmark
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Null for private content saved without a URL.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("progress")]
        public InstapaperProgress Progress { get; set; }

        [JsonPropertyName("liked")]
        public bool Liked { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        /// <summary>
        /// When the bookmark was saved, as a Unix timestamp.
        /// </summary>
        [JsonPropertyName("time")]
        public long Time { get; set; }

        /// <summary>
        /// When the article was published, as a Unix timestamp, if known.
        /// </summary>
        [JsonPropertyName("pubtime")]
        public long? Pubtime { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        /// <summary>
        /// The folder it's in, or null when it's in the home list.
        /// </summary>
        [JsonPropertyName("folder_id")]
        public long? FolderId { get; set; }

        [JsonPropertyName("tags")]
        public List<InstapaperTag> Tags { get; set; }

        [JsonPropertyName("private_source")]
        public string PrivateSource { get; set; }

        /// <summary>
        /// See <see cref="InstapaperBookmarkCategory"/>. May hold values that enum doesn't know yet.
        /// </summary>
        [JsonPropertyName("category")]
        public int Category { get; set; }

        [JsonIgnore]
        public DateTimeOffset SavedAt => DateTimeOffset.FromUnixTimeSeconds(Time);
    }

    public class InstapaperFolder
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("position")]
        public long Position { get; set; }

        [JsonPropertyName("public")]
        public bool Public { get; set; }

        [JsonPropertyName("count")]
        public long Count { get; set; }
    }

    public class InstapaperHighlight
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("bookmark_id")]
        public long BookmarkId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        /// <summary>
        /// Which occurrence of the text in the article body this highlight marks, counting from 0.
        /// </summary>
        [JsonPropertyName("position")]
        public int Position { get; set; }

        /// <summary>
        /// When it was created, as a Unix timestamp.
        /// </summary>
        [JsonPropertyName("time")]
        public long Time { get; set; }
    }

    public class InstapaperArticleAuthor
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class InstapaperArticleMetadata
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author")]
        public InstapaperArticleAuthor Author { get; set; }

        [JsonPropertyName("pubtime")]
        public long? Pubtime { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("private_source")]
        public string PrivateSource { get; set; }

        [JsonPropertyName("category")]
        public int Category { get; set; }
    }

    public class InstapaperArticleContent
    {
        /// <summary>
        /// Article HTML, UTF-8, with scripts stripped.
        /// </summary>
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("images")]
        public List<string> Images { get; set; }

        [JsonPropertyName("words")]
        public long? Words { get; set; }

        [JsonPropertyName("paywalled")]
        public bool Paywalled { get; set; }

        /// <summary>
        /// ltr or rtl.
        /// </summary>
        [JsonPropertyName("direction")]
        public string Direction { get; set; }
    }

    public class InstapaperParsedArticle
    {
        [JsonPropertyName("metadata")]
        public InstapaperArticleMetadata Metadata { get; set; }

        [JsonPropertyName("content")]
        public InstapaperArticleContent Content { get; set; }
    }

    public class InstapaperBookmarkList
    {
        [JsonPropertyName("bookmarks")]
        public List<InstapaperBookmark> Bookmarks { get; set; }

        /// <summary>
        /// Size of the whole section, not of this page.
        /// </summary>
        [JsonPropertyName("total")]
        public long Total { get; set; }
    }

    public class InstapaperBookmarkChanges
    {
        [JsonPropertyName("bookmarks")]
        public List<InstapaperBookmark> Bookmarks { get; set; } = new();

        /// <summary>
        /// IDs of bookmarks deleted since the timestamp.
        /// </summary>
        [JsonPropertyName("deleted_ids")]
        public List<long> DeletedIds { get; set; } = new();

        /// <summary>
        /// Number of changed bookmarks.
        /// </summary>
        [JsonPropertyName("total")]
        public long Total { get; set; }
    }

    public class InstapaperTagChanges
    {
        /// <summary>
        /// Tags this call created.
        /// </summary>
        [JsonPropertyName("created_tags")]
        public List<InstapaperTag> CreatedTags { get; set; }

        /// <summary>
        /// The bookmark's full tag list afterwards.
        /// </summary>
        [JsonPropertyName("tags")]
        public List<InstapaperTag> Tags { get; set; }
    }

    internal class InstapaperFolderList
    {
        [JsonPropertyName("folders")]
        public List<InstapaperFolder> Folders { get; set; }
    }

    internal class InstapaperTagList
    {
        [JsonPropertyName("tags")]
        public List<InstapaperTag> Tags { get; set; }
    }

    internal class InstapaperHighlightList
    {
        [JsonPropertyName("highlights")]
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
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Send it if you have it; otherwise the title is looked up, which is slower.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("folder_id")]
        public long? FolderId { get; set; }

        [JsonPropertyName("archived")]
        public bool? Archived { get; set; }

        /// <summary>
        /// False skips resolving redirects and canonicalizing the URL.
        /// </summary>
        [JsonPropertyName("canonicalize")]
        public bool? Canonicalize { get; set; }

        /// <summary>
        /// Tag names. Tags that don't exist yet are created.
        /// </summary>
        [JsonIgnore]
        public List<string> Tags { get; set; }

        [JsonPropertyName("tags")]
        private List<InstapaperTagReference> TagReferences => Tags?.ConvertAll(InstapaperTagReference.ByName);

        /// <summary>
        /// Full article HTML, if you already have it. Required with <see cref="PrivateSource"/>.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// A short label naming where private content came from. Used instead of <see cref="Url"/>.
        /// </summary>
        [JsonPropertyName("private_source")]
        public string PrivateSource { get; set; }
    }

    internal class InstapaperTagReference
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public static InstapaperTagReference ByName(string name) => new() { Name = name };
        public static InstapaperTagReference ById(long id) => new() { Id = id };
    }

    internal class InstapaperBookmarkEdit
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("progress")]
        public InstapaperProgress Progress { get; set; }
    }

    internal class InstapaperTagEdit
    {
        [JsonPropertyName("add_tags")]
        public List<InstapaperTagReference> AddTags { get; set; }

        [JsonPropertyName("remove_tags")]
        public List<InstapaperTagReference> RemoveTags { get; set; }
    }

    internal class InstapaperParseRequest
    {
        [JsonPropertyName("use_cache")]
        public bool UseCache { get; set; }

        [JsonPropertyName("force")]
        public bool Force { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("instaparser_api_key")]
        public string InstaparserApiKey { get; set; }
    }

    internal class InstapaperFolderPosition
    {
        [JsonPropertyName("folder_id")]
        public long FolderId { get; set; }

        [JsonPropertyName("position")]
        public long Position { get; set; }
    }

    internal class InstapaperFolderReorder
    {
        [JsonPropertyName("order")]
        public List<InstapaperFolderPosition> Order { get; set; }
    }
}
