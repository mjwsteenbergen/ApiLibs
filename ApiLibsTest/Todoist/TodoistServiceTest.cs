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
    class TodoistServiceTest
    {
        private TodoistService todoistService;

        [OneTimeSetUp]
        public void GetTodoistService()
        {
            Passwords passwords = Passwords.ReadPasswords();
            todoistService = new TodoistService(passwords.TodoistKey, passwords.TodoistUserAgent);
        }

        [Test]
        public async Task SyncTest()
        {
            await todoistService.GetLabels();
        }
    }
}
