using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
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
            Passwords.readPasswords();
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

            if (resp.StatusCode.ToString() != "OK")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }

                throw new Exception("\n" + "A problem occured in PocketService.Connect() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);
            }

            //so2 = JsonConvert.DeserializeObject<SyncObject>(resp.Content);
            //UpdateParameterIfExists(new Param("seq_no", so2.seq_no.ToString()));
            //UpdateParameterIfExists(new Param("seq_no_global", so2.seq_no_global.ToString()));
            string code = resp.Content.ToString().Replace("code=", "");

            authenticator.ActivateOAuth(new Uri("https://getpocket.com/auth/authorize?request_token=" + code + "&redirect_uri=" + Passwords.GeneralRedirectUrl));

            parameters = new List<Param>();
            parameters.Add(new Param("consumer_key", Passwords.PocketKey));
            parameters.Add(new Param("code", code));

            resp = await MakeRequestPost("oauth/authorize.php", parameters);

            if (resp.StatusCode.ToString() != "OK")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }

                throw new Exception("\n" + "A problem occured in PocketService.Connect() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);
            }

            var noAccessToken = resp.Content.Replace("access_token=", "");
            string accessToken = noAccessToken.Remove(30, resp.Content.Length - 13 - 30);

            Passwords.addPassword("Pocket_access_token", accessToken);
            Passwords.writePasswords();
        }

        public async Task<Item> addArticle(string url)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("url", url));

            return (await MakeRequest<AddItemResponse>("add.php", parameters)).item;
        }

        public async Task<ReadingList> getReadingList(bool simple)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("detailType", simple ? "simple" : "complete"));

            IRestResponse resp = await MakeRequestPost("get.php", parameters);

            if (resp.StatusCode.ToString() != "OK")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }

                throw new Exception("\n" + "A problem occured in PocketService.Connect() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);
            }

            var regexd = Regex.Replace(resp.Content, @"""\d+"":", "").Replace("\"list\":{", "\"list\":[").Replace("}},\"error\"", "}],\"error\"");
            return JsonConvert.DeserializeObject<ReadingList>(regexd);
        }

        public void delete(List ls)
        {
            sendAction(new PocketAction("delete", ls.item_id));

        }

        public void unfavorite(List ls)
        {
            sendAction(new PocketAction("unfavorite", ls.item_id));
        }

        private async void sendAction(PocketAction pa)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("actions", JsonConvert.SerializeObject(new List<PocketAction>() { pa })));
            await MakeRequest("send.php", parameters);
        }

        private class PocketAction
        {
            public string action;
            public string item_id;
            public string time;

            public PocketAction(string category, string item_id)
            {
                this.action = category;
                this.item_id = item_id;
                this.time = DateTime.Now.ToFileTime().ToString();
            }
        }
    }
}
