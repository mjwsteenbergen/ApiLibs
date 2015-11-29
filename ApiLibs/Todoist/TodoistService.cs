using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;


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
            // Todoist does not need to connect.
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
            SyncObject syncobject = await MakeRequest<SyncObject>("sync", parameters);

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
            List<Param> parameters = new List<Param>
            {
                new Param("queries", s)
            };

            List<RootObject> obj = await MakeRequest<List<RootObject>>("query", parameters);
            
            return obj[0] ?? new RootObject();
        }

        public void SortSyncObject(SyncObject sync)
        {
            foreach (Item it in sync.Items)
            {
                sync.getProjectById(it.project_id).AddItem(it);
                foreach(int lb in it.labels)
                {
                    it.labelList.Add(sync.GetLabelbyId(lb));
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
                "[{\"type\": \"item_close\", \"uuid\": \"" + (new Random()).Next(0,10000) + "\", \"args\": {\"id\": " + todo.id + "}}]"));

                //new Param("commands",@"[{""type"": ""item_complete"", ""uuid"": """ + DateTime.Now + @""", ""args"": {""project_id"": " + todo.project_id + @", ""ids"": [" + todo.id + "]}}]"));

            await MakeRequest("sync", parameters);
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
