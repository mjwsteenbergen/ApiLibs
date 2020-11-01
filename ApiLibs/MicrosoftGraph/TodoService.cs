using System;
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

        public Task<List<Todo>> GetTasks(TaskFolder folder, bool includeCompleted = false)
        {
            return GetTasks(folder.Id);
        }

        public async Task<List<Todo>> GetTasks(string folderId, bool includeCompleted = false)
        {
            var items = (await MakeRequest<TaskResult>($"me/todo/lists/{folderId}/tasks?$top=200")).Value;
            if(!includeCompleted) {
                items = items.Where(i => i.CompletedDateTime == null).ToList();
            }
            return items;
        }

        public Task<Todo> Create(string content, string folderId) => Create(new Todo
        {
            Title = content
        }, folderId);

        public Task<Todo> Create(string content, TaskFolder folder) => Create(content, folder.Id);

        public Task Delete(string todoId, TaskFolder folder) => Delete(todoId, folder.Id);

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

        public Task<Todo> Update(string id, string listId, Todo todo)
        {
            return MakeRequest<Todo>($"me/todo/lists/{listId}/tasks/{id}", Call.PATCH, content: todo);
        }


        public Task<Todo> Update(Todo original, string listId, Todo newValues)
        {
            return Update(original.Id, listId, newValues);
        }

        public Task Delete(string id, string listId)
        {
            return HandleRequest($"me/todo/lists/{listId}/tasks/{id}", Call.DELETE, statusCode: HttpStatusCode.NoContent);
        }

        public Task Delete(Todo todo, TaskFolder listId) => Delete(todo.Id, listId.Id);
        
        public Task Complete(Todo other, TaskFolder listId) => Complete(other, listId.Id);

        public Task Complete(string id, string listId)
        {
            return Update(id, listId, new Todo {
                CompletedDateTime = new DatetimeTimeZone {
                    DateTime = DateTimeOffset.UtcNow
                },
                Status = "completed"
            });
        }

        public Task Complete(Todo todo, string listId)
        {
            return Complete(todo.Id, listId);
        }

        public Task<TaskFolder> CreateFolder(TaskFolder folder)
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
