using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Feedly
{
    public class FeedlyService : Service
    {
        public FeedlyService() : base("https://cloud.feedly.com") { }

        private string UserId;

        public FeedlyService(string token) : base("https://cloud.feedly.com/v3/")
        {
            AddStandardHeader("Authorization", $"OAuth ${token}");
        }

        public async Task<Stream> GetSavedArticles()
        {
            await GetUserId();
            return await MakeRequest<Stream>($"streams/contents?streamId=user/{UserId}/tag/global.saved");
        }

        private async Task GetUserId()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = (await GetUser()).Id;
            }
        }

        public Task<Profile> GetUser()
        {
            return MakeRequest<Profile>("profile");
        }

        public Task MarkAsUnsaved(Article a)
        {
            return MarkAsUnsaved(a.Id);
        }

        public Task MarkAsUnsaved(string id)
        {
            return HandleRequest("/markers", Call.POST, content: new
            {
                action = "markAsUnsaved",
                type = "entries",
                entryIds = new[] {
                    id
                }
            });
        }

        public Task MarkAsSaved(Article a)
        {
            return MarkAsSaved(a.Id);
        }

        public Task MarkAsSaved(string id)
        {
            return HandleRequest("/markers", Call.POST, content: new
            {
                action = "markAsSaved",
                type = "entries",
                entryIds = new[] {
                    id
                }
            });
        }

        public Task<Stream> GetItemsFromFeed(string feedId)
        {
            return MakeRequest<Stream>($"streams/contents?streamId={feedId}");
        }
    }
}
