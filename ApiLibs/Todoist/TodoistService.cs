using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public TodoistService(string todoistKey, string todoistUserAgent) : base("https://todoist.com/api/v8/")
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

        protected internal override async Task<string> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var res = await base.HandleRequest(url, call, parameters, headers, content, statusCode);
            if (url.ToLower() == "sync")
            {
                var result = JsonConvert.DeserializeObject<SyncResult>(res);
                var jsonError = result.SyncStatus?.FirstOrDefault(i => !(i.Value is string)).Value;
                if (jsonError != null)
                {
                    
                    var error = (jsonError as JObject).ToObject(typeof(TodoistError)) as TodoistError;
                    throw new TodoistException(error, error.http_code, "", url, "Wrong call", res);
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
            await MarkTodoAsDone(todo.Id);
        }

        public Task MarkTodoAsDone(IEnumerable<Item> removableOnes)
        {
            return MarkTodoAsDone(removableOnes.Select(i => i.Id));
        }

        public Task MarkTodoAsDone(IEnumerable<long> removableOnes)
        {
            return HandleRequest("sync", parameters: new List<Param> { TodoistCommand.ToParam(removableOnes.Select(i => new TodoistCommand("item_close", new ItemUpdate(i)))) });
        }

        public async Task MarkTodoAsDone(long id)
        {
            List<Param> parameters = new List<Param>
            {
                new TodoistCommand("item_close", new ItemUpdate(id)).ToParam()
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
    
        public async Task<long> AddTodo(string name, Project project = null, string date = null, int? priority = null, int? parentId = null, List<Label> labels = null)
        {
            return await AddTodo(name, project?.Id, date, priority, parentId, labels);
        }

        public async Task<long> AddTodo(string name, long? project_id = null, string date = null, int? priority = null, int? parentId = null, List<Label> labels = null)
        {
            var res = await MakeRequest<SyncResult>("sync", parameters: new List<Param>
            {
                new TodoistCommand("item_add", new Item()
                {
                    Content = name,
                    ProjectId = project_id,
                    Due = date != null ? new Due {
                        String = date
                    } : null,
                    Priority = priority,
                    ParentId = parentId,
                    Labels = labels?.Select(i => i.Id).ToList()
                }).ToParam()
            });
            return res.TempIdMapping.Values.FirstOrDefault();
        }


        public async Task<List<long>> AddTodo(IEnumerable<Item> items)
        {
            var original = items.Select(i => new TodoistCommand("item_add", i)).ToList();
            var res = await MakeRequest<SyncResult>("sync", parameters: new List<Param>
            {
                TodoistCommand.ToParam(original)
            });
            return original.Select(i => res.TempIdMapping.First(j => j.Key == i.TempId).Value).ToList();
        }

        public async Task Update(ItemUpdate update)
        {
            var res = await HandleRequest("sync", parameters: new List<Param>
            {
                new TodoistCommand("item_update", update).ToParam()
            });
        }

        public async Task Update(IEnumerable<ItemUpdate> updates)
        {
            if(updates.Count() == 0)
            {
                return;
            }

            var res = await HandleRequest("sync", parameters: new List<Param>
            {
                TodoistCommand.ToParam(updates.Select(i => new TodoistCommand("item_update", i)))
            });
        }

        public async Task<long> CreateProject(string name, long? color = null, long? parentId = null, bool? isFavorite = null)
        {
            var res = await MakeRequest<SyncResult>("sync", parameters: new List<Param>
            {
                new TodoistCommand("project_add", new Project()
                {
                    Name = name,
                    Color = color,
                    ParentId = parentId,
                    IsFavorite = isFavorite
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
                new TodoistCommand("project_delete", new Project
                {
                    Id = projectId
                }).ToParam()
            });
        }

        public async Task RemoveTodo(Item other)
        {
            var res = await MakeRequest<string>("sync", parameters: new List<Param>
            {
                new TodoistCommand("item_delete", new Item()
                {
                    Id = other.Id
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

        private TodoistCommand(string type) {
            Uuid = new Random((int) DateTime.Now.Ticks).Next(0, 10000);
            this.Type = type;
            if (Type != null && Type.Contains("add"))
            {
                TempId = $"{RandomString(8)}-{RandomString(4)}-{RandomString(4)}-{RandomString(4)}-{RandomString(12)}";
            }
        }

        public TodoistCommand(string type, Item args) : this(type)
        {
            Arguments = args;
        }

        public TodoistCommand(string type, Note args) : this(type)
        {
            Arguments = args;
        }

        public TodoistCommand(string type, Project args) : this(type)
        {
            Arguments = args;
        }

        public string ToCommand()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            string serializedObject = JsonConvert.SerializeObject(this, jsonSerializerSettings);
            return serializedObject;
        }

        public static Param ToParam(IEnumerable<TodoistCommand> commands)
        {
            return new Param("commands", "[" + commands.Select(i => i.ToCommand()).Aggregate((i,j) => $"{i},{j}") + "]");
        }

        public Param ToParam()
        {
            return ToParam(new List<TodoistCommand> { this });
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        { 
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public class ItemUpdate : Item
    {
        public ItemUpdate(long id)
        {
            Id = id;
        }
    }
}
