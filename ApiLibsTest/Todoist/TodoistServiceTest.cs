using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.MicrosoftGraph;
using ApiLibs.Todoist;
using NUnit.Framework;

namespace ApiLibsTest.Todoist
{
    [Explicit]
    class TodoistServiceTest
    {
        private TodoistService Todoist;

        [OneTimeSetUp]
        public async Task GetTodoistService()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            Todoist = new TodoistService(passwords.TodoistKey, passwords.TodoistUserAgent);
        }

        [Test]
        public async Task SyncTest()
        {
            await Todoist.GetLabels();
        }

        [Test]
        public async Task AddSingleItemTest()
        {
            await Todoist.AddTodo("test", 0);
        }

        [Test]
        public async Task MarkAsDoneTest()
        {
            var id = await Todoist.AddTodo("test", 0);
            await Todoist.MarkTodoAsDone(id);
        }

        [Test]
        public async Task AddMultipleItemsTest()
        {
            var proj = await Todoist.GetProject("Followup");
            await Todoist.AddTodo(new List<Item>
            {
                new Item
                {
                    Content = "Hello",
                    ProjectId = proj.Id,
                    Priority = 2
                },
                new Item
                {
                    Content = "Hello2",
                    ProjectId = proj.Id
                }
            });
        }
    }
}
