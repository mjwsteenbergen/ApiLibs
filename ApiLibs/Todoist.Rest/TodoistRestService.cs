using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibs.TodoistRest
{
    public class TodoistRestService : Service
    {
        public TodoistRestService(string token) : base("https://api.todoist.com/rest/v1/", false)
        {
            AddStandardHeader("Authorization", $"Bearer {token}");
        }

        private List<TodoistProject> project_cache = new List<TodoistProject>();

        public Task<List<TodoistProject>> GetProjects() => MakeRequest<List<TodoistProject>>("projects");
        public Task<TodoistProject> GetProject(int id) => MakeRequest<TodoistProject>("projects/" + id);

        public Task UpdateProject(TodoistProject project) => UpdateProject(project.Id ?? throw new ArgumentNullException(nameof(project.Id)), project);
        public Task UpdateProject(long id, TodoistProject project) => MakeRequest<TodoistProject>("projects/" + id, Call.POST, content: project, statusCode: System.Net.HttpStatusCode.NoContent);

        public Task DeleteProject(TodoistProject project) => DeleteProject(project.Id ?? throw new ArgumentNullException(nameof(project.Id)));
        public Task DeleteProject(long id) => MakeRequest<string>("projects/" + id, Call.DELETE);

        public Task<TodoistProject> CreateProject(TodoistProject project) => MakeRequest<TodoistProject>("projects", Call.POST, content: project);


        public Task<List<TodoistSection>> GetSections() => MakeRequest<List<TodoistSection>>("sections");
        public Task<List<TodoistSection>> GetSections(TodoistProject project) => GetSections(project.Id ?? throw new ArgumentNullException(nameof(project.Id)));
        public Task<List<TodoistSection>> GetSections(long id) => MakeRequest<List<TodoistSection>>("sections", parameters: new List<Param> { new Param("project_id", id) });
        public Task<TodoistSection> GetSection(long id) => MakeRequest<TodoistSection>("sections/" + id);
        public Task<TodoistSection> CreateSection(TodoistSection section) => MakeRequest<TodoistSection>("sections", Call.POST, content: section);

        public async Task<TodoistProject> GetProject(string name)
        {
            TodoistProject proj = project_cache.FirstOrDefault(i => i.Name == name);
            if(proj != null) return proj;

            project_cache = await GetProjects();
            return project_cache.FirstOrDefault(i => i.Name == name) ?? throw new KeyNotFoundException("A project with the following name does not exist: " + name);
        }

        public Task UpdateSection(long id, TodoistSection section) => MakeRequest<TodoistSection>("sections/" + id, Call.POST, content: section, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task UpdateSection(TodoistSection section) => UpdateSection(section.Id ?? throw new ArgumentNullException(nameof(section.Id)), section);
        public Task DeleteSection(long id) => MakeRequest<string>("sections/" + id, Call.DELETE, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task DeleteSection(TodoistSection section) => DeleteSection(section.Id ?? throw new ArgumentNullException(nameof(section.Id)));


        public Task<List<TodoistTask>> GetTasks(long? project_id = null, long? label_id = null, string filter = null, string lang = null, List<long> ids = null) => MakeRequest<List<TodoistTask>>("tasks", parameters: new List<Param> {
            new OParam(nameof(project_id), project_id),
            new OParam(nameof(label_id), label_id),
            new OParam(nameof(filter), filter),
            new OParam(nameof(lang), lang),
            new OParam(nameof(ids), ids)
        });
        public Task<List<TodoistTask>> GetTasks(TodoistProject proj) => GetTasks(project_id: proj.Id);

        public Task<TodoistTask> GetTask(long id) => MakeRequest<TodoistTask>("tasks/"+id);
        public Task<TodoistTask> CreateTask(TodoistRequestTask task) => MakeRequest<TodoistTask>("tasks", Call.POST, content: task);
        public Task UpdateTasks(long id, TodoistTask task) => MakeRequest<string>("tasks/" + id, Call.POST, content: task, statusCode: System.Net.HttpStatusCode.NoContent);


        public Task UpdateTasks(TodoistTask task) => UpdateTasks(task.Id ?? throw new ArgumentNullException(nameof(task.Id)), task);
        public Task Close(TodoistTask task) => Close(task.Id ?? throw new ArgumentNullException(nameof(task.Id)));
        public Task Close(long id) => MakeRequest<string>($"tasks/{id}/close", Call.POST, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task Reopen(TodoistTask task) => Reopen(task.Id ?? throw new ArgumentNullException(nameof(task.Id)));
        public Task Reopen(long id) => MakeRequest<string>($"tasks/{id}/reopen", Call.POST, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task DeleteTask(TodoistTask task) => DeleteTask(task.Id ?? throw new ArgumentNullException(nameof(task.Id)));
        public Task DeleteTask(long id) => MakeRequest<string>($"tasks/{id}", Call.DELETE, statusCode: System.Net.HttpStatusCode.NoContent);

        public Task<List<TodoistLabel>> GetLabels() => MakeRequest<List<TodoistLabel>>("labels");
        public Task<TodoistLabel> CreateLabel(TodoistLabel label) => MakeRequest<TodoistLabel>("labels", Call.POST, content: label);
        public Task UpdateLabel(TodoistLabel label) => UpdateLabel(label.Id ?? throw new ArgumentNullException(nameof(label.Id)), label);
        public Task UpdateLabel(long id, TodoistLabel label) => MakeRequest<string>("labels/" + id, Call.POST, content: label, statusCode: System.Net.HttpStatusCode.NoContent);
        public Task DeleteLabel(TodoistLabel label) => DeleteLabel(label.Id ?? throw new ArgumentNullException(nameof(label.Id)));
        public Task DeleteLabel(long id) => MakeRequest<string>("labels/" + id, Call.DELETE, statusCode: System.Net.HttpStatusCode.NoContent);
    }
}