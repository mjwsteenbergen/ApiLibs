using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;
using Newtonsoft.Json;
using RestSharp;


namespace ApiLibs.Todoist
{
    public class TodoistService : Service
    {
        private readonly SyncObject _syncObject = new SyncObject();

        /// <summary>
        /// Get an access token by going to https://todoist.com/Users/viewPrefs?page=account
        /// </summary>
        /// <param name="todoistKey"></param>
        /// <param name="todoistUserAgent"></param>
        public TodoistService(string todoistKey, string todoistUserAgent)
        {
            SetUp("https://todoist.com/API/v7/");
            AddStandardParameter(new Param("user-agent", todoistUserAgent));
            AddStandardParameter(new Param("token", todoistKey));
            AddStandardParameter(new Param("sync_token", "*"));
        }

        public async Task<List<Project>> GetProjects()
        {
            List<Param> parameters = new List<Param> { new Param("resource_types", @"[""projects""]") };
            SyncObject syncobject = await MakeRequest<SyncObject>("sync", parameters: parameters);
            UpdateParameterIfExists(new Param("sync_token", syncobject.sync_token));
            this._syncObject.Projects = Merger.Merge(_syncObject.Projects, syncobject.Projects);
            return _syncObject.Projects.ToList();
        }

        public async Task<List<Label>> GetLabels()
        {
            List<Param> parameters = new List<Param> { new Param("resource_types", @"[""labels""]") };
            SyncObject syncobject = await MakeRequest<SyncObject>("sync", parameters: parameters);
            UpdateParameterIfExists(new Param("sync_token", syncobject.sync_token));
            this._syncObject.Labels = Merger.Merge(_syncObject.Labels, syncobject.Labels);
            return _syncObject.Labels.ToList();
        }

        public async Task<List<Item>> GetItems()
        {
            List<Param> parameters = new List<Param> { new Param("resource_types", @"[""items""]") };
            SyncObject syncobject = await MakeRequest<SyncObject>("sync", parameters: parameters);
            UpdateParameterIfExists(new Param("sync_token", syncobject.sync_token));
            this._syncObject.Items = Merger.Merge(_syncObject.Items, syncobject.Items);
            return _syncObject.Items.ToList();
        }

        public async Task<Label> GetLabel(string name)
        {
            foreach (Label label in await GetLabels())
            {
                if (label.name == name)
                {
                    return label;
                }
            }
            throw new KeyNotFoundException("Label: " + name + " was not found. Try something else");
        }

        

        public async Task<Project> GetProject(string projectName)
        {
            foreach (Project p in await GetProjects())
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
