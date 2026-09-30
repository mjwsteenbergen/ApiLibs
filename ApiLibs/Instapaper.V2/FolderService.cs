using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Instapaper.V2
{
    /// <summary>
    /// List, create, reorder, and delete a user's folders.
    /// </summary>
    public class FolderService : SubService<InstapaperV2Service>
    {
        public FolderService(InstapaperV2Service service) : base(service) { }

        /// <summary>
        /// The user's folders, in their own order.
        /// </summary>
        public async Task<List<InstapaperFolder>> GetFolders() => (await MakeRequest<InstapaperFolderList>("folders")).Folders;

        /// <summary>
        /// Finds a folder by title
        /// Library implemented
        /// </summary>
        public async Task<InstapaperFolder> GetFolder(string title) => (await GetFolders()).FirstOrDefault(i => i.Title == title) ?? throw new KeyNotFoundException("Your folder could not be found");

        public Task<InstapaperFolder> AddFolder(string title) => MakeRequest<InstapaperFolder>("folders", Call.POST, content: new Dictionary<string, string> { { "title", title } });

        /// <summary>
        /// Delete a folder. Its bookmarks move back to the home list.
        /// </summary>
        public Task DeleteFolder(long folderId) => MakeRequest($"folders/{InstapaperV2Service.CheckId(folderId)}", Call.DELETE);
        public Task DeleteFolder(InstapaperFolder folder) => DeleteFolder(folder.Id);

        /// <summary>
        /// Set folder positions. Folders you leave out keep their positions. Returns every folder in its new order.
        /// </summary>
        /// <param name="positions">Map of folder id to position. Positions start at 1.</param>
        public async Task<List<InstapaperFolder>> ReorderFolders(IDictionary<long, long> positions)
        {
            if (positions == null || positions.Count == 0)
            {
                throw new ArgumentException("ReorderFolders needs at least one folder", nameof(positions));
            }
            var order = positions.Select(pair =>
            {
                // The API skips a position of 0, so positions start at 1.
                if (pair.Value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(positions), pair.Value, "Folder positions must be positive");
                }
                return new InstapaperFolderPosition { FolderId = InstapaperV2Service.CheckId(pair.Key, nameof(positions)), Position = pair.Value };
            }).ToList();
            return (await MakeRequest<InstapaperFolderList>("folders/reorder", Call.POST, content: new InstapaperFolderReorder { Order = order })).Folders;
        }
    }
}
