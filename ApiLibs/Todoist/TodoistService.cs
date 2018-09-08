using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using Newtonsoft.Json;
using RestSharp;


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

        internal override async Task<IRestResponse> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var res = await base.HandleRequest(url, call, parameters, headers, content, statusCode);
            if (url.ToLower() == "sync")
            {
                var result = JsonConvert.DeserializeObject<SyncResult>(res.Content);
                if (result.SyncStatus?.Values.Any(i => i != "ok") ?? false)
                {
                    throw new TodoistException(null, null);
                }
            }

            return res;
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
    
        public async Task<long> AddTodo(string name, Project project = null, string date = null, int? priority = null, int? indent = null, string note = null, List<Label> labels = null)
        {
            return await AddTodo(name, project?.Id, date, priority, indent, note, labels);
        }

        public async Task<long> AddTodo(string name, long? project_id = null, string date = null, int? priority = null, int? indent = null, string note = null, List<Label> labels = null)
        {
            var res = await MakeRequest<SyncResult>("sync", parameters: new List<Param>
            {
                new TodoistCommand("item_add", new
                {
                    content = name,
                    project_id,
                    date_string = date,
                    priority,
                    indent,
                    note,
                    labels = labels?.Select(i => i.Id).ToArray()
                }).ToParam()
            });
            return res.TempIdMapping.Values.FirstOrDefault();
        }

        public async Task Update(ItemUpdate update)
        {
            var res = await HandleRequest("sync", parameters: new List<Param>
            {
                new TodoistCommand("item_update", update).ToParam()
            });
        }

        public async Task<long> CreateProject(string name, int? color = null, int? indent = null, int? itemOrder = null, bool? isFavorite = null)
        {
            var res = await MakeRequest<SyncResult>("sync", parameters: new List<Param>
            {
                new TodoistCommand("project_add", new
                {
                    name,
                    color,
                    indent,
                    item_order = itemOrder,
                    is_favorite = isFavorite
                }).ToParam()
            });
            return res.TempIdMapping.Values.FirstOrDefault();
        }

        public async Task RemoveProject(Project project)
        {
            await RemoveProject(project.Id);
        }

        public async Task RemoveProject(long projectId)
        {
            var res = await MakeRequest<string>("sync", parameters: new List<Param>
            {
                new TodoistCommand("project_delete", new
                {
                    ids = new [] {projectId.ToString()}
                }).ToParam()
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
        public string TempId { get; set; }

        [JsonProperty(PropertyName = "args")]
        public Object Arguments { get; set; }

        public TodoistCommand(string type, object args)
        {
            Uuid = (new Random()).Next(0, 10000);
            this.Type = type;
            if (Type != null && Type.Contains("add"))
            {
                TempId = $"{RandomString(8)}-{RandomString(4)}-{RandomString(4)}-{RandomString(4)}-{RandomString(12)}";
            }
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

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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
