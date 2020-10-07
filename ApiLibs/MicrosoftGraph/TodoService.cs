using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.MicrosoftGraph
{
    public class TodoService : GraphSubService
    {
        public TodoService(GraphService service) : base(service, "beta")
        {
        }

        public async Task<List<TaskFolder>> GetFolders()
        {
            return (await MakeRequest<FolderResult>("me/todo/lists?$top=200")).Value;
        }

        public async Task<TaskFolder> GetFolder(string name)
        {
            return (await GetFolders()).First(i => i.DisplayName == name);
        }

        public async Task<List<Todo>> GetTasks()
        {
            return (await MakeRequest<TaskResult>("me/todo/tasks?$top=200")).Value;
        }

        public Task<List<Todo>> GetTasks(TaskFolder folder)
        {
            return GetTasks(folder.Id);
        }

        public async Task<List<Todo>> GetTasks(string folderId)
        {
            return (await MakeRequest<TaskResult>($"me/todo/lists/{folderId}/tasks?$top=200")).Value;
        }

        public Task<Todo> Create(string content, TaskFolder folder)
        {
            return Create(new Todo {
                Title = content
            }, folder?.Id);
        }

        public Task<Todo> Create(Todo todo, TaskFolder folder)
        {
            return Create(todo, folder?.Id);
        }

        public Task<Todo> Create(Todo todo, string id = null)
        {
            if (id != null)
            {
                id = "/lists/" + id;
            }
            return MakeRequest<Todo>($"me/todo{id}/tasks", Call.POST, content: todo);
        }

        public Task<Todo> Update(string id, Todo todo)
        {
            return MakeRequest<Todo>($"me/todo/tasks('{id}')", Call.PATCH, content: todo);
        }


        public Task<Todo> Update(Todo original, Todo newValues)
        {
            return Update(original.Id, newValues);
        }

        public Task Delete(string id)
        {
            return HandleRequest($"me/todo/tasks('{id}')", Call.DELETE, statusCode: HttpStatusCode.NoContent);
        }

        public Task Delete(Todo todo)
        {
            return Delete(todo.Id);
        }

        public async Task Complete(string id)
        {
            await HandleRequest($"me/todo/tasks('{id}')/complete", Call.POST);
        }

        public Task Complete(Todo todo)
        {
            return Complete(todo.Id);
        }

        public Task CreateFolder(TaskFolder folder)
        {
            return MakeRequest<TaskFolder>("me/todo/lists", Call.POST, content: folder);
        }

        public Task CreateFolder(string projectName)
        {
            return CreateFolder(new TaskFolder
            {
                DisplayName = projectName
            });
        }

        public Task RemoveFolder(TaskFolder taskFolder)
        {
            return RemoveFolder(taskFolder.Id);
        }

        public Task RemoveFolder(string taskFolderId)
        {
            return HandleRequest($"me/todo/lists('{taskFolderId}')", Call.DELETE, statusCode: HttpStatusCode.NoContent);
        }
    }
}
