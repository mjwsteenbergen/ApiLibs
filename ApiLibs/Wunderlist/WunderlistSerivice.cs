using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;

namespace ApiLibs.Wunderlist
{
    public class WunderlistService : Service
    {
        private readonly IOAuth _authenticator;

        public WunderlistService(IOAuth authenticator)
        {
            _authenticator = authenticator;
            SetUp("https://www.wunderlist.com/oauth/access_token");
            //id 8bbe75c06b0ca9d71160
            //client secret 78ef23e935e8368379d8cb2b6721f95e45eda28c7f93beae837c3e624638
            //access token  f39b49a71e889f2d6a22a0d0683045f38b7d22350fe1b860cd5a27d0e116
        }

        public async Task Connect()
        {
            if (Passwords.WunderlistToken == null)
            {

                string url = "https://www.wunderlist.com/oauth/authorize?client_id=" + Passwords.WunderlistId + "&redirect_uri=" + "https://developer.wunderlist.com" + "&state=PleasCopyThis";
                string res = _authenticator.ActivateOAuth(new Uri(url), "https://developer.wunderlist.com");
                string token = Regex.Match(res, "code=(.+)").Groups[1].Value;

                Auth auth = await MakeRequestPost<Auth>("", new List<Param>
                {
                    new Param("client_id", Passwords.WunderlistId),
                    new Param("code", token),
                    new Param("client_secret", Passwords.WunderlistSecret)
                });

                Passwords.addPassword("WunderlistToken", auth.access_token);
                Passwords.writePasswords();
            }

            setBaseUrl("https://a.wunderlist.com/api/v1/");

            AddStandardHeader(new Param("X-Access-Token", Passwords.WunderlistToken));
            AddStandardHeader(new Param("X-Client-ID", Passwords.WunderlistId));
        }

        public async Task<List<WList>> GetLists()
        {
            return await MakeRequest<List<WList>>("lists", new List<Param>());
        }

        public async Task<List<WTask>> GetTasks(int id)
        {
            return await MakeRequest<List<WTask>>("tasks", new List<Param> {new Param("list_id", id.ToString())});
        }

        public async Task<WList> GetList(string listName)
        {
            foreach (WList list in await GetLists())
            {
                if (list.title == listName)
                {
                    return list;
                }
            }

            throw new KeyNotFoundException("The list with name:" + listName + " could not be found.");
        }

        public async Task<WTask> AddTask(WRequestTask wTask)
        {
            if (wTask?.title == null)
            {
                throw new ArgumentException("wTask.title was null");
            }

            return await MakeRequest<WTask>(Method.POST, "tasks", wTask);
        }

        public async Task RemoveTask(WTask it)
        {
            if (it?.title == null)
            {
                throw new ArgumentException("it.title was null");
            }

            try
            {
                await MakeRequest(Method.DELETE, "tasks/" + it.id, new List<Param>
                {
                    new Param("revision", it.revision.ToString())
                });
            }
            catch
            {
                //This just fails.
            }
        }

        public async Task<IEnumerable<WTask>> GetCompleted(string listName)
        {
            return await MakeRequest<List<WTask>>("tasks", new List<Param>
            {
                new Param("completed", "true"),
                new Param("list_id", (await GetList(listName)).id.ToString())
            });
        }
    }
}
