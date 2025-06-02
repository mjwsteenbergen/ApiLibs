using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using Martijn.Extensions.AsyncLinq;
using Newtonsoft.Json;

namespace ApiLibs.Todoist.Future
{
    public class TodoistFutureService : RestSharpService
    {
        private readonly string clientId;
        private readonly string clientSecret;

        public TodoistFutureLabelService Labels { get; private set;}
        public TodoistFutureSectionService Sections { get; private set;}
        public TodoistFutureProjectService Projects { get; private set;}
        public TodoistFutureTaskService Tasks { get; private set;}

        public TodoistFutureService(string clientId, string clientSecret) : base("https://api.todoist.com/api/v1/")
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public TodoistFutureService(string accessToken) : base("https://api.todoist.com/api/v1/")
        {
            AddStandardHeader("Authorization", $"Bearer {accessToken}");
            Labels = new TodoistFutureLabelService(this);
            Sections = new TodoistFutureSectionService(this);
            Projects = new TodoistFutureProjectService(this);
            Tasks = new TodoistFutureTaskService(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_authenticator"></param>
        /// <param name="scopes"></param>
        /// <param name="secret">A random string of text</param>
        public void Connect(IOAuth _authenticator, List<TodoistScope> scopes, string secret)
        {
            string MapToString(TodoistScope token) {
                switch (token)
                {
                    case TodoistScope.TaskAdd:
                        return "task:add";
                    case TodoistScope.DataRead:
                        return "data:read";
                    case TodoistScope.DataReadWrite:
                        return "data:read_write";
                    case TodoistScope.DataDelete:
                        return "data:delete";
                    case TodoistScope.ProjectDelete:
                        return "project:delete";
                    case TodoistScope.BackupsRead:
                        return "backups:read";
                    default:
                        throw new KeyNotFoundException(token.ToString());
                }
            }


            var url = $"https://todoist.com/oauth/authorize?client_id={clientId}&scope={string.Join(",", scopes.Select(MapToString))}&state={secret}";
            _authenticator.ActivateOAuth(new Uri(url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret">A random string of text you passed at <see cref="Connect"/></param>
        /// <returns></returns>
        public async Task<TodoistFutureTokenResponse> Exchange(string secret)
        {
            List<Param> parameters = new List<Param>
            {
                new Param("client_id",clientId),
                new Param("client_secret", clientSecret),
                new Param("code", secret)
            };

            var returns = await new BlandService().MakeRequest<TodoistFutureTokenResponse>("https://todoist.com/oauth/access_token", Call.POST, parameters);
            AddStandardHeader("Authorization", "Bearer " + returns.AccessToken);
            return returns;
        }

        internal async IAsyncEnumerable<T> UsePagination<T>(string endpoint, List<Param> @params = null)
        {
            string cursor = null;
            do
            {
                var results = await MakeRequest<TodoistFuturePaginatedResponse<T>>(endpoint, parameters: (@params ?? new List<Param>()).Concat(new List<Param>() { new OParam("cursor", cursor), new("limit", 200) }).ToList());
                cursor = results.NextCursor;
                foreach (var item in results.Results)
                {
                    yield return item;
                }
            } while (cursor != null);
        }
    }

    public class TodoistFutureProjectService : SubService<TodoistFutureService>
    {
        public TodoistFutureProjectService(TodoistFutureService service) : base(service, "projects")
        {
        }

        public IAsyncEnumerable<TodoistProjectSummary> GetProjects() => Service.UsePagination<TodoistProjectSummary>("projects");
        public Task<TodoistProject> GetProject(string projectId) => MakeRequest<TodoistProject>(projectId);
        public Task<TodoistProject> CreateProject(TodoistFutureProjectEdit request) => MakeRequest<TodoistProject>("", Call.POST, content: request);
        public Task<TodoistProject> EditProject(string projectId, TodoistFutureProjectEdit request) => MakeRequest<TodoistProject>(projectId, Call.POST, content: request);
        public Task DeleteProject(string projectId) => MakeRequest(projectId, Call.DELETE);

        public async Task<TodoistProject> GetProjectByName(string name)
        {
            var project = await GetProjects().First(i => i.Name == name);
            return await GetProject(project.Id);
        } 
    }

    public class TodoistFutureProjectEdit
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }

        [JsonProperty("view_style")]
        public string ViewStyle { get; set; }
    }

    public class TodoistFutureSectionService : SubService<TodoistFutureService>
    {
        public TodoistFutureSectionService(TodoistFutureService service) : base(service, "sections")
        {
        }

        public IAsyncEnumerable<TodoistSection> GetSections(string projectId) => Service.UsePagination<TodoistSection>("sections", new List<Param>() { new("project_id", projectId) });
        public Task<TodoistSection> GetSection(string sectionId) => MakeRequest<TodoistSection>(sectionId);
        public Task<TodoistSection> CreateSection(TodoistFutureSectionEdit request) => MakeRequest<TodoistSection>("", Call.POST, content: request);
        public Task<TodoistSection> EditSection(string sectionId, TodoistFutureSectionEdit request) => MakeRequest<TodoistSection>(sectionId, Call.POST, content: request);
        public Task DeleteSection(string sectionId) => MakeRequest(sectionId, Call.DELETE);
    }

    public class TodoistFutureSectionEdit
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }
    }

