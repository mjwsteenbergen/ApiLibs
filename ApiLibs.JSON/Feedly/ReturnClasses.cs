using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;
using System;
using System.Collections.Generic;

namespace ApiLibs.JSON.Feedly
{
    public partial class Profile
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("client")]
        public string Client { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("wave")]
        public string Wave { get; set; }

        [JsonPropertyName("logins")]
        public List<Login> Logins { get; set; }

        [JsonPropertyName("picture")]
        public Uri Picture { get; set; }

        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("familyName")]
        public string FamilyName { get; set; }

        [JsonPropertyName("google")]
        public string Google { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("dropboxConnected")]
        public bool DropboxConnected { get; set; }

        [JsonPropertyName("twitterConnected")]
        public bool TwitterConnected { get; set; }

        [JsonPropertyName("facebookConnected")]
        public bool FacebookConnected { get; set; }

        [JsonPropertyName("evernoteConnected")]
        public bool EvernoteConnected { get; set; }

        [JsonPropertyName("pocketConnected")]
        public bool PocketConnected { get; set; }

        [JsonPropertyName("wordPressConnected")]
        public bool WordPressConnected { get; set; }

        [JsonPropertyName("windowsLiveConnected")]
        public bool WindowsLiveConnected { get; set; }

        [JsonPropertyName("instapaperConnected")]
        public bool InstapaperConnected { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("reader")]
        public string Reader { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
    }

    public partial class Login
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        [JsonPropertyName("picture")]
        public Uri Picture { get; set; }

        [JsonPropertyName("provider")]
        public string Provider { get; set; }

        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
    }

    public partial class Stream
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("continuation")]
        public string Continuation { get; set; }

        [JsonPropertyName("items")]
        public List<Article> Items { get; set; }
    }

    public partial class Article
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("keywords"), NullValueHandling(NullHandling.Ignore)]
        public List<string> Keywords { get; set; }

        [JsonPropertyName("originId")]
        public string OriginId { get; set; }

        [JsonPropertyName("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("crawled")]
        public long Crawled { get; set; }

        [JsonPropertyName("published")]
        public long Published { get; set; }

        [JsonPropertyName("summary"), NullValueHandling(NullHandling.Ignore)]
        public Content Summary { get; set; }

        [JsonPropertyName("origin")]
        public Origin Origin { get; set; }

        [JsonPropertyName("alternate")]
        public List<Alternate> Alternate { get; set; }

        [JsonPropertyName("author"), NullValueHandling(NullHandling.Ignore)]
        public string Author { get; set; }

        [JsonPropertyName("visual"), NullValueHandling(NullHandling.Ignore)]
        public Visual Visual { get; set; }

        [JsonPropertyName("canonicalUrl"), NullValueHandling(NullHandling.Ignore)]
        public Uri CanonicalUrl { get; set; }

        [JsonPropertyName("ampUrl"), NullValueHandling(NullHandling.Ignore)]
        public Uri AmpUrl { get; set; }

        [JsonPropertyName("cdnAmpUrl"), NullValueHandling(NullHandling.Ignore)]
        public Uri CdnAmpUrl { get; set; }

        [JsonPropertyName("unread")]
        public bool Unread { get; set; }

        [JsonPropertyName("categories")]
        public List<Category> Categories { get; set; }

        [JsonPropertyName("tags")]
        public List<Category> Tags { get; set; }

        [JsonPropertyName("actionTimestamp")]
        public long ActionTimestamp { get; set; }

        [JsonPropertyName("engagement"), NullValueHandling(NullHandling.Ignore)]
        public long? Engagement { get; set; }

        [JsonPropertyName("engagementRate"), NullValueHandling(NullHandling.Ignore)]
        public double? EngagementRate { get; set; }

        [JsonPropertyName("recrawled"), NullValueHandling(NullHandling.Ignore)]
        public long? Recrawled { get; set; }

        [JsonPropertyName("updateCount"), NullValueHandling(NullHandling.Ignore)]
        public long? UpdateCount { get; set; }

        [JsonPropertyName("content"), NullValueHandling(NullHandling.Ignore)]
        public Content Content { get; set; }

        [JsonPropertyName("canonical"), NullValueHandling(NullHandling.Ignore)]
        public List<Alternate> Canonical { get; set; }

        [JsonPropertyName("memes"), NullValueHandling(NullHandling.Ignore)]
        public List<Meme> Memes { get; set; }

        [JsonPropertyName("entities"), NullValueHandling(NullHandling.Ignore)]
        public List<Entity> Entities { get; set; }

        [JsonPropertyName("updated"), NullValueHandling(NullHandling.Ignore)]
        public long? Updated { get; set; }

        [JsonPropertyName("enclosure"), NullValueHandling(NullHandling.Ignore)]
        public List<Enclosure> Enclosure { get; set; }
    }

    public partial class Alternate
    {
        [JsonPropertyName("href")]
        public Uri Href { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public partial class Category
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }
    }

    public partial class Content
    {
        [JsonPropertyName("content")]
        public string ContentContent { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }
    }

    public partial class Enclosure
    {
        [JsonPropertyName("href")]
        public Uri Href { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public partial class Entity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("mentions")]
        public List<Mention> Mentions { get; set; }

        [JsonPropertyName("salienceLevel")]
        public string SalienceLevel { get; set; }
    }

    public partial class Mention
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public partial class Meme
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }
    }

    public partial class Origin
    {
        [JsonPropertyName("streamId")]
        public string StreamId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("htmlUrl")]
        public Uri HtmlUrl { get; set; }
    }

    public partial class Visual
    {
        [JsonPropertyName("processor"), NullValueHandling(NullHandling.Ignore)]
        public string Processor { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width"), NullValueHandling(NullHandling.Ignore)]
        public long? Width { get; set; }

        [JsonPropertyName("height"), NullValueHandling(NullHandling.Ignore)]
        public long? Height { get; set; }

        [JsonPropertyName("contentType"), NullValueHandling(NullHandling.Ignore)]
        public string ContentType { get; set; }
    }
}
