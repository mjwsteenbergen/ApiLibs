using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Instapaper.V2
{
    /// <summary>
    /// List, create, and rename a user's tags.
    /// </summary>
    public class TagService : SubService<InstapaperV2Service>
    {
        public TagService(InstapaperV2Service service) : base(service) { }

        public async Task<List<InstapaperTag>> GetTags() => (await MakeRequest<InstapaperTagList>("tags")).Tags;

        public Task<InstapaperTag> AddTag(string name) => MakeRequest<InstapaperTag>("tags", Call.POST, content: new Dictionary<string, string> { { "name", name } });

        public Task<InstapaperTag> RenameTag(long tagId, string name) => MakeRequest<InstapaperTag>($"tags/{InstapaperV2Service.CheckId(tagId)}", Call.POST, content: new Dictionary<string, string> { { "name", name } });
        public Task<InstapaperTag> RenameTag(InstapaperTag tag, string name) => RenameTag(tag.Id, name);
    }
}
