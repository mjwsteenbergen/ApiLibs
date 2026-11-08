using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs.JSON.Yamtrack
{
    /// <summary>
    /// Client for the Yamtrack REST API (<c>/api/v1/</c>). The token is the one shown on the
    /// Integrations section of Yamtrack's Settings page.
    /// </summary>
    public class YamtrackService : RestSharpService
    {
        public MediaService MediaService { get; }
        public ListService ListService { get; }

        /// <param name="url">Root url of the Yamtrack instance, for example <c>https://yamtrack.example.com</c></param>
        /// <param name="token">API token of the user</param>
        public YamtrackService(string url, string token) : base(url.TrimEnd('/') + "/api/v1/")
        {
            AddStandardHeader("Authorization", $"Bearer {token}");

            MediaService = new MediaService(this);
            ListService = new ListService(this);
        }

        /// <summary>
        /// Builds the <c>item_id</c> Yamtrack uses to identify an item, for example <c>movie/tmdb/603</c>,
        /// <c>tv/tmdb/1396/1</c> (a season) or <c>tv/tmdb/1396/1/2</c> (an episode).
        /// </summary>
        public static string ItemId(YamtrackMediaType mediaType, YamtrackSource source, string mediaId, int? seasonNumber = null, int? episodeNumber = null)
        {
            if (episodeNumber != null && seasonNumber == null)
            {
                throw new ArgumentException("An episode needs a season number", nameof(seasonNumber));
            }

            var id = $"{ToApiString(mediaType)}/{ToApiString(source)}/{mediaId}";
            if (seasonNumber != null)
            {
                id += $"/{seasonNumber}";
            }
            if (episodeNumber != null)
            {
                id += $"/{episodeNumber}";
            }
            return id;
        }

        public Task<YamtrackInfo> GetInfo() => MakeRequest<YamtrackInfo>("info/");

        /// <summary>Returns the health of the instance. An unavailable instance answers with 503 and its failing checks.</summary>
        public Task<YamtrackHealth> GetHealth() => MakeRequest(new Request<YamtrackHealth>("health/")
        {
            ExpectedStatusCode = new HttpStatusCode[] { HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable },
        });

        public Task<YamtrackStatistics> GetStatistics(DateTime? startDate = null, DateTime? endDate = null) => MakeRequest<YamtrackStatistics>("statistics/", parameters: new List<Param>
        {
            new OParam("start_date", ToApiDate(startDate)),
            new OParam("end_date", ToApiDate(endDate)),
        });

        /// <summary>Searches a provider for media. Without a source the default provider of the media type is used.</summary>
        public Task<YamtrackPaginated<YamtrackSearchResult>> Search(YamtrackMediaType mediaType, string query, YamtrackSource? source = null, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackSearchResult>>($"search/{ToApiString(mediaType)}/", parameters: new List<Param>
        {
            new Param("search", query),
            new OParam("source", source == null ? null : ToApiString(source.Value)),
            new OParam("limit", limit?.ToString()),
            new OParam("offset", offset?.ToString()),
        });

        /// <summary>Lists the upcoming releases of tracked media. Without dates, the current month is returned.</summary>
        public Task<YamtrackPaginated<YamtrackCalendarEvent>> GetCalendar(DateTime? startDate = null, DateTime? endDate = null, int? year = null, int? month = null, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackCalendarEvent>>("calendar/", parameters: new List<Param>
        {
            new OParam("start_date", ToApiDate(startDate)),
            new OParam("end_date", ToApiDate(endDate)),
            new OParam("year", year?.ToString()),
            new OParam("month", month?.ToString()),
            new OParam("limit", limit?.ToString()),
            new OParam("offset", offset?.ToString()),
        });

        /// <summary>Starts a background refresh of the calendar.</summary>
        public Task<YamtrackMessage> UpdateCalendar() => MakeRequest<YamtrackMessage>("calendar/update/", Call.POST, statusCode: HttpStatusCode.Accepted);

        public Task<YamtrackChange> GetChange(YamtrackMediaType mediaType, int historyId) => MakeRequest<YamtrackChange>($"changes_history/{ToApiString(mediaType)}/{historyId}/");

        public Task DeleteChange(YamtrackMediaType mediaType, int historyId) => MakeRequest($"changes_history/{ToApiString(mediaType)}/{historyId}/", Call.DELETE, statusCode: HttpStatusCode.NoContent);

        internal static string ToApiString(YamtrackMediaType mediaType) => mediaType.ToString().ToLowerInvariant();

        internal static string ToApiString(YamtrackSource source) => source.ToString().ToLowerInvariant();

        internal static string ToApiDate(DateTime? date) => date?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}
