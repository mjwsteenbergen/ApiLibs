using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibs.TodoistRest
{
    public class TodoistRestService : RestSharpService
    {
        public TodoistRestService(string token) : base("https://api.todoist.com/rest/v2/")
        {
            AddStandardHeader("Authorization", $"Bearer {token}");
        }

        private List<TodoistProject> project_cache = new List<TodoistProject>();

        public Task<List<TodoistProject>> GetProjects() => MakeRequest<List<TodoistProject>>("projects");
        public Task<TodoistProject> GetProject(string id) => MakeRequest<TodoistProject>("projects/" + id);

        public Task UpdateProject(TodoistProject project) => UpdateProject(project.Id ?? throw new ArgumentNullException(nameof(project.Id)), project);
        public Task UpdateProject(string id, TodoistProject project) => MakeRequest<TodoistProject>("projects/" + id, Call.POST, content: project, statusCode: System.Net.HttpStatusCode.NoContent);

        public Task DeleteProject(TodoistProject project) => DeleteProject(project.Id ?? throw new ArgumentNullException(nameof(project.Id)));
        public Task DeleteProject(string id) => MakeRequest<string>("projects/" + id, Call.DELETE);

        public Task<TodoistProject> CreateProject(TodoistProject project) => MakeRequest<TodoistProject>("projects", Call.POST, content: project);


        public Task<List<TodoistSection>> GetSections() => MakeRequest<List<TodoistSection>>("sections");
        public Task<List<TodoistSection>> GetSections(TodoistProject project) => GetSections(project.Id ?? throw new ArgumentNullException(nameof(project.Id)));
        public Task<List<TodoistSection>> GetSections(string id) => MakeRequest<List<TodoistSection>>("sections", parameters: new List<Param> { new Param("project_id", id) });
        public Task<TodoistSection> GetSection(string id) => MakeRequest<TodoistSection>("sections/" + id);
        public Task<TodoistSection> CreateSection(TodoistSection section) => MakeRequest<TodoistSection>("sections", Call.POST, content: section);

        public async Task<TodoistProject> GetProjectByName(string name)
        {
            TodoistProject proj = project_cache.FirstOrDefault(i => i.Name == name);
            if(proj != null) return proj;

            project_cache = await GetProjects();
            return project_cache.FirstOrDefault(i => i.Name == name) ?? throw new KeyNotFoundException("A project with the following name does not exist: " + name);
        }

        public Task UpdateSection(string id, TodoistSection section) => MakeRequest<TodoistSection>("sections/" + id, Call.POST, content: section, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task UpdateSection(TodoistSection section) => UpdateSection(section.Id ?? throw new ArgumentNullException(nameof(section.Id)), section);
        public Task DeleteSection(string id) => MakeRequest<string>("sections/" + id, Call.DELETE, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task DeleteSection(TodoistSection section) => DeleteSection(section.Id ?? throw new ArgumentNullException(nameof(section.Id)));


        public Task<List<TodoistTask>> GetTasks(string project_id = null, string label_id = null, string filter = null, string lang = null, List<string> ids = null) => MakeRequest<List<TodoistTask>>("tasks", parameters: new List<Param> {
            new OParam(nameof(project_id), project_id),
            new OParam(nameof(label_id), label_id),
            new OParam(nameof(filter), filter),
            new OParam(nameof(lang), lang),
            new OParam(nameof(ids), ids)
        });
        public Task<List<TodoistTask>> GetTasks(TodoistProject proj) => GetTasks(project_id: proj.Id);

        public Task<TodoistTask> GetTask(string id) => MakeRequest<TodoistTask>("tasks/"+id);
        public Task<TodoistTask> CreateTask(TodoistRequestTask task) => MakeRequest<TodoistTask>("tasks", Call.POST, content: task);
        public Task UpdateTasks(string id, TodoistTask task) => MakeRequest<string>("tasks/" + id, Call.POST, content: task);


        public Task UpdateTasks(TodoistTask task) => UpdateTasks(task.Id ?? throw new ArgumentNullException(nameof(task.Id)), task);
        public Task Close(TodoistTask task) => Close(task.Id ?? throw new ArgumentNullException(nameof(task.Id)));
        public Task Close(string id) => MakeRequest<string>($"tasks/{id}/close", Call.POST, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task Reopen(TodoistTask task) => Reopen(task.Id ?? throw new ArgumentNullException(nameof(task.Id)));
        public Task Reopen(string id) => MakeRequest<string>($"tasks/{id}/reopen", Call.POST, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task DeleteTask(TodoistTask task) => DeleteTask(task.Id ?? throw new ArgumentNullException(nameof(task.Id)));
        public Task DeleteTask(string id) => MakeRequest<string>($"tasks/{id}", Call.DELETE, statusCode: System.Net.HttpStatusCode.NoContent);

        public Task<List<TodoistLabel>> GetLabels() => MakeRequest<List<TodoistLabel>>("labels");
        public Task<TodoistLabel> CreateLabel(TodoistLabel label) => MakeRequest<TodoistLabel>("labels", Call.POST, content: label);
        public Task UpdateLabel(TodoistLabel label) => UpdateLabel(label.Id ?? throw new ArgumentNullException(nameof(label.Id)), label);
        public Task UpdateLabel(string id, TodoistLabel label) => MakeRequest<string>("labels/" + id, Call.POST, content: label, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task DeleteLabel(TodoistLabel label) => DeleteLabel(label.Id ?? throw new ArgumentNullException(nameof(label.Id)));
        public Task DeleteLabel(string id) => MakeRequest<string>("labels/" + id, Call.DELETE, statusCode: System.Net.HttpStatusCode.NoContent);
    }
}