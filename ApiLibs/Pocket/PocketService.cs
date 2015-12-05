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
        public PocketService()
        {
            SetUp("https://getpocket.com/v3/");
            Passwords.ReadPasswords();
            AddStandardParameter(new Param("access_token", Passwords.Pocket_access_token));
            AddStandardParameter(new Param("consumer_key", Passwords.PocketKey));
        }

        public async Task Connect(IOAuth authenticator)
        {
            if (Passwords.Pocket_access_token != null)
            {
                return;
            }

            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("consumer_key", Passwords.PocketKey));
            parameters.Add(new Param("redirect_uri", "zeus://"));

            IRestResponse resp = await MakeRequestPost("oauth/request.php", parameters);
            
            string code = resp.Content.Replace("code=", "");

            authenticator.ActivateOAuth(new Uri("https://getpocket.com/auth/authorize?request_token=" + code + "&redirect_uri=" + Passwords.GeneralRedirectUrl));

            parameters = new List<Param>
            {
                new Param("consumer_key", Passwords.PocketKey),
                new Param("code", code)
            };

            resp = await MakeRequestPost("oauth/authorize.php", parameters);

            var noAccessToken = resp.Content.Replace("access_token=", "");
            string accessToken = noAccessToken.Remove(30, resp.Content.Length - 13 - 30);

            Passwords.AddPassword("Pocket_access_token", accessToken);
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

            IRestResponse resp = await MakeRequestPost("get.php", parameters);

            var regexd = Regex.Replace(resp.Content, @"""\d+"":", "").Replace("\"list\":{", "\"list\":[").Replace("}},\"error\"", "}],\"error\"");
            return JsonConvert.DeserializeObject<ReadingList>(regexd);
        }

        public async Task Delete(List ls)
        {
            await SendAction(new PocketAction("delete", ls.item_id));

        }

        public async Task Unfavorite(List ls)
        {
            await SendAction(new PocketAction("unfavorite", ls.item_id));
        }

        private async Task SendAction(PocketAction pa)
        {
            List<Param> parameters = new List<Param>
            {
                new Param("actions", JsonConvert.SerializeObject(new List<PocketAction> {pa}))
            };
            await MakeRequest("send.php", parameters);
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
