using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Instapaper.V2
{
    /// <summary>
    /// List, save, update, move, like, tag, delete, and parse bookmarks.
    /// </summary>
    public class BookmarkService : SubService<InstapaperV2Service>
    {
        private const int MaxPageSize = 500;

        public BookmarkService(InstapaperV2Service service) : base(service) { }

        /// <summary>
        /// One page of bookmarks from a section.
        /// </summary>
        /// <param name="section">Defaults to home, or to the section implied by <paramref name="folderId"/> or <paramref name="tag"/></param>
        /// <param name="folderId">Only bookmarks in this folder</param>
        /// <param name="tag">Only bookmarks with this tag name. Can't be combined with <paramref name="folderId"/></param>
        /// <param name="limit">1 to 500. Defaults to 25.</param>
        /// <param name="offset">Number of bookmarks to skip</param>
        public Task<InstapaperBookmarkList> GetBookmarks(InstapaperBookmarkSection? section = null, long? folderId = null, string tag = null, int? limit = null, int? offset = null)
        {
            CheckSection(section, folderId, tag);
            if (limit.HasValue)
            {
                CheckPageSize(limit.Value, nameof(limit));
            }
            return MakeRequest<InstapaperBookmarkList>("bookmarks", parameters: new List<Param>
            {
                new OParam("section", section?.ToString().ToLowerInvariant()),
                new OParam("folder_id", folderId?.ToString()),
                new OParam("tag", tag),
                new OParam("limit", limit?.ToString()),
                new OParam("offset", offset?.ToString()),
            });
        }

        public Task<InstapaperBookmarkList> GetBookmarks(InstapaperFolder folder, int? limit = null, int? offset = null) => GetBookmarks(folderId: folder.Id, limit: limit, offset: offset);

        /// <summary>
        /// Every bookmark in a section, fetching pages as you go.
        /// </summary>
        /// <param name="pageSize">Bookmarks fetched per request, 1 to 500.</param>
        public async IAsyncEnumerable<InstapaperBookmark> GetAllBookmarks(InstapaperBookmarkSection? section = null, long? folderId = null, string tag = null, int pageSize = 100)
        {
            CheckPageSize(pageSize, nameof(pageSize));
            CheckSection(section, folderId, tag);
            int offset = 0;
            while (true)
            {
                var page = await GetBookmarks(section, folderId, tag, pageSize, offset);
                foreach (var bookmark in page.Bookmarks)
                {
                    yield return bookmark;
                }
                offset += page.Bookmarks.Count;
                // A short page is the end. Total can lag behind, so don't stop on it.
                if (page.Bookmarks.Count < pageSize)
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// One page of bookmarks changed since a time, across every section, plus deleted IDs.
        /// </summary>
        /// <param name="limit">1 to 500. Defaults to 500.</param>
        public async Task<InstapaperBookmarkChanges> GetChanges(DateTimeOffset since, int limit = MaxPageSize, int? offset = null)
        {
            CheckPageSize(limit, nameof(limit));
            var changes = await MakeRequest<InstapaperBookmarkChanges>("bookmarks", parameters: new List<Param>
            {
                new Param("since", ToTimestamp(since).ToString()),
                new Param("limit", limit.ToString()),
                new OParam("offset", offset?.ToString()),
            });
            changes.Bookmarks ??= new List<InstapaperBookmark>();
            changes.DeletedIds ??= new List<long>();
            return changes;
        }

        /// <summary>
        /// Everything changed since a time, fetching every page. Record when you started the sync
        /// and pass it as <paramref name="since"/> next time.
        /// </summary>
        /// <param name="pageSize">Changes fetched per request, 1 to 500.</param>
        public async Task<InstapaperBookmarkChanges> Sync(DateTimeOffset since, int pageSize = MaxPageSize)
        {
            CheckPageSize(pageSize, nameof(pageSize));
            var result = new InstapaperBookmarkChanges();
            int offset = 0;
            while (true)
            {
                var page = await GetChanges(since, pageSize, offset);
                result.Bookmarks.AddRange(page.Bookmarks);
                result.DeletedIds.AddRange(page.DeletedIds);
                result.Total = page.Total;
                // A page holds up to pageSize entries, counting deleted IDs.
                int received = page.Bookmarks.Count + page.DeletedIds.Count;
                offset += received;
                if (received < pageSize)
                {
                    return result;
                }
            }
        }

        /// <summary>
        /// Save a URL. Saving a URL twice updates the existing bookmark.
        /// </summary>
        /// <param name="url">The URL to save</param>
        /// <param name="title">Send it if you have it; otherwise the title is looked up, which is slower.</param>
        /// <param name="tags">Tag names. Tags that don't exist yet are created.</param>
        public Task<InstapaperBookmark> AddBookmark(string url, string title = null, string description = null, long? folderId = null, IEnumerable<string> tags = null) => AddBookmark(new InstapaperNewBookmark
        {
            Url = url,
            Title = title,
            Description = description,
            FolderId = folderId,
            Tags = tags?.ToList(),
        });

        /// <summary>
        /// Save a URL, or private content.
        /// </summary>
        public Task<InstapaperBookmark> AddBookmark(InstapaperNewBookmark bookmark)
        {
            if (bookmark.Url == null && bookmark.PrivateSource == null)
            {
                throw new ArgumentException("Url is required unless PrivateSource is set");
            }
            if (bookmark.Url != null && bookmark.PrivateSource != null)
            {
                throw new ArgumentException("Url and PrivateSource cannot be used together");
            }
            if (bookmark.PrivateSource != null && bookmark.Content == null)
            {
                throw new ArgumentException("PrivateSource requires Content");
            }
            return MakeRequest<InstapaperBookmark>("bookmarks", Call.POST, content: bookmark);
        }

        /// <summary>
        /// Change a bookmark's title, description, or reading progress.
        /// </summary>
        /// <param name="progress">Reading progress from 0.0 to 1.0</param>
        /// <param name="progressTime">When the progress was recorded. Defaults to now.</param>
        public Task<InstapaperBookmark> UpdateBookmark(long bookmarkId, string title = null, string description = null, double? progress = null, DateTimeOffset? progressTime = null)
        {
            if (title == null && description == null && progress == null)
            {
                throw new ArgumentException("An update needs a title, description, or progress");
            }
            if (progress == null && progressTime != null)
            {
                throw new ArgumentException("progressTime requires progress", nameof(progressTime));
            }
            if (progress is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(progress), progress, "progress must be between 0 and 1");
            }
            return MakeRequest<InstapaperBookmark>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}", Call.POST, content: new InstapaperBookmarkEdit
            {
                Title = title,
                Description = description,
                Progress = progress.HasValue ? new InstapaperProgress
                {
                    Percentage = progress.Value,
                    Timestamp = (progressTime ?? DateTimeOffset.UtcNow).ToUnixTimeSeconds()
                } : null
            });
        }

        public Task<InstapaperBookmark> UpdateReadProgress(InstapaperBookmark bookmark, double progress, DateTimeOffset? time = null) => UpdateBookmark(bookmark.Id, progress: progress, progressTime: time);

        /// <summary>
        /// Permanently delete a bookmark. This is NOT the same as archiving. Please be clear to users if you're going to do this.
        /// </summary>
        public Task DeleteBookmark(long bookmarkId) => MakeRequest($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}", Call.DELETE);
        public Task DeleteBookmark(InstapaperBookmark bookmark) => DeleteBookmark(bookmark.Id);

        public Task<InstapaperBookmark> ArchiveBookmark(long bookmarkId) => Move(bookmarkId, "archive");
        public Task<InstapaperBookmark> ArchiveBookmark(InstapaperBookmark bookmark) => ArchiveBookmark(bookmark.Id);

        /// <summary>
        /// Move a bookmark back to the home list.
        /// </summary>
        public Task<InstapaperBookmark> UnarchiveBookmark(long bookmarkId) => Move(bookmarkId, "home");
        public Task<InstapaperBookmark> UnarchiveBookmark(InstapaperBookmark bookmark) => UnarchiveBookmark(bookmark.Id);

        public Task<InstapaperBookmark> MoveBookmark(long bookmarkId, long folderId) => Move(bookmarkId, InstapaperV2Service.CheckId(folderId, nameof(folderId)).ToString());
        public Task<InstapaperBookmark> MoveBookmark(InstapaperBookmark bookmark, InstapaperFolder folder) => MoveBookmark(bookmark.Id, folder.Id);

        private Task<InstapaperBookmark> Move(long bookmarkId, string section) => MakeRequest<InstapaperBookmark>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/move", Call.POST, content: new Dictionary<string, string> { { "section", section } });

        public Task<InstapaperBookmark> LikeBookmark(long bookmarkId) => MakeRequest<InstapaperBookmark>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/like", Call.POST);
        public Task<InstapaperBookmark> LikeBookmark(InstapaperBookmark bookmark) => LikeBookmark(bookmark.Id);

        public Task<InstapaperBookmark> UnlikeBookmark(long bookmarkId) => MakeRequest<InstapaperBookmark>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/like", Call.DELETE);
        public Task<InstapaperBookmark> UnlikeBookmark(InstapaperBookmark bookmark) => UnlikeBookmark(bookmark.Id);

        /// <summary>
        /// Add tags to a bookmark and remove tags from it.
        /// </summary>
        /// <param name="addNames">Tag names to add. Tags that don't exist yet are created.</param>
        /// <param name="addIds">Tag IDs to add</param>
        /// <param name="removeIds">Tag IDs to remove</param>
        public Task<InstapaperTagChanges> UpdateTags(long bookmarkId, IEnumerable<string> addNames = null, IEnumerable<long> addIds = null, IEnumerable<long> removeIds = null)
        {
            var add = (addNames ?? Enumerable.Empty<string>()).Select(name =>
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Tags to add must be non-empty names", nameof(addNames));
                }
                return InstapaperTagReference.ByName(name);
            }).Concat((addIds ?? Enumerable.Empty<long>()).Select(tagId => InstapaperTagReference.ById(InstapaperV2Service.CheckId(tagId, nameof(addIds))))).ToList();
            var remove = (removeIds ?? Enumerable.Empty<long>()).Select(tagId => InstapaperTagReference.ById(InstapaperV2Service.CheckId(tagId, nameof(removeIds)))).ToList();

            if (add.Count == 0 && remove.Count == 0)
            {
                throw new ArgumentException("UpdateTags needs at least one tag to add or remove");
            }

            return MakeRequest<InstapaperTagChanges>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/tags", Call.POST, content: new InstapaperTagEdit
            {
                AddTags = add,
                RemoveTags = remove,
            });
        }

        public Task<InstapaperTagChanges> AddTags(InstapaperBookmark bookmark, params string[] names) => UpdateTags(bookmark.Id, addNames: names);
        public Task<InstapaperTagChanges> RemoveTags(InstapaperBookmark bookmark, params InstapaperTag[] tags) => UpdateTags(bookmark.Id, removeIds: tags.Select(i => i.Id));

        /// <summary>
        /// Instapaper's parsed, reader-ready version of a saved article.
        /// Without an Instaparser key this only works for the authenticated user of your own application, like a personal script.
        /// </summary>
        /// <param name="useCache">False bypasses the parser cache</param>
        /// <param name="force">Force a re-parse rather than serving a stored copy</param>
        /// <param name="content">HTML you already have for the article, parsed instead of fetching the URL</param>
        /// <param name="instaparserApiKey">Your Instaparser key, required for non-personal use</param>
        public Task<InstapaperParsedArticle> GetParsedArticle(long bookmarkId, bool useCache = true, bool force = false, string content = null, string instaparserApiKey = null)
        {
            var path = $"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/parse";
            if (content != null)
            {
                return MakeRequest<InstapaperParsedArticle>(path, Call.POST, content: new InstapaperParseRequest
                {
                    UseCache = useCache,
                    Force = force,
                    Content = content,
                    InstaparserApiKey = instaparserApiKey,
                });
            }
            return MakeRequest<InstapaperParsedArticle>(path, parameters: new List<Param>
            {
                new OParam("use_cache", useCache ? null : "0"),
                new OParam("force", force ? "1" : null),
                new OParam("instaparser_api_key", instaparserApiKey),
            });
        }

        public Task<InstapaperParsedArticle> GetParsedArticle(InstapaperBookmark bookmark) => GetParsedArticle(bookmark.Id);

        /// <summary>
        /// The API ignores folder_id and tag whenever section is sent, so a mismatched
        /// section would quietly return the wrong list.
        /// </summary>
        private static void CheckSection(InstapaperBookmarkSection? section, long? folderId, string tag)
        {
            if (folderId != null && tag != null)
            {
                throw new ArgumentException("folderId and tag cannot be used together");
            }
            if (folderId != null && section != null && section != InstapaperBookmarkSection.Folder)
            {
                throw new ArgumentException($"folderId can't be used with section {section}");
            }
            if (tag != null && section != null && section != InstapaperBookmarkSection.Tag)
            {
                throw new ArgumentException($"tag can't be used with section {section}");
            }
        }

        private static void CheckPageSize(int value, string name)
        {
            if (value < 1 || value > MaxPageSize)
            {
                throw new ArgumentOutOfRangeException(name, value, $"{name} must be from 1 to {MaxPageSize}");
            }
        }

        private static long ToTimestamp(DateTimeOffset since)
        {
            var value = since.ToUnixTimeSeconds();
            // The API treats 0 as "no timestamp" and returns a normal list instead.
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(since), since, "since must be after 1970-01-01");
            }
            return value;
        }
    }
}
