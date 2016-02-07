using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiLibs.Wunderlist
{
    public class WunderlistService : Service
    {
        private readonly IOAuth _authenticator;

        public WunderlistService(IOAuth authenticator)
        {
            _authenticator = authenticator;
            SetUp("https://www.wunderlist.com/oauth/access_token");
        }

        public async Task Connect()
        {
            if (Passwords.WunderlistToken == null)
            {

                string url = "https://www.wunderlist.com/oauth/authorize?client_id=" + Passwords.WunderlistId + "&redirect_uri=" + "https://developer.wunderlist.com" + "&state=PleasCopyThis";
                string res = _authenticator.ActivateOAuth(new Uri(url), "https://developer.wunderlist.com");
                string token = Regex.Match(res, "code=(.+)").Groups[1].Value;

                Auth auth = await MakeRequest<Auth>("", Call.POST, new List<Param>
                {
                    new Param("client_id", Passwords.WunderlistId),
                    new Param("code", token),
                    new Param("client_secret", Passwords.WunderlistSecret)
                });

                Passwords.AddPassword("WunderlistToken", auth.access_token);
                Passwords.WritePasswords();
            }

            SetBaseUrl("https://a.wunderlist.com/api/v1/");

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

            return await MakeRequest<WTask>("tasks", Call.POST, wTask);
        }

        public async Task RemoveTask(WTask it)
        {
            if (it?.title == null)
            {
                throw new ArgumentException("it.title was null");
            }

            try
            {
                await MakeRequest("tasks/" + it.id, Call.DELETE, new List<Param>
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
