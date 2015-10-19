using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApiLibs;
using Newtonsoft.Json;
using RestSharp;

//using Newtonsoft.Json;
//using RestSharp;

namespace ApiLibs.Todoist
{
    public class TodoistService : Service
    {
        private List<Note> cachedNotes;

        public List<Project> cachedProjects { get; private set; }

        public List<Item> cachedItems { get; private set; }
        public List<Label> cachedLabels { get; private set; }

        public TodoistService()
        {
            SetUp("https://todoist.com/API/v6/");
            AddStandardParameter(new Param("user-agent", Passwords.TodoistUserAgent));
            AddStandardParameter(new Param("token", Passwords.TodoistKey));
            AddStandardParameter(new Param("seq_no", "0"));
            AddStandardParameter(new Param("seq_no_global", "0"));

        }


        public void Connect()
        {
            
        }

        public async Task<List<Project>> GetProjects(bool cached)
        {

            if (cached && cachedProjects != null)
            {
                return cachedProjects;
            }
            else
            {
                await SyncAllItems();
                return cachedProjects;
            }
        }

        public async Task<List<Item>> GetItems(bool cached)
        {
            if(cached && cachedItems != null)
            {
                return cachedItems;
            }
            else
            {
                await SyncAllItems();
                return cachedItems;
            }
        }

        public async Task<List<Label>> GetLabels(bool cached)
        {
            if (cached && cachedLabels != null)
            {
                return cachedLabels;
            }
            else
            {
                await SyncAllItems();
                return cachedLabels;
            }
        }

        public async Task SyncAllItems()
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("resource_types", @"[""all""]"));
            IRestResponse resp = await MakeRequest("sync", parameters);

            if (resp.StatusCode.ToString() != "OK")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }

                throw new Exception("\n" + "A problem occured in TodoistService.GetProjects() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);
            }
            else
            {

                Console.WriteLine("Completed");

                SyncObject syncobject = JsonConvert.DeserializeObject<SyncObject>(resp.Content);

                SortSyncObject(syncobject);

                cachedItems = syncobject.Items;
                cachedProjects = syncobject.Projects;
                cachedLabels = syncobject.Labels;
                cachedNotes = syncobject.Notes;

            }
        }

        public async Task<RootObject> Search(string s)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("queries", s));//@"[""today""]"

            IRestResponse resp = await MakeRequest("query", parameters);

           List<RootObject> obj = new List<RootObject>();
            obj.Add(new RootObject());
            if (resp.StatusCode.ToString() != "OK")
            {
                Console.WriteLine("\n" + "A problem occured in TodoistService.Search() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);

                foreach (Parameter p in resp.Headers)
                {
                    //Console.WriteLine(p.ToString());
                }
            }
            else
            {
                try
                {
                    JsonSerializerSettings t = new JsonSerializerSettings();
                    t.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                    obj = JsonConvert.DeserializeObject<List<RootObject>>(resp.Content, t);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            
            return obj[0];
        }

        private void UpdateStandardParameters(IList<Parameter> list)
        {
            foreach (Parameter p in list)
            {
                if (p.Name == "seq_no" || p.Name == "seq_no_global")
                {
                    //UpdateParameterIfExists(p);
                }
            }
        }

        public void SortSyncObject(SyncObject sync)
        {
            foreach (Item it in sync.Items)
            {
                sync.getProjectById(it.project_id).AddItem(it);
                foreach(int lb in it.labels)
                {
                    it.labelList.Add(sync.getLabelbyId(lb));
                }
                
            }

            sync.SortProjects();

            foreach (Project proj in sync.Projects)
            {
                proj.OrderItems();
            }
        }

        public async Task MarkTodoAsDone(Item todo)
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("commands",
                @"[{""type"": ""item_complete"", ""uuid"": """ + DateTime.Now + @""", ""args"": {""project_id"": " + todo.project_id + @", ""ids"": [" + todo.id + "]}}]"));
            IRestResponse resp = await MakeRequest("sync", parameters);

            if (resp.StatusCode.ToString() != "OK")
            {

                Console.WriteLine("\n" + "A problem occured in TodoistService.Sync() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);

                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }
            }
            else
            {

            }
        }

        public async void addTodo(string name)
        {
            

            //string uuid = @"""uuid"": """ + DateTime.Now + @"""";
            //string tempid = @"""tempid"": """ + DateTime.Now + @"""";
            //string args = "\"args\": {\"content\": \"" + name + "\", \"project_id\"+";
            string item_add = name;


            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("content", item_add));
            IRestResponse resp = await MakeRequest("add_item", parameters);

            if (resp.StatusCode.ToString() != "OK")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }

                throw new Exception("\n" + "A problem occured in TodoistService.GetProjects() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);
            }

            Console.WriteLine("Completed");
        }
    }
}
