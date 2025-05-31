using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApiLibs.Feedly
{
    public partial class Profile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("client")]
        public string Client { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("wave")]
        public string Wave { get; set; }

        [JsonProperty("logins")]
        public List<Login> Logins { get; set; }

        [JsonProperty("picture")]
        public Uri Picture { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("familyName")]
        public string FamilyName { get; set; }

        [JsonProperty("google")]
        public string Google { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("dropboxConnected")]
        public bool DropboxConnected { get; set; }

        [JsonProperty("twitterConnected")]
        public bool TwitterConnected { get; set; }

        [JsonProperty("facebookConnected")]
        public bool FacebookConnected { get; set; }

        [JsonProperty("evernoteConnected")]
        public bool EvernoteConnected { get; set; }

        [JsonProperty("pocketConnected")]
        public bool PocketConnected { get; set; }

        [JsonProperty("wordPressConnected")]
        public bool WordPressConnected { get; set; }

        [JsonProperty("windowsLiveConnected")]
        public bool WindowsLiveConnected { get; set; }

        [JsonProperty("instapaperConnected")]
        public bool InstapaperConnected { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("reader")]
        public string Reader { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }

    public partial class Login
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("picture")]
        public Uri Picture { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("providerId")]
        public string ProviderId { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }

    public partial class Stream
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("continuation")]
        public string Continuation { get; set; }

        [JsonProperty("items")]
        public List<Article> Items { get; set; }
    }

    public partial class Article
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("keywords", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Keywords { get; set; }

        [JsonProperty("originId")]
        public string OriginId { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("crawled")]
        public long Crawled { get; set; }

        [JsonProperty("published")]
        public long Published { get; set; }

        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public Content Summary { get; set; }

        [JsonProperty("origin")]
        public Origin Origin { get; set; }

        [JsonProperty("alternate")]
        public List<Alternate> Alternate { get; set; }

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public string Author { get; set; }

        [JsonProperty("visual", NullValueHandling = NullValueHandling.Ignore)]
        public Visual Visual { get; set; }

        [JsonProperty("canonicalUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri CanonicalUrl { get; set; }

        [JsonProperty("ampUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri AmpUrl { get; set; }

        [JsonProperty("cdnAmpUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri CdnAmpUrl { get; set; }

        [JsonProperty("unread")]
        public bool Unread { get; set; }

        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }

        [JsonProperty("tags")]
        public List<Category> Tags { get; set; }

        [JsonProperty("actionTimestamp")]
        public long ActionTimestamp { get; set; }

        [JsonProperty("engagement", NullValueHandling = NullValueHandling.Ignore)]
        public long? Engagement { get; set; }

        [JsonProperty("engagementRate", NullValueHandling = NullValueHandling.Ignore)]
        public double? EngagementRate { get; set; }

        [JsonProperty("recrawled", NullValueHandling = NullValueHandling.Ignore)]
        public long? Recrawled { get; set; }

        [JsonProperty("updateCount", NullValueHandling = NullValueHandling.Ignore)]
        public long? UpdateCount { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public Content Content { get; set; }

        [JsonProperty("canonical", NullValueHandling = NullValueHandling.Ignore)]
        public List<Alternate> Canonical { get; set; }

        [JsonProperty("memes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Meme> Memes { get; set; }

        [JsonProperty("entities", NullValueHandling = NullValueHandling.Ignore)]
        public List<Entity> Entities { get; set; }

        [JsonProperty("updated", NullValueHandling = NullValueHandling.Ignore)]
        public long? Updated { get; set; }

        [JsonProperty("enclosure", NullValueHandling = NullValueHandling.Ignore)]
        public List<Enclosure> Enclosure { get; set; }
    }

    public partial class Alternate
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Category
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("content")]
        public string ContentContent { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }
    }

    public partial class Enclosure
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Entity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("mentions")]
        public List<Mention> Mentions { get; set; }

        [JsonProperty("salienceLevel")]
        public string SalienceLevel { get; set; }
    }

    public partial class Mention
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class Meme
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public partial class Origin
    {
        [JsonProperty("streamId")]
        public string StreamId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("htmlUrl")]
        public Uri HtmlUrl { get; set; }
    }

    public partial class Visual
    {
        [JsonProperty("processor", NullValueHandling = NullValueHandling.Ignore)]
        public string Processor { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long? Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long? Height { get; set; }

        [JsonProperty("contentType", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; }
    }
}
