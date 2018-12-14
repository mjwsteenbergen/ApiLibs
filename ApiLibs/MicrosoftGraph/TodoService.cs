using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class TodoService : SubService
    {
        public TodoService(GraphService service) : base(service)
        {
        }

        internal override Task<T> MakeRequest<T>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.MakeRequest<T>("beta/" + url, m, parameters, header, content, statusCode);
        }

        internal override Task<IRestResponse> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.HandleRequest("beta/" + url, m, parameters, header, content, statusCode);
        }

        public async Task<List<TaskFolder>> GetFolders()
        {
            return (await MakeRequest<FolderResult>("me/outlook/taskFolders?$top=200")).Value;
        }

        public async Task<TaskFolder> GetFolder(string name)
        {
            return (await GetFolders()).First(i => i.Name == name);
        }

        public async Task<List<Todo>> GetTasks()
        {
            return (await MakeRequest<TaskResult>("me/outlook/tasks?$top=200")).Value;
        }

        public Task<List<Todo>> GetTasks(TaskFolder folder)
        {
            return GetTasks(folder.Id);
        }

        public async Task<List<Todo>> GetTasks(string folderId)
        {
            return (await MakeRequest<TaskResult>($"me/outlook/taskFolders/{folderId}/tasks?$top=200")).Value;
        }

        public Task<Todo> Create(Todo todo, TaskFolder folder)
        {
            return Create(todo, folder?.Id);
        }

        public Task<Todo> Create(Todo todo, string id = null)
        {
            if (id != null)
            {
                id = "/taskFolders/" + id;
            }
            return MakeRequest<Todo>($"me/outlook{id}/tasks", Call.POST, content: todo);
        }

        public Task<Todo> Update(string id, Todo todo)
        {
            return MakeRequest<Todo>($"me/outlook/tasks('{id}')", Call.PATCH, content: todo);
        }


        public Task<Todo> Update(Todo original, Todo newValues)
        {
            return Update(original.Id, newValues);
        }

        public Task Delete(string id)
        {
            return HandleRequest($"me/outlook/tasks('{id}')", Call.DELETE, statusCode: HttpStatusCode.NoContent);
        }

        public Task Delete(Todo todo)
        {
            return Delete(todo.Id);
        }

        public async Task Complete(string id)
        {
            await HandleRequest($"me/outlook/tasks('{id}')/complete", Call.POST);
        }

        public Task Complete(Todo todo)
        {
            return Complete(todo.Id);
        }

        public Task CreateFolder(TaskFolder folder)
        {
            return MakeRequest<TaskFolder>("me/outlook/taskFolders", Call.POST, content: folder);
        }

        public Task CreateFolder(string projectName)
        {
            return CreateFolder(new TaskFolder
            {
                Name = projectName
            });
        }

        public Task RemoveFolder(TaskFolder taskFolder)
        {
            return RemoveFolder(taskFolder.Id);
        }

        public Task RemoveFolder(string taskFolderId)
        {
            return HandleRequest("me/outlook/taskFolders/" + taskFolderId, Call.DELETE, statusCode: HttpStatusCode.NoContent);
        }
    }
}
