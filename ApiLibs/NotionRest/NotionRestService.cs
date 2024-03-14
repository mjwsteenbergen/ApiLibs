

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using Newtonsoft.Json;
using RestSharp;

namespace ApiLibs.NotionRest
{
    public class NotionRestService : RestSharpService
    {
        public NotionRestService(string key) : base("https://api.notion.com/v1/")
        {
            AddStandardHeader("Authorization", "Bearer " + key);
            AddStandardHeader("Notion-Version", "2021-08-16");
            Page = new NotionRestPageService(this);
        }

        public NotionRestPageService Page { get; private set; }

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

        public Task<IEnumerable<NotionBlock>> AddBlockChild(string blockId, List<NotionBlock> blocks) => MakeRequest<IEnumerable<NotionBlock>>($"blocks/{blockId}/children", method: Call.PATCH, content: blocks);
        public Task<IEnumerable<NotionBlock>> AddBlockChild(NotionBlock block, List<NotionBlock> blocks) => MakeRequest<IEnumerable<NotionBlock>>(block.Id.ToString(), method: Call.PATCH, content: blocks);
        public Task<IEnumerable<NotionBlock>> AddBlockChild(Page page, List<NotionBlock> blocks) => MakeRequest<IEnumerable<NotionBlock>>(page.Id.ToString(), method: Call.PATCH, content: blocks);

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

        private class Parent {
            [JsonProperty("type")]
            public string Type { get; set;}

            [JsonProperty("database_id")]
            public Guid? DatabaseId { get; set;}

            [JsonProperty("page_id")]
            public Guid? PageId { get; set;}
        }   
    }

    public class NotionRestPageService : SubService<NotionRestService>
    {
        public NotionRestPageService(NotionRestService service) : base(service)
        {
        }

        private Task<Page> Create(Parent parent, Dictionary<string, NotionProperty> props) => MakeRequest<Page>("pages", Call.POST, content: new
        {
            parent,
            properties = props
        });

        public Task<Page> Create(NotionDatabase parent, Dictionary<string, NotionProperty> props) => Create(new Parent
        {
            Type = "database_id",
            DatabaseId = parent.Id
        }, props);

        public Task<Page> Update(Guid id, Page page) => MakeRequest<Page>($"pages/{id}", Call.PATCH, content: page);

        public Task<Page> Get(Guid guid) => MakeRequest<Page>("pages/" + guid);
    }

    public class QueryParams {
        [JsonProperty("page_size")]
        public int? PageSize { get; set;}

        [JsonProperty("start_cursor")]
        public string StartCursor { get; set;}
    }
}