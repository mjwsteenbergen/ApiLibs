using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ApiLibs.JSON.Trakt
{
    public class MediaRequestObject : MediaExtended {
        [JsonPropertyName("watched_at")]
        public DateTimeOffset? WatchedAt { get; set; }
    }

    public class Movie : MediaExtended {
        [JsonPropertyName("tagline")]
        public string Tagline { get; set; }

        [JsonPropertyName("released")]
        public DateTimeOffset? Released { get; set; }

        [JsonPropertyName("runtime")]
        public long? Runtime { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("trailer")]
        public string Trailer { get; set; }

        [JsonPropertyName("homepage")]
        public Uri Homepage { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonPropertyName("certification")]
        public string Certification { get; set; }
    }

    public partial class Show : ShowSmall
    {
        [JsonPropertyName("first_aired")]
        public DateTimeOffset? FirstAired { get; set; }

        [JsonPropertyName("airs")]
        public Airs Airs { get; set; }

        [JsonPropertyName("runtime")]
        public long? Runtime { get; set; }

        [JsonPropertyName("certification")]
        public string Certification { get; set; }

        [JsonPropertyName("network")]
        public string Network { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("trailer")]
        public object Trailer { get; set; }

        [JsonPropertyName("homepage")]
        public Uri Homepage { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonPropertyName("aired_episodes")]
        public long? AiredEpisodes { get; set; }
    }

    public partial class Airs
    {
        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
    }

    public partial class Watching
    {
        [JsonPropertyName("expires_at")]
        public DateTimeOffset ExpiresAt { get; set; }

        [JsonPropertyName("started_at")]
        public DateTime StartedAt { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("movie"), NullValueHandling(NullHandling.Ignore)]
        public Media Movie { get; set; }

        [JsonPropertyName("episode"), NullValueHandling(NullHandling.Ignore)]
        public Episode Episode { get; set; }

        [JsonPropertyName("show"), NullValueHandling(NullHandling.Ignore)]
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
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("watched_at")]
        public DateTimeOffset WatchedAt { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("movie"), NullValueHandling(NullHandling.Ignore)]
        public MovieType Movie { get; set; }

        [JsonPropertyName("episode"), NullValueHandling(NullHandling.Ignore)]
        public EpisodeType Episode { get; set; }

        [JsonPropertyName("show"), NullValueHandling(NullHandling.Ignore)]
        public ShowType Show { get; set; }

        [JsonPropertyName("season"), NullValueHandling(NullHandling.Ignore)]
        public SeasonType Season { get; set; }

        public override string ToString()
        {
            return Movie?.Title ?? Episode?.Title ?? Show?.Title ?? "Unkown value";
        }
    }

    public partial class Episode : Media
    {
        [JsonPropertyName("season")]
        public long Season { get; set; }

        [JsonPropertyName("number")]
        public long Number { get; set; }

        public string TorrentFormat => "S"  + Season.ToString("D2", CultureInfo.InvariantCulture) + "E" + Number.ToString("D2", CultureInfo.InvariantCulture);

    }

    public partial class EpisodeExtended : Episode
    {
        [JsonPropertyName("number_abs")]
        public object NumberAbs { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        [JsonPropertyName("rating")]
        public double? Rating { get; set; }

        [JsonPropertyName("votes")]
        public long? Votes { get; set; }

        [JsonPropertyName("comment_count")]
        public long? CommentCount { get; set; }

        [JsonPropertyName("first_aired")]
        public DateTimeOffset? FirstAired { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonPropertyName("available_translations")]
        public List<string> AvailableTranslations { get; set; }

        [JsonPropertyName("runtime")]
        public long Runtime { get; set; }
    }

    public partial class Ids
    {
        [JsonPropertyName("trakt")]
        public long? Trakt { get; set; }

        [JsonPropertyName("tvdb"), NullValueHandling(NullHandling.Ignore)]
        public long? Tvdb { get; set; }

        [JsonPropertyName("imdb"), NullValueHandling(NullHandling.Ignore)]
        public string Imdb { get; set; }

        [JsonPropertyName("tmdb"), NullValueHandling(NullHandling.Ignore)]
        public long? Tmdb { get; set; }

        [JsonPropertyName("slug"), NullValueHandling(NullHandling.Ignore)]
        public string Slug { get; set; }
    }

    public class MediaExtended : Media
    {
        [JsonPropertyName("rating")]
        public long? Rating { get; set; }

        [JsonPropertyName("votes")]
        public long? Votes { get; set; }

        [JsonPropertyName("comment_count")]
        public long? CommentCount { get; set; }

        [JsonPropertyName("available_translations")]
        public List<string> AvailableTranslations { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }
    }

    public class MovieSmall : Media {
    }

    public class ShowSmall : Media
    {
    }

    public class SeasonSmall : Media
    {

        [JsonPropertyName("number")]
        public int? Number { get; set; }

        [JsonIgnore]
        public ShowSmall Show { get; internal set; }

        public string TorrentFormat => "S" + Number?.ToString("D2", CultureInfo.InvariantCulture);
    }

    public class SeasonEpisodes : SeasonSmall 
    {
        [JsonPropertyName("episodes")]
        public List<Episode> Episodes { get; set; }
    }

    public partial class Media
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("year")]
        public long? Year { get; set; }

        [JsonPropertyName("ids")]
        public Ids Ids { get; set; }
    }
}
