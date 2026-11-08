using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.Yamtrack
{
    public enum YamtrackMediaType
    {
        Tv,
        Season,
        Episode,
        Movie,
        Anime,
        Manga,
        Game,
        Book,
        Comic,
        BoardGame,
    }

    public enum YamtrackSource
    {
        Tmdb,
        Mal,
        MangaUpdates,
        Igdb,
        OpenLibrary,
        Hardcover,
        ComicVine,
        Bgg,
        Manual,
    }

    /// <summary>
    /// Reads and writes <see cref="YamtrackMediaType"/> and <see cref="YamtrackSource"/> (nullable or not) as
    /// their lowercase name, the way Yamtrack spells them. It is set on each property, because a converter on
    /// the property is the only one that takes precedence over the compat layer's enum converter.
    /// </summary>
    internal sealed class YamtrackEnumConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => (Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert).IsEnum;

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var underlying = Nullable.GetUnderlyingType(typeToConvert);
            return (JsonConverter)Activator.CreateInstance(underlying == null
                ? typeof(Converter<>).MakeGenericType(typeToConvert)
                : typeof(NullableConverter<>).MakeGenericType(underlying));
        }

        private static T Parse<T>(ref Utf8JsonReader reader) where T : struct, Enum
        {
            var text = reader.GetString();
            if (Enum.TryParse<T>(text, true, out var value))
            {
                return value;
            }
            throw new JsonException($"Unknown {typeof(T).Name} \"{text}\"");
        }

        private sealed class Converter<T> : JsonConverter<T> where T : struct, Enum
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Parse<T>(ref reader);

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString().ToLowerInvariant());
        }

        private sealed class NullableConverter<T> : JsonConverter<T?> where T : struct, Enum
        {
            public override bool HandleNull => true;

            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => reader.TokenType == JsonTokenType.Null ? null : Parse<T>(ref reader);

            public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    writer.WriteStringValue(value.Value.ToString().ToLowerInvariant());
                }
            }
        }
    }

    /// <summary>Yamtrack sends and receives the status as a number.</summary>
    public enum YamtrackStatus
    {
        Planning = 0,
        InProgress = 1,
        Paused = 2,
        Completed = 3,
        Dropped = 4,
    }

    public class YamtrackPagination
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }

        [JsonPropertyName("previous")]
        public string Previous { get; set; }
    }

    public class YamtrackPaginated<T>
    {
        [JsonPropertyName("pagination")]
        public YamtrackPagination Pagination { get; set; }

        [JsonPropertyName("results")]
        public List<T> Results { get; set; }
    }

    public class YamtrackMessage
    {
        [JsonPropertyName("detail")]
        public string Detail { get; set; }
    }

    public class YamtrackItem
    {
        [JsonPropertyName("media_id")]
        public string MediaId { get; set; }

        [JsonPropertyName("source")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackSource Source { get; set; }

        [JsonPropertyName("media_type")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackMediaType MediaType { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("season_number")]
        public int? SeasonNumber { get; set; }

        [JsonPropertyName("episode_number")]
        public int? EpisodeNumber { get; set; }
    }

    public class YamtrackListMembership
    {
        [JsonPropertyName("list_id")]
        public int ListId { get; set; }

        [JsonPropertyName("list_item_id")]
        public int? ListItemId { get; set; }
    }

    /// <summary>A tracked media item, season or episode with its last consumption.</summary>
    public class YamtrackMedia
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("consumption_id")]
        public int? ConsumptionId { get; set; }

        [JsonPropertyName("item")]
        public YamtrackItem Item { get; set; }

        [JsonPropertyName("item_id")]
        public string ItemId { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("tracked")]
        public bool Tracked { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }

        [JsonPropertyName("status")]
        public YamtrackStatus? Status { get; set; }

        [JsonPropertyName("progress")]
        public int? Progress { get; set; }

        [JsonPropertyName("progressed_at")]
        public DateTime? ProgressedAt { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("lists")]
        public List<YamtrackListMembership> Lists { get; set; }
    }

    /// <summary>One consumption (watch, read, play...) of an item.</summary>
    public class YamtrackHistory
    {
        [JsonPropertyName("consumption_id")]
        public int ConsumptionId { get; set; }

        [JsonPropertyName("created")]
        public DateTime? Created { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }

        [JsonPropertyName("progress")]
        public int? Progress { get; set; }

        [JsonPropertyName("progressed_at")]
        public DateTime? ProgressedAt { get; set; }

        [JsonPropertyName("status")]
        public YamtrackStatus? Status { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }

    /// <summary>The fields <see cref="YamtrackCompleteMedia"/> and <see cref="YamtrackCompleteEpisode"/> share.</summary>
    public abstract class YamtrackCompleteMediaBase
    {
        /// <summary>Id of the tracked item, null when the item is not tracked.</summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("media_id")]
        public string MediaId { get; set; }

        [JsonPropertyName("source")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackSource? Source { get; set; }

        [JsonPropertyName("source_url")]
        public string SourceUrl { get; set; }

        [JsonPropertyName("media_type")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackMediaType? MediaType { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("max_progress")]
        public int? MaxProgress { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("synopsis")]
        public string Synopsis { get; set; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }

        [JsonPropertyName("score_count")]
        public int? ScoreCount { get; set; }

        /// <summary>Provider specific related media (seasons, sequels...), keyed by relation.</summary>
        [JsonPropertyName("related")]
        public Dictionary<string, object> Related { get; set; }

        [JsonPropertyName("item_id")]
        public string ItemId { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("tracked")]
        public bool Tracked { get; set; }

        [JsonPropertyName("consumptions_number")]
        public int ConsumptionsNumber { get; set; }

        [JsonPropertyName("consumptions")]
        public List<YamtrackHistory> Consumptions { get; set; }

        [JsonPropertyName("lists")]
        public List<YamtrackListMembership> Lists { get; set; }

        /// <summary>The seasons of a tv show, from <see cref="Related"/>. Empty for anything else.</summary>
        [JsonIgnore]
        public List<YamtrackMedia> Seasons => GetRelated<YamtrackMedia>("seasons");

        /// <summary>The episodes of a season, from <see cref="Related"/>. Empty for anything else.</summary>
        [JsonIgnore]
        public List<YamtrackMedia> Episodes => GetRelated<YamtrackMedia>("episodes");

        /// <summary>Reads one entry of <see cref="Related"/> as a list of <typeparamref name="T"/>.</summary>
        public List<T> GetRelated<T>(string key) => Related != null && Related.TryGetValue(key, out var value) && value is JsonElement { ValueKind: JsonValueKind.Array } element
            ? NewtonsoftCompat.Deserialize<List<T>>(element.GetRawText())
            : new List<T>();
    }

    /// <summary>A media item or season with its metadata and all its consumptions.</summary>
    public class YamtrackCompleteMedia : YamtrackCompleteMediaBase
    {
        /// <summary>Provider specific metadata, such as the release date, runtime or studios.</summary>
        [JsonPropertyName("details")]
        public Dictionary<string, object> Details { get; set; }

        /// <summary>
        /// Reads one entry of <see cref="Details"/>, for example <c>status</c> (<c>Released</c> for a released
        /// TMDB movie), <c>release_date</c> or <c>tvdb_id</c>. Returns the default when it is missing or null.
        /// </summary>
        public T GetDetail<T>(string key)
        {
            if (Details == null || !Details.TryGetValue(key, out var value) || value == null)
            {
                return default;
            }

            // Objects and arrays arrive as JsonElement, numbers, text and dates as plain values.
            var json = value is JsonElement element ? element.GetRawText() : NewtonsoftCompat.Serialize(value);
            return NewtonsoftCompat.Deserialize<T>(json);
        }
    }

    public class YamtrackCompleteEpisode : YamtrackCompleteMediaBase
    {
        [JsonPropertyName("details")]
        public YamtrackEpisodeDetails Details { get; set; }
    }

    public class YamtrackEpisodeDetails
    {
        [JsonPropertyName("air_date")]
        public DateTime? AirDate { get; set; }

        [JsonPropertyName("episode_number")]
        public int? EpisodeNumber { get; set; }

        [JsonPropertyName("season_number")]
        public int? SeasonNumber { get; set; }

        /// <summary>Runtime in minutes.</summary>
        [JsonPropertyName("runtime")]
        public int? Runtime { get; set; }

        [JsonPropertyName("episode_type")]
        public string EpisodeType { get; set; }

        [JsonPropertyName("crew")]
        public List<Dictionary<string, object>> Crew { get; set; }

        [JsonPropertyName("guest_stars")]
        public List<Dictionary<string, object>> GuestStars { get; set; }
    }

    public class YamtrackRelatedMedia
    {
        [JsonPropertyName("source")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackSource? Source { get; set; }

        [JsonPropertyName("media_type")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackMediaType? MediaType { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("media_id")]
        public string MediaId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class YamtrackSearchResult
    {
        [JsonPropertyName("media_id")]
        public string MediaId { get; set; }

        [JsonPropertyName("source")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackSource Source { get; set; }

        [JsonPropertyName("media_type")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackMediaType MediaType { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
    }

    public class YamtrackChangedField
    {
        /// <summary>One of <c>score</c>, <c>status</c>, <c>progress</c>, <c>start_date</c>, <c>end_date</c> or <c>notes</c>.</summary>
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("old_value")]
        public object OldValue { get; set; }

        [JsonPropertyName("new_value")]
        public object NewValue { get; set; }
    }

    public class YamtrackChange
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("item_id")]
        public string ItemId { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonPropertyName("changes")]
        public List<YamtrackChangedField> Changes { get; set; }
    }

    public class YamtrackUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }

    public class YamtrackListSummary
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("owner")]
        public YamtrackUser Owner { get; set; }

        [JsonPropertyName("collaborators")]
        public List<YamtrackUser> Collaborators { get; set; }

        [JsonPropertyName("items_count")]
        public int ItemsCount { get; set; }

        [JsonPropertyName("latest_update")]
        public DateTime? LatestUpdate { get; set; }
    }

    public class YamtrackList : YamtrackListSummary
    {
        [JsonPropertyName("items")]
        public YamtrackPaginated<YamtrackMedia> Items { get; set; }
    }

    /// <summary>Body to create or edit a list. Properties left null are not changed.</summary>
    public class YamtrackListEdit
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>User ids of the collaborators.</summary>
        [JsonPropertyName("collaborators")]
        public List<int> Collaborators { get; set; }
    }

    public class YamtrackCalendarEvent
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("item")]
        public YamtrackItem Item { get; set; }

        [JsonPropertyName("item_id")]
        public string ItemId { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        /// <summary>Episode or chapter number of the release.</summary>
        [JsonPropertyName("content_number")]
        public long? ContentNumber { get; set; }

        [JsonPropertyName("datetime")]
        public DateTime Datetime { get; set; }

        [JsonPropertyName("notification_sent")]
        public bool NotificationSent { get; set; }
    }

    public class YamtrackInfo
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("debug")]
        public bool Debug { get; set; }

        [JsonPropertyName("frontend_url")]
        public string FrontendUrl { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("admin_enabled")]
        public bool AdminEnabled { get; set; }

        [JsonPropertyName("track_time")]
        public bool TrackTime { get; set; }
    }

    public class YamtrackHealthCheck
    {
        /// <summary><c>ok</c> or <c>error</c></summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    public class YamtrackHealth
    {
        /// <summary><c>ok</c> or <c>unavailable</c></summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("checks")]
        public Dictionary<string, YamtrackHealthCheck> Checks { get; set; }
    }

    public class YamtrackMediaCount
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("tv")]
        public int? Tv { get; set; }

        [JsonPropertyName("season")]
        public int? Season { get; set; }

        [JsonPropertyName("movie")]
        public int? Movie { get; set; }

        [JsonPropertyName("anime")]
        public int? Anime { get; set; }

        [JsonPropertyName("manga")]
        public int? Manga { get; set; }

        [JsonPropertyName("game")]
        public int? Game { get; set; }

        [JsonPropertyName("book")]
        public int? Book { get; set; }

        [JsonPropertyName("comic")]
        public int? Comic { get; set; }

        [JsonPropertyName("boardgame")]
        public int? BoardGame { get; set; }
    }

    public class YamtrackTimelineItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("item_id")]
        public string ItemId { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("media_type")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackMediaType? MediaType { get; set; }

        [JsonPropertyName("source")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackSource? Source { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }

        [JsonPropertyName("progress")]
        public int? Progress { get; set; }

        [JsonPropertyName("progressed_at")]
        public DateTime? ProgressedAt { get; set; }

        [JsonPropertyName("status")]
        public YamtrackStatus? Status { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }

    /// <summary>Statistics as shown on Yamtrack's statistics page. The chart data is passed through as sent.</summary>
    public class YamtrackStatistics
    {
        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("media_count")]
        public YamtrackMediaCount MediaCount { get; set; }

        [JsonPropertyName("activity_data")]
        public object ActivityData { get; set; }

        [JsonPropertyName("media_type_distribution")]
        public Dictionary<string, object> MediaTypeDistribution { get; set; }

        [JsonPropertyName("score_distribution")]
        public Dictionary<string, object> ScoreDistribution { get; set; }

        [JsonPropertyName("top_rated")]
        public List<YamtrackMedia> TopRated { get; set; }

        [JsonPropertyName("status_distribution")]
        public Dictionary<string, object> StatusDistribution { get; set; }

        [JsonPropertyName("status_pie_chart_data")]
        public object StatusPieChartData { get; set; }

        /// <summary>Consumptions grouped by month.</summary>
        [JsonPropertyName("timeline")]
        public Dictionary<string, List<YamtrackTimelineItem>> Timeline { get; set; }
    }

    /// <summary>
    /// Body to start tracking an item. For a provider item set <see cref="Source"/> and <see cref="MediaId"/>
    /// (the provider's data overwrites <see cref="Title"/> and <see cref="Image"/>). For a manual item use
    /// <see cref="YamtrackSource.Manual"/>, leave <see cref="MediaId"/> empty and set <see cref="Title"/>.
    /// </summary>
    public class YamtrackTrackRequest
    {
        [JsonPropertyName("source")]
        [JsonConverter(typeof(YamtrackEnumConverter))]
        public YamtrackSource? Source { get; set; }

        [JsonPropertyName("media_id")]
        public string MediaId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }

        [JsonPropertyName("status")]
        public YamtrackStatus? Status { get; set; }

        [JsonPropertyName("progress")]
        public int? Progress { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("season_number")]
        public int? SeasonNumber { get; set; }

        [JsonPropertyName("episode_number")]
        public int? EpisodeNumber { get; set; }

        /// <summary>Id of the tracked show a manual season belongs to.</summary>
        [JsonPropertyName("parent_tv")]
        public int? ParentTv { get; set; }

        /// <summary>Id of the tracked season a manual episode belongs to.</summary>
        [JsonPropertyName("parent_season")]
        public int? ParentSeason { get; set; }
    }

    /// <summary>
    /// Body to edit a consumption. Properties left null are not sent and stay as they are. Which
    /// properties a media type accepts differs: movies have no progress, tv shows and seasons only
    /// take score, status and notes, and episodes only take end date and notes.
    /// </summary>
    public class YamtrackUpdate
    {
        [JsonPropertyName("score")]
        public double? Score { get; set; }

        [JsonPropertyName("status")]
        public YamtrackStatus? Status { get; set; }

        [JsonPropertyName("progress")]
        public int? Progress { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }
}
