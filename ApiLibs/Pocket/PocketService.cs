using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiLibs.Pocket
{
    public class PocketService : Service
    {
        private string Pocket_access_token;
        private string PocketKey;
        private string GeneralRedirectUrl;


        public PocketService(Passwords pass)
        {
            Pocket_access_token = pass.Pocket_access_token;
            PocketKey = pass.PocketKey;
            GeneralRedirectUrl = pass.GeneralRedirectUrl;
        }

        public async Task Connect(IOAuth authenticator, Passwords pass)
        {
            pass.AddPassword("Pocket_access_token", await Connect(authenticator));
        }

        public async Task<string> Connect(IOAuth authenticator)
        {
            SetUp("https://getpocket.com/v3/");
            if (Pocket_access_token != null)
            {
                AddStandardParameter(new Param("access_token", Pocket_access_token));
                AddStandardParameter(new Param("consumer_key", PocketKey));
                return Pocket_access_token;
            }

            List<Param> parameters = new List<Param>
            {
                new Param("consumer_key", PocketKey),
                new Param("redirect_uri", "zeus://")
            };

            IRestResponse resp = await MakeRequest("oauth/request.php", Call.POST, parameters);
            
            string code = resp.Content.Replace("code=", "");

            authenticator.ActivateOAuth(new Uri("https://getpocket.com/auth/authorize?request_token=" + code + "&redirect_uri=" + GeneralRedirectUrl));

            parameters = new List<Param>
            {
                new Param("consumer_key", PocketKey),
                new Param("code", code)
            };

            resp = await MakeRequest("oauth/authorize.php", Call.POST, parameters);

            var noAccessToken = resp.Content.Replace("access_token=", "");
            string accessToken = noAccessToken.Remove(30, resp.Content.Length - 13 - 30);

            return accessToken;
        }

        public async Task<Item> AddArticle(string url)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("url", url));

            return (await MakeRequest<AddItemResponse>("add.php", parameters)).item;
        }

        public async Task<ReadingList> GetReadingList(bool simple)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("detailType", simple ? "simple" : "complete"));

            IRestResponse resp = await MakeRequest("get.php", Call.POST, parameters);

            var regexd = Regex.Replace(Regex.Replace(resp.Content, @"""\d+"":", "").Replace("\"list\":{", "\"list\":[").Replace("}},\"error\"", "}],\"error\""), "{{(([^{}]|{[^{}]+}|)+)}(([^{}]|{[^{}]+}|)+)}", "[{$1}$3]");
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
                new Param("actions", JsonConvert.SerializeObject(new List<PocketAction> {pa}))
            };
            await MakeRequest("send.php", Call.GET, parameters);
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
    }
}
