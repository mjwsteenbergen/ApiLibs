using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiLibs.Wunderlist
{
    public class WunderlistService : Service
    {
        private string WunderlistToken;
        private readonly string WunderlistId;
        private readonly string WunderlistSecret;

        /// <summary>
        /// Use this constructor if you don't have a wunderlist token
        /// </summary>
        /// <param name="wunderlistId"></param>
        /// <param name="wunderlistSecret"></param>
        /// <param name="NoToken">Ignore this. Is so that we have 2 constructors</param>
        public WunderlistService(string wunderlistId, string wunderlistSecret, bool NoToken)
        {
            WunderlistId = wunderlistId;
            WunderlistSecret = wunderlistSecret;
        }

        /// <summary>
        /// Use this constructor if you already have a wunderlist token
        /// </summary>
        /// <param name="wunderlistId"></param>
        /// <param name="wunderlistToken"></param>
        public WunderlistService(string wunderlistId, string wunderlistToken)
        {
            SetBaseUrl("https://a.wunderlist.com/api/v1/");

            AddStandardHeader(new Param("X-Access-Token", WunderlistToken));
            AddStandardHeader(new Param("X-Client-ID", WunderlistId));
        }

        public async Task<string> Connect(IOAuth _authenticator)
        {
            SetUp("https://www.wunderlist.com/oauth/access_token");
            if (WunderlistToken == null)
            {

                string url = "https://www.wunderlist.com/oauth/authorize?client_id=" + WunderlistId + "&redirect_uri=" + "https://developer.wunderlist.com" + "&state=PleasCopyThis";
                string res = _authenticator.ActivateOAuth(new Uri(url), "https://developer.wunderlist.com");
                string token = Regex.Match(res, "code=(.+)").Groups[1].Value;

                Auth auth = await MakeRequest<Auth>("", Call.POST, new List<Param>
                {
                    new Param("client_id", WunderlistId),
                    new Param("code", token),
                    new Param("client_secret", WunderlistSecret)
                });

                WunderlistToken = auth.access_token;
            }

            SetBaseUrl("https://a.wunderlist.com/api/v1/");

            AddStandardHeader(new Param("X-Access-Token", WunderlistToken));
            AddStandardHeader(new Param("X-Client-ID", WunderlistId));

            return WunderlistToken;
        }

        public async Task<List<WList>> GetLists()
        {
            return await MakeRequest<List<WList>>("lists");
        }

        public async Task<List<WTask>> GetTasks(int id)
        {
            return await MakeRequest<List<WTask>>("tasks", parameters: new List <Param> {new Param("list_id", id.ToString())});
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

            throw new KeyNotFoundException("The PocketList with name:" + listName + " could not be found.");
        }

        public async Task<WTask> AddTask(WRequestTask wTask)
        {
            if (wTask?.title == null)
            {
                throw new ArgumentException("wTask.title was null");
            }

            return await MakeRequest<WTask>("tasks", Call.POST, content: wTask);
        }

        public async Task RemoveTask(WTask it)
        {
            if (it?.title == null)
            {
                throw new ArgumentException("it.title was null");
            }

            try
            {
                await HandleRequest("tasks/" + it.id, Call.DELETE, new List<Param>
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
            return await MakeRequest<List<WTask>>("tasks", parameters: new List<Param>
            {
                new Param("completed", "true"),
                new Param("list_id", (await GetList(listName)).id.ToString())
            });
        }
    }
}