    public class TodoistFutureTaskService : SubService<TodoistFutureService>
    {
        public TodoistFutureTaskService(TodoistFutureService service) : base(service, "tasks")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="sectionId"></param>
        /// <param name="parentId"></param>
        /// <param name="label">The name of the label</param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IAsyncEnumerable<TodoistTask> GetTasks(string projectId = null, string sectionId = null, string parentId = null, string label = null, string ids = null) => Service.UsePagination<TodoistTask>("tasks", new List<Param>() {
            new OParam("project_id", projectId),
            new OParam("section_id", sectionId),
            new OParam("parent_id", parentId),
            new OParam("label", label),
            new OParam("ids", ids),
        });

        public IAsyncEnumerable<TodoistTask> GetTasksByFilter(string query = null, string lang = null) => Service.UsePagination<TodoistTask>("", new List<Param>() {
            new Param("query", query),
            new OParam("lang", lang),
        });
        public Task<TodoistTask> GetTask(string taskId) => MakeRequest<TodoistTask>(taskId);
        public Task<TodoistTask> CreateTask(TodoistFutureTaskCreate request) => MakeRequest<TodoistTask>("", Call.POST, content: request);
        public Task<TodoistTask> QuickCreateTask(TodoistFutureTaskQuickCreate request) => MakeRequest<TodoistTask>("quick", Call.POST, content: request);
        public Task<TodoistTask> EditTask(string taskId, TodoistFutureTaskEdit request) => MakeRequest<TodoistTask>(taskId, Call.POST, content: request);
        public Task<TodoistTask> ReopenTask(string taskId) => MakeRequest<TodoistTask>($"{taskId}/reopen", Call.POST);
        public Task<TodoistTask> CloseTask(string taskId) => MakeRequest<TodoistTask>($"{taskId}/close", Call.POST, statusCode: HttpStatusCode.NoContent);
        public Task<TodoistTask> CloseTask(TodoistTask task) => CloseTask(task.Id);
        public Task<TodoistTask> MoveTask(string taskId, TodoistFutureTaskMove move) => MakeRequest<TodoistTask>($"{taskId}/move", Call.POST, content: move);
        public Task DeleteTask(string taskId) => MakeRequest(taskId, Call.DELETE);
    }

    public class TodoistFutureTaskMove
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("section_id")]
        public string SectionId { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }
    }

    public class TodoistFutureTaskQuickCreate
    {
        /// <summary>
        /// The text of the task that is parsed. It can include a due date in free form text, a project name starting with the # character (without spaces), a label starting with the @ character, an assignee starting with the + character, a priority (e.g., p1), a deadline between {} (e.g. {in 3 days}), or a description starting from // until the end of the text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("reminder")]
        public string Reminder { get; set; }

        /// <summary>
        /// When this option is enabled, the default reminder will be added to the new item if it has a due date with time set. See also the auto_reminder user option for more info about the default reminder.
        /// </summary>
        [JsonProperty("auto_reminder")]
        public bool? AutoReminder { get; set; }

        [JsonProperty("meta")]
        public bool? Meta { get; set; }
    }

    public class TodoistFutureTaskCreate : TodoistFutureTaskEdit
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("section_id")]
        public string SectionId { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }
    }

    public partial class TodoistFutureTaskEdit
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        [JsonProperty("priority")]
        public long? Priority { get; set; }

        [JsonProperty("assignee_id")]
        public long? AssigneeId { get; set; }

        [JsonProperty("due_string")]
        public string DueString { get; set; }

        [JsonProperty("due_date")]
        [JsonConverter(typeof(DateConverter))]
        public DateTimeOffset? DueDate { get; set; }

        [JsonProperty("due_datetime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTimeOffset? DueDatetime { get; set; }

        [JsonProperty("due_lang")]
        public string DueLang { get; set; }

        [JsonProperty("duration")]
        public long? Duration { get; set; }

        [JsonProperty("duration_unit")]
        public string DurationUnit { get; set; }

        [JsonProperty("deadline_date")]
        public string DeadlineDate { get; set; }

        [JsonProperty("deadline_lang")]
        public string DeadlineLang { get; set; }
    }

    public class TodoistFutureLabelService : SubService<TodoistFutureService>
    {
        public TodoistFutureLabelService(TodoistFutureService service) : base(service, "labels")
        {
        }

        public IAsyncEnumerable<TodoistLabel> GetLabels() => Service.UsePagination<TodoistLabel>("labels");
        public Task<TodoistLabel> GetLabel(string labelId) => MakeRequest<TodoistLabel>(labelId);
        public Task<TodoistLabel> CreateLabel(TodoistFutureLabelEdit request) => MakeRequest<TodoistLabel>("", Call.POST, content: request);
        public Task<TodoistLabel> EditLabel(string labelId, TodoistFutureLabelEdit request) => MakeRequest<TodoistLabel>(labelId, Call.POST, content: request);
        public Task DeleteLabel(string labelId) => MakeRequest(labelId, Call.DELETE);
    }

    public class TodoistFutureLabelEdit
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }
    }
    
}