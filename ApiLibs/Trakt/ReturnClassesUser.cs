using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibs.Trakt
{
    public partial class Watching
    {
        [JsonProperty("expires_at")]
        public DateTimeOffset ExpiresAt { get; set; }

        [JsonProperty("started_at")]
        public DateTime StartedAt { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("movie", NullValueHandling = NullValueHandling.Ignore)]
        public Media Movie { get; set; }

        [JsonProperty("episode", NullValueHandling = NullValueHandling.Ignore)]
        public Episode Episode { get; set; }

        [JsonProperty("show", NullValueHandling = NullValueHandling.Ignore)]
        public Media Show { get; set; }
    }

    public partial class HistoryObject
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("watched_at")]
        public DateTimeOffset WatchedAt { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("movie", NullValueHandling = NullValueHandling.Ignore)]
        public Media Movie { get; set; }

        [JsonProperty("episode", NullValueHandling = NullValueHandling.Ignore)]
        public Episode Episode { get; set; }

        [JsonProperty("show", NullValueHandling = NullValueHandling.Ignore)]
        public Media Show { get; set; }
    }

    public partial class Episode
    {
        [JsonProperty("season")]
        public long Season { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ids")]
        public Ids Ids { get; set; }
    }

    public partial class Ids
    {
        [JsonProperty("trakt")]
        public long? Trakt { get; set; }

        [JsonProperty("tvdb", NullValueHandling = NullValueHandling.Ignore)]
        public long? Tvdb { get; set; }

        [JsonProperty("imdb", NullValueHandling = NullValueHandling.Ignore)]
        public string Imdb { get; set; }

        [JsonProperty("tmdb", NullValueHandling = NullValueHandling.Ignore)]
        public long? Tmdb { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("year")]
        public long Year { get; set; }

        [JsonProperty("ids")]
        public Ids Ids { get; set; }
    }
}
