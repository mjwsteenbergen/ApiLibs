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

            await authenticator.ActivateOAuth(new Uri("https://getpocket.com/auth/authorize?request_token=" + code + "&redirect_uri=" + Passwords.GeneralRedirectUrl));

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

        public async void addArticle(string url)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("url", url));

            IRestResponse resp = await MakeRequestPost("add.php", parameters);

            if (resp.StatusCode.ToString() != "OK")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }

                throw new Exception("\n" + "A problem occured in PocketService.Connect() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);
            }
            else
            {
                //so2 = JsonConvert.DeserializeObject<SyncObject>(resp.Content);
                //UpdateParameterIfExists(new Param("seq_no", so2.seq_no.ToString()));
                //UpdateParameterIfExists(new Param("seq_no_global", so2.seq_no_global.ToString()));

                Console.WriteLine("Completed" + resp.Content);

            }

            //cachedItems = so2.Items;
            //return so2.Items;get
        }

        public async void getReadingList(bool simple)
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
            Rootobject pocket = JsonConvert.DeserializeObject<Rootobject>(regexd);
        }
    }
}
