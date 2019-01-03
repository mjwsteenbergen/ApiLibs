using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiLibs.Pocket
{
    public class PocketService : Service
    {
        private string PocketKey;

        /// <summary>
        /// Use this method if you do not have an access token yet.
        /// Call Connect() to continue
        /// </summary>
        /// <param name="pocketKey"></param>
        public PocketService(string pocketKey) : base("https://getpocket.com/v3/")
        {
            PocketKey = pocketKey;
        }

        /// <summary>
        /// Call this constructor if you already have an access token
        /// </summary>
        /// <param name="pocket_access_token"></param>
        /// <param name="pocketKey"></param>
        public PocketService(string pocket_access_token, string pocketKey) : base("https://getpocket.com/v3/")
        {
            AddStandardParameter(new Param("access_token", pocket_access_token));
            AddStandardParameter(new Param("consumer_key", pocketKey));
        }

        public async Task<string> Connect(IOAuth authenticator, string generalRedirectUrl)
        {
            List<Param> parameters = new List<Param>
            {
                new Param("consumer_key", PocketKey),
                new Param("redirect_uri", "zeus://")
            };

            string content = await HandleRequest("oauth/request.php", Call.POST, parameters: parameters);
            
            string code = content.Replace("code=", "");

            authenticator.ActivateOAuth(new Uri("https://getpocket.com/auth/authorize?request_token=" + code + "&redirect_uri=" + generalRedirectUrl));
            return code;
        }

        public async Task<string> ConvertToToken(string code)
        {
            var parameters = new List<Param>
            {
                new Param("consumer_key", PocketKey),
                new Param("code", code)
            };

            var content = await HandleRequest("oauth/authorize.php", Call.POST, parameters: parameters);

            var noAccessToken = content.Replace("access_token=", "");
            string accessToken = noAccessToken.Remove(30, content.Length - 13 - 30);
            return accessToken;
        }

        public async Task<Item> AddArticle(string url)
        {
            List<Param> parameters = new List<Param> {new Param("url", url)};

            return (await MakeRequest<AddItemResponse>("add.php", parameters: parameters)).item;
        }

        public async Task<ReadingList> GetReadingList(ListState state = ListState.Unread, bool onlyFavorites = false, string tagname = "", ContentType content = ContentType.All, SortOn sort = SortOn.Newest, DetailType detail = DetailType.Simple)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("state",state.ToString().ToLower()));
            parameters.Add(new Param("favorite", onlyFavorites ? "1" : "0"));
            if (tagname != "")
            {
                parameters.Add(new Param("tag", tagname));
            }
            if (content != ContentType.All)
            {
                parameters.Add(new Param("contentType", content.ToString().ToLower()));
            }
            parameters.Add(new Param("sort", sort.ToString().ToLower()));
            parameters.Add(new Param("detailType", detail.ToString().ToLower()));


            var resContent = await HandleRequest("get.php", Call.POST, parameters);

            var regexd = Regex.Replace(Regex.Replace(resContent, @"""\d+"":", "").Replace("\"list\":{", "\"list\":[").Replace("}},\"error\"", "}],\"error\""), "{{(([^{}]|{[^{}]+}|)+)}(([^{}]|{[^{}]+}|)+)}", "[{$1}$3]");
            return JsonConvert.DeserializeObject<ReadingList>(regexd);
        }

        public async Task Delete(PocketList ls)
        {
            await SendAction(new PocketAction("delete", ls.item_id));

        }

        public async Task Unfavorite(PocketList ls)
        {
            await SendAction(new PocketAction("unfavorite", ls.item_id));
        }

        private async Task SendAction(PocketAction pa)
        {
            List<Param> parameters = new List<Param>
            {
                new Param("actions", new List<PocketAction> {pa})
            };
            await HandleRequest("send.php", parameters: parameters);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private class PocketAction
        {
            public string action;
            public string item_id;
            public string time;

            public PocketAction(string category, string itemId)
            {
                this.action = category;
                this.item_id = itemId;
                this.time = DateTime.Now.ToFileTime().ToString();
            }
        }

        public enum ListState
        {
            
            All, Unread, Archive
        }

        public enum DetailType
        {
            Simple, Complete
        }

        public enum ContentType
        {
            Article, Video, Image, All
        }

        public enum SortOn
        {
            Newest, Oldest, Title, Site
        }
    }
}
