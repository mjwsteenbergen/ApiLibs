using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;

namespace ApiLibs.Wunderlist
{
    public class WunderlistService : Service
    {
        /// <summary>
        /// Use this constructor if you don't have a wunderlist token
        /// </summary>
        public WunderlistService() : base("https://a.wunderlist.com/api/v1/") { }

        /// <summary>
        /// Use this constructor if you already have a wunderlist token
        /// </summary>
        /// <param name="wunderlistId"></param>
        /// <param name="wunderlistToken"></param>
        public WunderlistService(string wunderlistId, string wunderlistToken) : base("https://a.wunderlist.com/api/v1/")
        {
            AddStandardHeader(new Param("X-Access-Token", wunderlistToken));
            AddStandardHeader(new Param("X-Client-ID", wunderlistId));
        }

        public void Connect(IOAuth authenticator, string WunderlistId)
        {
            string url = "https://www.wunderlist.com/oauth/authorize?client_id=" + WunderlistId + "&redirect_uri=" + authenticator.RedirectUrl + "&state=PleasCopyThis";
            authenticator.ActivateOAuth(new Uri(url));
        }

        public async Task<string> ConvertToToken(string token, string wunderlistId, string wunderlistSecret)
        {
            SetBaseUrl("https://www.wunderlist.com/oauth/access_token");

            Auth auth = await MakeRequest<Auth>("", Call.POST, new List<Param>
                {
                    new Param("client_id", wunderlistId),
                    new Param("code", token),
                    new Param("client_secret", wunderlistSecret)
                });

            string wunderlistToken = auth.access_token;

            AddStandardHeader(new Param("X-Access-Token", wunderlistToken));
            AddStandardHeader(new Param("X-Client-ID", wunderlistId));
            Client = new RestClient();
            SetBaseUrl("https://a.wunderlist.com/api/v1/");

            return wunderlistToken;
        }

        public async Task<List<WList>> GetLists()
        {
            return await MakeRequest<List<WList>>("lists");
        }

        public async Task<List<WTask>> GetTasks(WList list, bool completed = false)
        {
            return await GetTasks(list, completed);
        }

        public async Task<List<WTask>> GetTasks(long id, bool completed = false)
        {
            return await MakeRequest<List<WTask>>("tasks", parameters: new List <Param> {new Param("list_id", id.ToString()), new Param("completed", completed)});
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

            throw new KeyNotFoundException("The Wunder-List with name:" + listName + " could not be found.");
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

        public async Task Update(WTask task)
        {
            await MakeRequest<WTask>("tasks/" + task.id, Call.PATCH, content: task.ToPatchTask());
        }
    }
}
