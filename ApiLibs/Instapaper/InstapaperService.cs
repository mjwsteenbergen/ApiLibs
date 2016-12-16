using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;
using RestSharp.Authenticators.OAuth.Extensions;

namespace ApiLibs.Instapaper
{
    public class InstapaperService : Service
    {
        private string AcessToken { get; set; }

        public InstapaperService()
        {
            SetUp("https://www.instapaper.com/api/1/");
        }

        public InstapaperService(string clientId, string clientSecret, string token, string tokenSecret)
        {
            SetUp("https://www.instapaper.com/api/1/");
            Client.Authenticator = OAuth1Authenticator.ForAccessToken(clientId, clientSecret, token, tokenSecret);
        }

        public void Connect(string username, string password, string clientId, string clientSecret)
        {
            var client = new RestClient("https://www.instapaper.com/api/1/")
            {
                Authenticator = OAuth1Authenticator.ForRequestToken(clientId, clientSecret)
            };
            var request = new RestRequest("/oauth/access_token", Method.POST);
            request.Parameters.Add(new Parameter { Name = "x_auth_mode", Type = ParameterType.GetOrPost, Value = "client_auth" });
            request.Parameters.Add(new Parameter { Name = "x_auth_username", Type = ParameterType.GetOrPost, Value = username });
            request.Parameters.Add(new Parameter { Name = "x_auth_password", Type = ParameterType.GetOrPost, Value = password });
            var response = client.Execute(request);
            string[] respParameters = response.Content.Split('&');
            string tokenSecret = respParameters[0].Replace("oauth_token_secret=", "");
            string token = respParameters[1].Replace("oauth_token=", "");

            client.Authenticator = OAuth1Authenticator.ForAccessToken(clientId, clientSecret, token, tokenSecret);
        }

        public async Task<List<Bookmark>> GetBookmarks(int limit = 25, int folderId = -1)
        {
            List<Param> param = new List<Param>();
            if (limit != 25)
            {
                param.Add(new Param("limit", limit.ToString()));
            }
            if (folderId != -1)
            {
                param.Add(new Param("folder_id", folderId.ToString()));
            }
            List<Bookmark> bookmarks = await MakeRequest<List<Bookmark>>("bookmarks/list", parameters: param);
            bookmarks.RemoveRange(0,2);
            return bookmarks;
        }

        public async Task<List<Bookmark>> GetBookmarks(int limit, Folder folder)
        {
            return await GetBookmarks(limit, folder.folder_id);
        }

        public async Task<Bookmark> AddBookmark(string url, string title = "", string description = "", int folderId = -1,
            string finalUrl = "")
        {
            List<Param> param = new List<Param> { new Param("url", url)};
            if (title != "")
            {
                param.Add(new Param("title", title));
            }

            if (description != "")
            {
                param.Add(new Param("description", description));
            }

            if (folderId != -1)
            {
                param.Add(new Param("folder_id", folderId.ToString()));
            }

            if (finalUrl != "")
            {
                param.Add(new Param("final_url", finalUrl));
            }

            return (await MakeRequest<List<Bookmark>>("bookmarks/add", parameters: param))[0];
        }

        public async Task DeleteBookmark(int bookmarkId)
        {
            await HandleRequest("bookmarks/delete", parameters: new List<Param> {new Param("bookmark_id", bookmarkId.ToString())});
        }

        public async Task DeleteBookmark(Bookmark bm)
        {
            await DeleteBookmark(bm.bookmark_id);
        }

        public async void StarBookmark(int bookmarkId)
        {
            await HandleRequest("bookmarks/star", parameters: new List<Param> { new Param("bookmark_id", bookmarkId.ToString()) });
        }

        public async void UnstarBookmark(int bookmarkId)
        {
            await HandleRequest("bookmarks/unstar", parameters: new List<Param> { new Param("bookmark_id", bookmarkId.ToString()) });
        }

        public async void ArchiveBookmark(int bookmarkId)
        {
            await HandleRequest("bookmarks/archive", parameters: new List<Param> { new Param("bookmark_id", bookmarkId.ToString()) });
        }

        public async void UnarchiveBookmark(int bookmarkId)
        {
            await HandleRequest("bookmarks/unarchive", parameters: new List<Param> { new Param("bookmark_id", bookmarkId.ToString()) });
        }

        public async void MoveBookmark(int bookmarkId, int folderId)
        {
            await HandleRequest("bookmars/unarchive", parameters: new List<Param>
            {
                new Param("bookmark_id", bookmarkId.ToString()),
                new Param("folder_id", folderId.ToString())
            });
        }

        public async Task<List<Folder>> GetFolders()
        {
            return await MakeRequest<List<Folder>>("folders/list");
        }

        public async Task AddFolder(string title)
        {
            await HandleRequest("folders/add", parameters: new List<Param> {new Param("title", title)});
        }

        public async Task DeleteFolder(int folderId)
        {
            await HandleRequest("folders/delete", parameters: new List<Param> {new Param("folder_id", folderId.ToString())});
        }

        public async Task<Folder> GetFolder(string foldername)
        {
            List<Folder> folders = await GetFolders();
            foreach (var folder in folders)
            {
                if (folder.title == foldername)
                {
                    return folder;
                }
            }
            throw new KeyNotFoundException("Your folder could not be found");
        }
    }

}
