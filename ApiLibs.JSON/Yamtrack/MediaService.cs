using ApiLibs.JSON.General;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs.JSON.Yamtrack
{
    /// <summary>
    /// The tracked media of the user. Methods that act on one item take its <c>item_id</c>
    /// (see <see cref="YamtrackService.ItemId"/>), which can point to a media item, a season or an episode.
    /// </summary>
    public class MediaService : SubService<YamtrackService>
    {
        public MediaService(YamtrackService service) : base(service, "media")
        {
        }

        /// <summary>Lists tracked media.</summary>
        /// <param name="sort">A field with an optional direction, for example <c>score_desc</c> or <c>title</c></param>
        public Task<YamtrackPaginated<YamtrackMedia>> GetMedia(YamtrackMediaType? mediaType = null, YamtrackStatus? status = null, string search = null, string sort = null, IEnumerable<YamtrackMediaType> exclude = null, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackMedia>>("", parameters: new List<Param>
        {
            new OParam("media_type", mediaType == null ? null : YamtrackService.ToApiString(mediaType.Value)),
            new OParam("status", status == null ? null : StatusFilter(status.Value)),
            new OParam("search", search),
            new OParam("sort", sort),
            new OParam("exclude", exclude == null || !exclude.Any() ? null : string.Join(",", exclude.Select(YamtrackService.ToApiString))),
            new OParam("limit", limit?.ToString()),
            new OParam("offset", offset?.ToString()),
        });

        /// <summary>
        /// Starts tracking a media item. Set <see cref="YamtrackTrackRequest.Source"/> and <see cref="YamtrackTrackRequest.MediaId"/>
        /// to add an item from a provider, or use <see cref="YamtrackSource.Manual"/> to create one by hand.
        /// </summary>
        public Task<YamtrackMedia> Track(YamtrackMediaType mediaType, YamtrackTrackRequest request) => MakeRequest<YamtrackMedia>($"{YamtrackService.ToApiString(mediaType)}/", Call.POST, content: request, statusCode: HttpStatusCode.Created);

        /// <summary>Gets a tracked media item or season. Use <see cref="GetEpisode"/> for episodes.</summary>
        public Task<YamtrackCompleteMedia> GetItem(string itemId) => MakeRequest<YamtrackCompleteMedia>($"{itemId}/");

        public Task<YamtrackCompleteEpisode> GetEpisode(string itemId) => MakeRequest<YamtrackCompleteEpisode>($"{itemId}/");

        /// <summary>Edits the last consumption of a media item or season. Use <see cref="UpdateEpisode"/> for episodes.</summary>
        public Task<YamtrackCompleteMedia> UpdateItem(string itemId, YamtrackUpdate update) => MakeRequest<YamtrackCompleteMedia>($"{itemId}/", Call.PATCH, content: update);

        /// <summary>Edits the last watch of an episode. Episodes only accept <see cref="YamtrackUpdate.EndDate"/> and <see cref="YamtrackUpdate.Notes"/>.</summary>
        public Task<YamtrackCompleteEpisode> UpdateEpisode(string itemId, YamtrackUpdate update) => MakeRequest<YamtrackCompleteEpisode>($"{itemId}/", Call.PATCH, content: update);

        /// <summary>Stops tracking an item and deletes its history.</summary>
        public Task DeleteItem(string itemId) => MakeRequest($"{itemId}/", Call.DELETE, statusCode: HttpStatusCode.NoContent);

        /// <summary>Refreshes the metadata of the item from its provider in the background.</summary>
        public Task<YamtrackMessage> Sync(string itemId) => MakeRequest<YamtrackMessage>($"{itemId}/sync/", Call.POST, statusCode: HttpStatusCode.Accepted);

        public Task<YamtrackPaginated<YamtrackHistory>> GetHistory(string itemId, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackHistory>>($"{itemId}/history/", parameters: Paging(limit, offset));

        public Task<YamtrackHistory> GetHistoryEntry(string itemId, int consumptionId) => MakeRequest<YamtrackHistory>($"{itemId}/history/{consumptionId}/");

        public Task<YamtrackHistory> UpdateHistoryEntry(string itemId, int consumptionId, YamtrackUpdate update) => MakeRequest<YamtrackHistory>($"{itemId}/history/{consumptionId}/", Call.PATCH, content: update);

        public Task DeleteHistoryEntry(string itemId, int consumptionId) => MakeRequest($"{itemId}/history/{consumptionId}/", Call.DELETE, statusCode: HttpStatusCode.NoContent);

        /// <summary>Lists the edits made to the item. Single entries are read and deleted through <see cref="YamtrackService.GetChange"/>.</summary>
        public Task<YamtrackPaginated<YamtrackChange>> GetChangesHistory(string itemId, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackChange>>($"{itemId}/changes_history/", parameters: Paging(limit, offset));

        /// <summary>Lists the lists the item is in.</summary>
        public Task<YamtrackPaginated<YamtrackListMembership>> GetLists(string itemId, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackListMembership>>($"{itemId}/lists/", parameters: Paging(limit, offset));

        public Task<List<YamtrackListMembership>> AddToList(string itemId, int listId) => MakeRequest<List<YamtrackListMembership>>($"{itemId}/lists/{listId}/", Call.PUT);

        public Task RemoveFromList(string itemId, int listId) => MakeRequest($"{itemId}/lists/{listId}/", Call.DELETE, statusCode: HttpStatusCode.NoContent);

        public Task<List<YamtrackRelatedMedia>> GetRecommendations(string itemId) => MakeRequest<List<YamtrackRelatedMedia>>($"{itemId}/recommendations/");

        /// <param name="itemId">item_id of a tv show</param>
        public Task<YamtrackPaginated<YamtrackMedia>> GetSeasons(string itemId, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackMedia>>($"{itemId}/seasons/", parameters: Paging(limit, offset));

        /// <param name="itemId">item_id of a season</param>
        public Task<YamtrackPaginated<YamtrackMedia>> GetEpisodes(string itemId, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackMedia>>($"{itemId}/episodes/", parameters: Paging(limit, offset));

        internal static List<Param> Paging(int? limit, int? offset) => new()
        {
            new OParam("limit", limit?.ToString()),
            new OParam("offset", offset?.ToString()),
        };

        private static string StatusFilter(YamtrackStatus status) => status switch
        {
            YamtrackStatus.Planning => "Planning",
            YamtrackStatus.InProgress => "In progress",
            YamtrackStatus.Paused => "Paused",
            YamtrackStatus.Completed => "Completed",
            YamtrackStatus.Dropped => "Dropped",
            _ => throw new System.ArgumentOutOfRangeException(nameof(status)),
        };
    }
}
