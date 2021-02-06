using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.MicrosoftGraph;
using NUnit.Framework;

namespace ApiLibsTest.MicrosoftGraph
{
    [Explicit]
    class GraphTest
    {
        private GraphService _graph;

        [SetUp]
        public async Task SetUp()
        {
            _graph = await GetGraphService();
        }

        public static async Task<GraphService> GetGraphService()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            var _graph = new GraphService(passwords.OutlookRefreshToken, passwords.OutlookClientID, passwords.OutlookClientSecret, passwords.OutlookEmail);
            return _graph;
        }

        [Test]
        [Ignore("Startup")]
        public async Task OauthTest()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            _graph.Connect(passwords.OutlookClientID, "https://nntn.nl", new StupidOAuth(), new List<GraphService.Scopes>
            {
                GraphService.Scopes.Calendars_ReadWrite,
                GraphService.Scopes.Files_ReadWrite_All,
                GraphService.Scopes.Contacts_ReadWrite,
                GraphService.Scopes.Device_Read,
                GraphService.Scopes.Mail_ReadWrite,
                GraphService.Scopes.People_Read,
                GraphService.Scopes.Tasks_ReadWrite,
                GraphService.Scopes.Notes_Read_All,
                GraphService.Scopes.Notes_Create,
                GraphService.Scopes.Notes_Read,
                GraphService.Scopes.Notes_ReadWrite_All,
                GraphService.Scopes.Notes_ReadWrite,
            });
        }

        [Test]
        [Ignore("Startup")]
        public async Task ChangeToToken()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            var res = await _graph.ConvertToToken(passwords.OutlookClientID, passwords.OutlookClientSecret, "YOUR CODE HERE", "https://nntn.nl");
        }

        [Test]
        public async Task ConnectTest()
        {
            var res = await _graph.MailService.GetFolders(new OData());
        }

        [Test]
        public void TimezoneTest()
        {
            _graph.PreferTimeZone(TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));
        }
    }
}
