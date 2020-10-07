using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.MicrosoftGraph;
using NUnit.Framework;

namespace ApiLibsTest.MicrosoftGraph
{
    class TodoServiceTest
    {
        private TodoService todo;


        [OneTimeSetUp]
        public async Task SetUp()
        {
            todo = (await GraphTest.GetGraphService()).TodoService;
        }

        [Test]
        public async Task GetTodoFolders()
        {
            await todo.GetFolders();
        }

        [Test]
        public async Task GetTasks()
        {
            var s = await todo.GetTasks();
        }

        [Test]
        public async Task GetTasksFromFolder()
        {
            var res = await todo.GetTasks("Markender");
        }

        public async Task<Todo> CreateItem(Todo newtodo)
        {
            return await todo.Create(newtodo);
        }

        [Test]
        public async Task DeleteTasks()
        {
            await todo.Delete(await CreateItem(new Todo{ Title = "Delete"}));
        }

        [Test]
        public async Task CompleteTasks()
        {
            await todo.Complete(await CreateItem(new Todo { Title = "Complete" }));
        }
    }
}
