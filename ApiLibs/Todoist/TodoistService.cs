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

        private string TodoistUserAgent;
        private string TodoistKey;

        /// <summary>
        /// Get an access token by going to https://todoist.com/Users/viewPrefs?page=account
        /// </summary>
        /// <param name="todoistKey"></param>
        /// <param name="todoistUserAgent"></param>
        public TodoistService(string todoistKey, string todoistUserAgent)
        {
            TodoistKey = todoistKey;
            TodoistUserAgent = todoistUserAgent;
            SetUp("https://todoist.com/API/v6/");
            AddStandardParameter(new Param("user-agent", TodoistUserAgent));
            AddStandardParameter(new Param("token", TodoistKey));
            AddStandardParameter(new Param("seq_no", "0"));
            AddStandardParameter(new Param("seq_no_global", "0"));
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
            if (cached && CachedItems != null)
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

        public async Task<Label> GetLabel(string name)
        {
            foreach (Label label in await GetLabels(false))
            {
                if (label.name == name)
                {
                    return label;
                }
            }
            throw new KeyNotFoundException("Label: " + name + " was not found. Try something else");
        }

        public async Task SyncAllItems()
        {
            List<Param> parameters = new List<Param> {new Param("resource_types", @"[""all""]")};
            SyncObject syncobject = await MakeRequest<SyncObject>("sync", parameters: parameters);

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

            List<RootObject> obj = await MakeRequest<List<RootObject>>("query", parameters: parameters);

            return obj[0] ?? new RootObject();
        }

        public void SortSyncObject(SyncObject sync)
        {
            foreach (Item it in sync.Items)
            {
                sync.getProjectById(it.project_id).AddItem(it);
                foreach (int lb in it.labels)
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
                "[{\"type\": \"item_close\", \"uuid\": \"" + (new Random()).Next(0, 10000) + "\", \"args\": {\"id\": " +
                todo.id + "}}]"));

            //new Param("commands",@"[{""type"": ""item_complete"", ""uuid"": """ + DateTime.Now + @""", ""args"": {""project_id"": " + todo.project_id + @", ""ids"": [" + todo.id + "]}}]"));

            await HandleRequest("sync", Call.GET, parameters);
        }

        public async Task<Item> AddTodo(string name, Project project = null, List<Label> labels = null, string date =null)
        {
            int id = project?.id ?? -1;
            return await AddTodo(name, id, labels, date);
        }

        public async Task<Item> AddTodo(string name, int id, List<Label> labels = null, string date = null)
        {
            List<Param> parameters = new List<Param>
            {
                new Param("content", name),
            };
            if (id != -1)
            {
                parameters.Add(new Param("project_id", id.ToString()));
            }
            if (labels != null && labels.Count > 0)
            {
                string labelParameter = "[";
                foreach (Label label in labels)
                {
                    labelParameter += label.id + ",";
                }
                labelParameter = labelParameter.Substring(0, labelParameter.Length - 1);
                labelParameter += "]";
                parameters.Add(new Param("labels", labelParameter));
            }
            if (date != null)
            {
                parameters.Add(new Param("date_string", date));
            }
            try
            {
                return await MakeRequest<Item>("add_item", parameters: parameters);
            }
            catch(RequestException e)
            {
                if (e.Content != "")
                {
                    try
                    {
                        TodoistError error = Convert<TodoistError>(e.Content);
                        throw new TodoistException(error, e);
                    }
                    catch (JsonSerializationException notused)
                    {
                        throw e;
                    }
                }
                throw e;
            }
        }

        public void HandleException(RequestException e)
        {
            
        }
    }
}
