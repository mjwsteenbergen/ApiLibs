using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ApiLibs.Trakt
{
    public class MediaRequestObject : MediaExtended {
        [JsonProperty("watched_at")]
        public DateTimeOffset? WatchedAt { get; set; }
    }

    public class Movie : MediaExtended {
        [JsonProperty("tagline")]
        public string Tagline { get; set; }

        [JsonProperty("released")]
        public DateTimeOffset? Released { get; set; }

        [JsonProperty("runtime")]
        public long? Runtime { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("trailer")]
        public string Trailer { get; set; }

        [JsonProperty("homepage")]
        public Uri Homepage { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("certification")]
        public string Certification { get; set; }
    }

    public partial class Show : ShowSmall
    {
        [JsonProperty("first_aired")]
        public DateTimeOffset? FirstAired { get; set; }

        [JsonProperty("airs")]
        public Airs Airs { get; set; }

        [JsonProperty("runtime")]
        public long? Runtime { get; set; }

        [JsonProperty("certification")]
        public string Certification { get; set; }

        [JsonProperty("network")]
        public string Network { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("trailer")]
        public object Trailer { get; set; }

        [JsonProperty("homepage")]
        public Uri Homepage { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("aired_episodes")]
        public long? AiredEpisodes { get; set; }
    }

    public partial class Airs
    {
        [JsonProperty("day")]
        public string Day { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }

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

    public partial class WrappedMediaObject : WrappedMediaObject<MovieSmall, ShowSmall, SeasonSmall, Episode>
    {
    }

    public partial class ExtendedWrappedMediaObject : WrappedMediaObject<Movie, Show, SeasonSmall, EpisodeExtended>
    {
    }

    public partial class WrappedMediaObject<MovieType, ShowType, SeasonType, EpisodeType> 
        where MovieType : Media
        where ShowType : Media
        where SeasonType : Media
        where EpisodeType : Media
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("watched_at")]
        public DateTimeOffset WatchedAt { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("movie", NullValueHandling = NullValueHandling.Ignore)]
        public MovieType Movie { get; set; }

        [JsonProperty("episode", NullValueHandling = NullValueHandling.Ignore)]
        public EpisodeType Episode { get; set; }

        [JsonProperty("show", NullValueHandling = NullValueHandling.Ignore)]
        public ShowType Show { get; set; }

        [JsonProperty("season", NullValueHandling = NullValueHandling.Ignore)]
        public SeasonType Season { get; set; }

        public override string ToString()
        {
            return Movie?.Title ?? Episode?.Title ?? Show?.Title ?? "Unkown value";
        }
    }

    public partial class Episode : Media
    {
        [JsonProperty("season")]
        public long Season { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        public string TorrentFormat => "S"  + Season.ToString("D2", CultureInfo.InvariantCulture) + "E" + Number.ToString("D2", CultureInfo.InvariantCulture);

    }

    public partial class EpisodeExtended : Episode
    {
        [JsonProperty("number_abs")]
        public object NumberAbs { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("rating")]
        public double? Rating { get; set; }

        [JsonProperty("votes")]
        public long? Votes { get; set; }

        [JsonProperty("comment_count")]
        public long? CommentCount { get; set; }

        [JsonProperty("first_aired")]
        public DateTimeOffset? FirstAired { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty("available_translations")]
        public List<string> AvailableTranslations { get; set; }

        [JsonProperty("runtime")]
        public long Runtime { get; set; }
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

    public class MediaExtended : Media
    {
        [JsonProperty("rating")]
        public long? Rating { get; set; }

        [JsonProperty("votes")]
        public long? Votes { get; set; }

        [JsonProperty("comment_count")]
        public long? CommentCount { get; set; }

        [JsonProperty("available_translations")]
        public List<string> AvailableTranslations { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }
    }

    public class MovieSmall : Media {
    }

    public class ShowSmall : Media
    {
    }

    public class SeasonSmall : Media
    {

        [JsonProperty("number")]
        public int? Number { get; set; }

        [JsonIgnore]
        public ShowSmall Show { get; internal set; }

        public string TorrentFormat => "S" + Number?.ToString("D2", CultureInfo.InvariantCulture);
    }

    public class SeasonEpisodes : SeasonSmall 
    {
        [JsonProperty("episodes")]
        public List<Episode> Episodes { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("year")]
        public long? Year { get; set; }

        [JsonProperty("ids")]
        public Ids Ids { get; set; }
    }
}
