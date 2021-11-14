

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class NotionRestService : RestSharpService
    {
        public NotionRestService(string key) : base("https://api.notion.com/v1/")
        {
            AddStandardHeader("Authorization", "Bearer " + key);
            AddStandardHeader("Notion-Version", "2021-08-16");
        }

        public Task<NotionDatabase> GetDatabase(Guid id) => GetDatabase(id.ToString());
        public Task<NotionDatabase> GetDatabase(string id) => MakeRequest<NotionDatabase>("databases/" + id);

        public Task<NotionBlock> GetBlock(Guid blockId) => GetBlock(blockId.ToString());
        public Task<NotionBlock> GetBlock(string blockId) => MakeRequest<NotionBlock>($"blocks/{blockId}");

        public async IAsyncEnumerable<NotionBlock> GetBlockChildren(string blockId, string startCursor = null, int? size = null) {
            do
            {
                var list = await MakeRequest<NotionList<NotionBlock>>($"blocks/{blockId}/children", parameters: new List<Param> {
                    new OParam("start_cursor", startCursor),
                    new OParam("page_size", size)
                });
                foreach (var item in list.Results)
                {
                    yield return item;
                }
                startCursor = list.NextCursor;
            } while (startCursor != null);
        }

        public IAsyncEnumerable<NotionBlock> GetBlockChildren(Guid blockId, string startCursor = null, int? size = null) => GetBlockChildren(blockId.ToString(), startCursor, size);

        public IAsyncEnumerable<Page> QueryDatabase(Guid id, QueryParams param = null) => QueryDatabase(id.ToString(), param);
        public async IAsyncEnumerable<Page> QueryDatabase(string id, QueryParams param = null)
        {
            param ??= new QueryParams();
            do
            {
                var list = await MakeRequest<NotionList<Page>>($"databases/{id}/query", Call.POST, content: param);
                foreach (var item in list.Results)
                {
                    yield return item;
                }
                param.StartCursor = list.NextCursor;
            } while (param.StartCursor != null);
        }

    }

    public class QueryParams {
        [JsonProperty("page_size")]
        public int? PageSize { get; set;}

        [JsonProperty("start_cursor")]
        public string StartCursor { get; set;}
    }
}