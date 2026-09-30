using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Instapaper.V2
{
    /// <summary>
    /// Client for the Instapaper API v2.
    /// <see href="https://www.instapaper.com/developers/overview/introduction"/>
    /// </summary>
    public class InstapaperV2Service : RestSharpService
    {
        private const string HostUrl = "https://www.instapaper.com";
        private readonly string clientId;
        private readonly string clientSecret;

        public InstapaperV2BookmarkService Bookmarks { get; }
        public InstapaperV2FolderService Folders { get; }
        public InstapaperV2TagService Tags { get; }
        public InstapaperV2HighlightService Highlights { get; }

        /// <summary>
        /// Use this with a personal access token, or one issued through the OAuth flow.
        /// Personal access tokens are generated on your application's page at https://www.instapaper.com/developers/applications
        /// </summary>
        public InstapaperV2Service(string accessToken) : this()
        {
            SetAccessToken(accessToken);
        }

        /// <summary>
        /// Use this to authorize other users through the OAuth 2 authorization code flow.
        /// Call <see cref="Connect"/> and then <see cref="ExchangeCode"/>.
        /// </summary>
        /// <param name="clientId">The client id from your application's page</param>
        /// <param name="clientSecret">The client secret from your application's page</param>
        public InstapaperV2Service(string clientId, string clientSecret) : this()
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        private InstapaperV2Service() : base(HostUrl + "/api/2/")
        {
            Bookmarks = new InstapaperV2BookmarkService(this);
            Folders = new InstapaperV2FolderService(this);
            Tags = new InstapaperV2TagService(this);
            Highlights = new InstapaperV2HighlightService(this);
        }

        private void SetAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentException("accessToken is required", nameof(accessToken));
            }
            AddStandardHeader("Authorization", $"Bearer {accessToken}");
        }

        /// <summary>
        /// The URL to send the user to so they can authorize your application.
        /// </summary>
        /// <param name="redirectUri">Must match one of the application's registered callback URIs exactly</param>
        /// <param name="state">An opaque value echoed back to your redirect URI. Use it to defend against CSRF.</param>
        public string GetAuthorizationUrl(string redirectUri, string state = null)
        {
            var url = $"{HostUrl}/oauth2/authorize?client_id={Uri.EscapeDataString(clientId)}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code";
            if (state != null)
            {
                url += $"&state={Uri.EscapeDataString(state)}";
            }
            return url;
        }

        /// <summary>
        /// Opens the authorization page for the user.
        /// </summary>
        public void Connect(IOAuth authenticator, string state = null)
        {
            authenticator.ActivateOAuth(new Uri(GetAuthorizationUrl(authenticator.RedirectUrl, state)));
        }

        /// <summary>
        /// Exchange the code from your redirect URI for an access token. Codes work once.
        /// Access tokens don't expire. This service uses the token for any further calls.
        /// </summary>
        public async Task<InstapaperAccessToken> ExchangeCode(string code, string redirectUri)
        {
            var token = await new BlandService().MakeRequest<InstapaperAccessToken>($"{HostUrl}/oauth2/token", Call.POST, new List<Param>
            {
                new Param("client_id", clientId),
                new Param("client_secret", clientSecret),
                new Param("redirect_uri", redirectUri),
                new Param("code", code),
            });
            SetAccessToken(token.AccessToken);
            return token;
        }

        /// <summary>
        /// The account the access token belongs to.
        /// </summary>
        public Task<InstapaperUser> GetMe() => Send<InstapaperUser>("me");

        internal Task<T> Send<T>(string endPoint, Call method = Call.GET, List<Param> query = null, object content = null) => MakeRequest(new Request<T>(endPoint)
        {
            Method = method,
            Parameters = query ?? new List<Param>(),
            Content = content,
            ExpectedStatusCode = SuccessCodes,
        });

        internal Task Send(string endPoint, Call method) => MakeRequest(new Request(endPoint)
        {
            Method = method,
            ExpectedStatusCode = SuccessCodes,
        });

        private static readonly HttpStatusCode[] SuccessCodes = { HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NoContent };

        internal static long CheckId(long id, string name = "id")
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(name, id, "Expected a positive ID");
            }
            return id;
        }
    }

    /// <summary>
    /// List, save, update, move, like, tag, delete, and parse bookmarks.
    /// </summary>
    public class InstapaperV2BookmarkService : SubService<InstapaperV2Service>
    {
        private const int MaxPageSize = 500;

        public InstapaperV2BookmarkService(InstapaperV2Service service) : base(service) { }

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
            return Service.Send<InstapaperBookmarkList>("bookmarks", query: new List<Param>
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
            var changes = await Service.Send<InstapaperBookmarkChanges>("bookmarks", query: new List<Param>
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
            return Service.Send<InstapaperBookmark>("bookmarks", Call.POST, content: bookmark);
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
            return Service.Send<InstapaperBookmark>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}", Call.POST, content: new InstapaperBookmarkEdit
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
        public Task DeleteBookmark(long bookmarkId) => Service.Send($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}", Call.DELETE);
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

        private Task<InstapaperBookmark> Move(long bookmarkId, string section) => Service.Send<InstapaperBookmark>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/move", Call.POST, content: new Dictionary<string, string> { { "section", section } });

        public Task<InstapaperBookmark> LikeBookmark(long bookmarkId) => Service.Send<InstapaperBookmark>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/like", Call.POST);
        public Task<InstapaperBookmark> LikeBookmark(InstapaperBookmark bookmark) => LikeBookmark(bookmark.Id);

        public Task<InstapaperBookmark> UnlikeBookmark(long bookmarkId) => Service.Send<InstapaperBookmark>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/like", Call.DELETE);
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

            return Service.Send<InstapaperTagChanges>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/tags", Call.POST, content: new InstapaperTagEdit
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
                return Service.Send<InstapaperParsedArticle>(path, Call.POST, content: new InstapaperParseRequest
                {
                    UseCache = useCache,
                    Force = force,
                    Content = content,
                    InstaparserApiKey = instaparserApiKey,
                });
            }
            return Service.Send<InstapaperParsedArticle>(path, query: new List<Param>
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

    /// <summary>
    /// List, create, reorder, and delete a user's folders.
    /// </summary>
    public class InstapaperV2FolderService : SubService<InstapaperV2Service>
    {
        public InstapaperV2FolderService(InstapaperV2Service service) : base(service) { }

        /// <summary>
        /// The user's folders, in their own order.
        /// </summary>
        public async Task<List<InstapaperFolder>> GetFolders() => (await Service.Send<InstapaperFolderList>("folders")).Folders;

        /// <summary>
        /// Finds a folder by title
        /// Library implemented
        /// </summary>
        public async Task<InstapaperFolder> GetFolder(string title) => (await GetFolders()).FirstOrDefault(i => i.Title == title) ?? throw new KeyNotFoundException("Your folder could not be found");

        public Task<InstapaperFolder> AddFolder(string title) => Service.Send<InstapaperFolder>("folders", Call.POST, content: new Dictionary<string, string> { { "title", title } });

        /// <summary>
        /// Delete a folder. Its bookmarks move back to the home list.
        /// </summary>
        public Task DeleteFolder(long folderId) => Service.Send($"folders/{InstapaperV2Service.CheckId(folderId)}", Call.DELETE);
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
            return (await Service.Send<InstapaperFolderList>("folders/reorder", Call.POST, content: new InstapaperFolderReorder { Order = order })).Folders;
        }
    }

    /// <summary>
    /// List, create, and rename a user's tags.
    /// </summary>
    public class InstapaperV2TagService : SubService<InstapaperV2Service>
    {
        public InstapaperV2TagService(InstapaperV2Service service) : base(service) { }

        public async Task<List<InstapaperTag>> GetTags() => (await Service.Send<InstapaperTagList>("tags")).Tags;

        public Task<InstapaperTag> AddTag(string name) => Service.Send<InstapaperTag>("tags", Call.POST, content: new Dictionary<string, string> { { "name", name } });

        public Task<InstapaperTag> RenameTag(long tagId, string name) => Service.Send<InstapaperTag>($"tags/{InstapaperV2Service.CheckId(tagId)}", Call.POST, content: new Dictionary<string, string> { { "name", name } });
        public Task<InstapaperTag> RenameTag(InstapaperTag tag, string name) => RenameTag(tag.Id, name);
    }

    /// <summary>
    /// Read, create, and delete highlights on a bookmark.
    /// </summary>
    public class InstapaperV2HighlightService : SubService<InstapaperV2Service>
    {
        public InstapaperV2HighlightService(InstapaperV2Service service) : base(service) { }

        public async Task<List<InstapaperHighlight>> GetHighlights(long bookmarkId) => (await Service.Send<InstapaperHighlightList>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/highlights")).Highlights;
        public Task<List<InstapaperHighlight>> GetHighlights(InstapaperBookmark bookmark) => GetHighlights(bookmark.Id);

        /// <summary>
        /// Create a highlight. Accounts without Premium can create five per month; past that the API answers with a 403.
        /// </summary>
        /// <param name="text">The highlighted text. Leading and trailing whitespace is trimmed by the API.</param>
        /// <param name="position">Which occurrence of <paramref name="text"/> in the article body to highlight, counting from 0.</param>
        public Task<InstapaperHighlight> AddHighlight(long bookmarkId, string text, string note = null, int? position = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Highlight text cannot be blank", nameof(text));
            }
            var body = new Dictionary<string, object> { { "text", text } };
            if (note != null)
            {
                body.Add("note", note);
            }
            if (position != null)
            {
                body.Add("position", position.Value);
            }
            return Service.Send<InstapaperHighlight>($"bookmarks/{InstapaperV2Service.CheckId(bookmarkId)}/highlights", Call.POST, content: body);
        }

        public Task<InstapaperHighlight> AddHighlight(InstapaperBookmark bookmark, string text, string note = null, int? position = null) => AddHighlight(bookmark.Id, text, note, position);

        /// <summary>
        /// Delete a highlight. The API answers with a 400 if the highlight doesn't exist, belongs to someone else, or was already deleted.
        /// </summary>
        public Task DeleteHighlight(long highlightId) => Service.Send($"highlights/{InstapaperV2Service.CheckId(highlightId)}", Call.DELETE);
        public Task DeleteHighlight(InstapaperHighlight highlight) => DeleteHighlight(highlight.Id);
    }
}
