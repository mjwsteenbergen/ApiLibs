

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class NotionRestService : RestSharpService
    {
        public NotionRestService(string key) : base("https://api.notion.com/v1/")
        {
            AddStandardHeader("Authorization", "Bearer " + key);
            AddStandardHeader("Notion-Version", "2021-08-16");
            Page = new NotionRestPageService(this);
            Files = new NotionRestFileService(this);
        }

        public NotionRestPageService Page { get; private set; }
        public NotionRestFileService Files { get; private set; }

        public Task<NotionDatabase> GetDatabase(Guid id) => GetDatabase(id.ToString());
        public Task<NotionDatabase> GetDatabase(string id) => MakeRequest<NotionDatabase>("databases/" + id);

        public Task<NotionUnionTypeWrapper<NotionBlock>> GetBlock(Guid blockId) => GetBlock(blockId.ToString());
        public Task<NotionUnionTypeWrapper<NotionBlock>> GetBlock(string blockId) => MakeRequest<NotionUnionTypeWrapper<NotionBlock>>($"blocks/{blockId}");

        public async IAsyncEnumerable<NotionBlock> GetBlockChildren(string blockId, string startCursor = null, int? size = null)
        {
            do
            {
                var list = await MakeRequest<NotionList<NotionUnionTypeWrapper<NotionBlock>>>($"blocks/{blockId}/children", parameters: new List<Param> {
                    new OParam("start_cursor", startCursor),
                    new OParam("page_size", size)
                });
                foreach (var item in list.Results)
                {
                    yield return item.Value;
                }
                startCursor = list.NextCursor;
            } while (startCursor != null);
        }

        public Task<NotionList<NotionUnionTypeWrapper<NotionBlock>>> AddBlockChild(string blockId, List<INotionBlock> blocks) => MakeRequest<NotionList<NotionUnionTypeWrapper<NotionBlock>>>($"blocks/{blockId}/children", method: Call.PATCH, content: new
        {
            children = blocks
        });
        public Task<NotionList<NotionUnionTypeWrapper<NotionBlock>>> AddBlockChild(NotionBlock block, List<INotionBlock> blocks) => AddBlockChild(block.Id.ToString(), blocks);
        public Task<NotionList<NotionUnionTypeWrapper<NotionBlock>>> AddBlockChild(Page page, List<INotionBlock> blocks) => AddBlockChild(page.Id.ToString(), blocks);

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

        private class Parent
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("database_id")]
            public Guid? DatabaseId { get; set; }

            [JsonProperty("page_id")]
            public Guid? PageId { get; set; }
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

    public class QueryParams
    {
        [JsonProperty("page_size")]
        public int? PageSize { get; set; }

        [JsonProperty("start_cursor")]
        public string StartCursor { get; set; }
    }

    public class NotionRestFileService : SubService<NotionRestService>
    {
        public class FileUpload
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }
        }

        public NotionRestFileService(NotionRestService service) : base(service)
        {
        }

        public async Task<FileUpload> UploadExternalFile(string url, string fileName)
        {
            var resp = await MakeRequest<FileUpload>("file_uploads", content: new
            {
                mode = "external_url",
                external_url = url,
                filename = fileName
            });

            FileUpload fileUpload;

            do
            {
                fileUpload = await MakeRequest<FileUpload>($"file_uploads/{resp.Id}");
            } while (fileUpload.Status != "uploaded");

            return fileUpload;
        }
    }
}