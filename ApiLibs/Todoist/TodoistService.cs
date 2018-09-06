using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;
using Newtonsoft.Json;


namespace ApiLibs.Todoist
{
    public class TodoistService : Service
    {
        private readonly SyncRoot _syncObject = new SyncRoot();

        /// <summary>
        /// Get an access token by going to https://todoist.com/Users/viewPrefs?page=account
        /// </summary>
        /// <param name="todoistKey"></param>
        /// <param name="todoistUserAgent"></param>
        public TodoistService(string todoistKey, string todoistUserAgent) : base("https://todoist.com/API/v7/")
        {
            AddStandardParameter(new Param("user-agent", todoistUserAgent));
            AddStandardParameter(new Param("token", todoistKey));
            AddStandardParameter(new Param("sync_token", "*"));
        }

        private async Task Sync()
        {
            List<Param> parameters = new List<Param> { new Param("resource_types", @"[""all""]") };
            SyncRoot syncobject = await MakeRequest<SyncRoot>("sync", parameters: parameters);
            UpdateParameterIfExists(new Param("sync_token", syncobject.SyncToken));
            this._syncObject.Projects = Merger.Merge(_syncObject.Projects, syncobject.Projects);
            this._syncObject.Labels = Merger.Merge(_syncObject.Labels, syncobject.Labels);
            this._syncObject.Items = Merger.Merge(_syncObject.Items, syncobject.Items);
        }

        public async Task<List<Project>> GetProjects()
        {
            await Sync();
            return _syncObject.Projects.ToList();
        }

        public async Task<List<Label>> GetLabels()
        {
            await Sync();
            return _syncObject.Labels.ToList();
        }

        public async Task<List<Item>> GetItems()
        {
            await Sync();
            return _syncObject.Items.ToList();
        }

        public async Task<Label> GetLabel(string name)
        {
            foreach (Label label in await GetLabels())
            {
                if (label.Name == name)
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
                if (p.Name.ToLower() == projectName.ToLower())
                {
                    return p;
                }
            }
            throw new KeyNotFoundException(projectName + " was not found");
        }

        public async Task<SearchResult> Search(string s)
        {
            List<Param> parameters = new List<Param>
            {
                new Param("queries", s)
            };

            List<SearchResult> obj = await MakeRequest<List<SearchResult>>("query", parameters: parameters);

            return obj[0] ?? new SearchResult();
        }

        public async Task MarkTodoAsDone(Item todo)
        {
            List<Param> parameters = new List<Param>
            {
                new TodoistCommand("item_close", new ItemUpdate()
                {
                    Id = todo.Id
                }).ToParam()
            };
            await HandleRequest("sync", parameters: parameters);
        }

        public async Task AddNote(string note, Item todo)
        {
            await AddNote(note, todo.Id);
        }

        public async Task AddNote(string note, long itemId)
        {
            Note noteObject = new Note { Content = note, ItemId = itemId};
            await MakeRequest<Note>("sync",
                parameters: new List<Param> { new TodoistCommand("note_add", noteObject).ToParam() });
        }
    
        public async Task<Item> AddTodo(string name, Project project = null, List<Label> labels = null, string date =null)
        {
            long id = project?.Id ?? -1;
            return await AddTodo(name, id, labels, date);
        }

        public async Task<Item> AddTodo(string name, long id, List<Label> labels = null, string date = null)
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
                    labelParameter += label.Id + ",";
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
                    catch (JsonSerializationException)
                    {
                        throw e;
                    }
                }
                throw e;
            }
        }

        public async Task Update(ItemUpdate update)
        {
            var res = await HandleRequest("sync", parameters: new List<Param>
            {
                new TodoistCommand("item_update", update).ToParam()
            });
        }
    }

    public class TodoistCommand
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public int Uuid { get; set; }

        [JsonProperty(PropertyName = "temp_id")]
        public int? Temp_id { get; set; }

        [JsonProperty(PropertyName = "args")]
        public Object Arguments { get; set; }

        public TodoistCommand(string type, object args)
        {
            Uuid = (new Random()).Next(0, 10000);
            if (type.Contains("add"))
            {
                Temp_id = (new Random()).Next(0, 10000);
            }
            this.Type = type;
            Arguments = args;
        }

        public string ToCommand()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            string serializedObject = JsonConvert.SerializeObject(this, jsonSerializerSettings);
            return serializedObject;
        }

        public Param ToParam()
        {
            return new Param("commands", "[" + ToCommand() + "]");
        }
    }

    public class ItemUpdate
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("date_string")]
        public string DateString { get; set; }

        [JsonProperty("date_lang")]
        public string DateLang { get; set; }

        [JsonProperty("due_date_utc")]
        public string DueDateUtc { get; set; }

        [JsonProperty("priority")]
        public long? Priority { get; set; }

        [JsonProperty("indent")]
        public long? Indent { get; set; }

        [JsonProperty("item_order")]
        public long? ItemOrder { get; set; }

        [JsonProperty("day_order")]
        public long? DayOrder { get; set; }

        [JsonProperty("collapsed")]
        public long? Collapsed { get; set; }

        [JsonProperty("labels")]
        public List<long> Labels { get; set; }

        [JsonProperty("assigned_by_uid")]
        public string AssignedByUid { get; set; }

        [JsonProperty("responsible_uid")]
        public string ResponsibleUid { get; set; }
    }
}
