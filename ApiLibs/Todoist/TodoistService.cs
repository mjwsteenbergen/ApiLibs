using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

//using Newtonsoft.Json;
//using RestSharp;

namespace ApiLibs.Todoist
{
    public class TodoistService : Service
    {
        public List<Note> CachedNotes { get; private set; }
        public List<Project> CachedProjects { get; private set; }
        public List<Item> CachedItems { get; private set; }
        public List<Label> CachedLabels { get; private set; }

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

            if (cached && CachedProjects != null)
            {
                return CachedProjects;
            }
            else
            {
                await SyncAllItems();
                return CachedProjects;
            }
        }

        public async Task<List<Item>> GetItems(bool cached)
        {
            if(cached && CachedItems != null)
            {
                return CachedItems;
            }
            await SyncAllItems();
            return CachedItems;
        }

        public async Task<List<Label>> GetLabels(bool cached)
        {
            if (cached && CachedLabels != null)
            {
                return CachedLabels;
            }
            await SyncAllItems();
            return CachedLabels;
        }

        public async Task SyncAllItems()
        {
            List<Param> parameters = new List<Param> {new Param("resource_types", @"[""all""]")};
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
            Console.WriteLine("Completed");

            SyncObject syncobject = JsonConvert.DeserializeObject<SyncObject>(resp.Content);

            SortSyncObject(syncobject);

            CachedItems = syncobject.Items;
            CachedProjects = syncobject.Projects;
            CachedLabels = syncobject.Labels;
            CachedNotes = syncobject.Notes;
        }

        public async Task<Project> GetProject(string projectName)
        {
            if (CachedProjects == null)
            {
                await SyncAllItems();
            }
            foreach (Project p in CachedProjects)
            {
                if (p.name == projectName)
                {
                    return p;
                }
            }
            throw new KeyNotFoundException(projectName + " was not found");
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
            List<Param> parameters = new List<Param>
            {
                new Param("commands",
                    @"[{""type"": ""item_complete"", ""uuid"": """ + DateTime.Now + @""", ""args"": {""project_id"": " +
                    todo.project_id + @", ""ids"": [" + todo.id + "]}}]")
            };
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

        public Task<Item> AddTodo(string name, Project project)
        {
            return AddTodo(name, project.id);
        }

        public async Task<Item> AddTodo(string name, int project)
        {
            List<Param> parameters = new List<Param> {new Param("content", name), new Param("project_id", project.ToString())};
            return await MakeRequest<Item>("add_item", parameters);
        }

        public async Task<Item> AddTodo(string name)
        {
            List<Param> parameters = new List<Param> { new Param("content", name) };
            return await MakeRequest<Item>("add_item", parameters);
        }


    }
}
