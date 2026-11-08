using ApiLibs.JSON.General;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs.JSON.Yamtrack
{
    /// <summary>The user's own lists and the lists shared with them.</summary>
    public class ListService : SubService<YamtrackService>
    {
        public ListService(YamtrackService service) : base(service, "lists")
        {
        }

        /// <param name="sort">One of <c>items</c>, <c>name</c>, <c>new</c> or <c>update</c>, with <c>_asc</c> or <c>_desc</c></param>
        public Task<YamtrackPaginated<YamtrackListSummary>> GetLists(string search = null, string sort = null, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackListSummary>>("", parameters: new List<Param>
        {
            new OParam("search", search),
            new OParam("sort", sort),
            new OParam("limit", limit?.ToString()),
            new OParam("offset", offset?.ToString()),
        });

        /// <param name="collaborators">User ids of the collaborators</param>
        public Task<YamtrackListSummary> CreateList(string name, string description = null, List<int> collaborators = null) => MakeRequest<YamtrackListSummary>("", Call.POST, content: new YamtrackListEdit
        {
            Name = name,
            Description = description,
            Collaborators = collaborators,
        }, statusCode: HttpStatusCode.Created);

        /// <summary>Gets a list with a page of its items.</summary>
        public Task<YamtrackList> GetList(int listId, string search = null, string sort = null, int? limit = null, int? offset = null) => MakeRequest<YamtrackList>($"{listId}/", parameters: ItemParameters(search, sort, limit, offset));

        public Task<YamtrackListSummary> UpdateList(int listId, YamtrackListEdit edit) => MakeRequest<YamtrackListSummary>($"{listId}/", Call.PATCH, content: edit);

        public Task DeleteList(int listId) => MakeRequest($"{listId}/", Call.DELETE, statusCode: HttpStatusCode.NoContent);

        public Task<YamtrackPaginated<YamtrackMedia>> GetItems(int listId, string search = null, string sort = null, int? limit = null, int? offset = null) => MakeRequest<YamtrackPaginated<YamtrackMedia>>($"{listId}/items/", parameters: ItemParameters(search, sort, limit, offset));

        public Task<List<YamtrackMedia>> GetAllItems(int listId, string search = null, string sort = null)
            => YamtrackService.AllPages((limit, offset) => GetItems(listId, search, sort, limit, offset));

        public Task<List<YamtrackListSummary>> GetAllLists(string search = null, string sort = null)
            => YamtrackService.AllPages((limit, offset) => GetLists(search, sort, limit, offset));

        /// <param name="listItemId">The <see cref="YamtrackListMembership.ListItemId"/> of the item</param>
        public Task<YamtrackCompleteMedia> GetItem(int listId, int listItemId) => MakeRequest<YamtrackCompleteMedia>($"{listId}/items/{listItemId}/");

        /// <param name="listItemId">The <see cref="YamtrackListMembership.ListItemId"/> of the item</param>
        public Task RemoveItem(int listId, int listItemId) => MakeRequest($"{listId}/items/{listItemId}/", Call.DELETE, statusCode: HttpStatusCode.NoContent);

        private static List<Param> ItemParameters(string search, string sort, int? limit, int? offset) => new()
        {
            new OParam("search", search),
            new OParam("sort", sort),
            new OParam("limit", limit?.ToString()),
            new OParam("offset", offset?.ToString()),
        };
    }
}
