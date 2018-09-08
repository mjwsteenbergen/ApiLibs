using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.MicrosoftGraph;
using NUnit.Framework;

namespace ApiLibsTest.MicrosoftGraph
{
    class GraphTest
    {
        private GraphService _graph;

        [SetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords();
            _graph = GetGraphService();
        }

        public static GraphService GetGraphService()
        {
            Passwords passwords = Passwords.ReadPasswords();
            var _graph = new GraphService(passwords.OutlookRefreshToken, passwords.OutlookClientID, passwords.OutlookClientSecret, passwords.OutlookEmail);
            _graph.Changed += (sender, args) =>
            {
                passwords.OutlookRefreshToken = args.RefreshToken;
                passwords.WriteToFile();
            };
            return _graph;
        }

        [Test]
        [Ignore("Startup")]
        public void OauthTest()
        {
            Passwords passwords = Passwords.ReadPasswords();
            _graph.Connect(passwords.OutlookClientID, "https://nntn.nl", new StupidOAuth(), new List<GraphService.Scopes>
            {
                GraphService.Scopes.Calendars_ReadWrite,
                GraphService.Scopes.Files_ReadWrite_All,
                GraphService.Scopes.Contacts_ReadWrite,
                GraphService.Scopes.Device_Read,
                GraphService.Scopes.Mail_ReadWrite,
                GraphService.Scopes.Notes_ReadWrite_All
            });
        }

        [Test]
        [Ignore("Startup")]
        public async Task ChangeToToken()
        {
            Passwords passwords = Passwords.ReadPasswords();
            var res = await _graph.ConvertToToken(passwords.OutlookClientID, passwords.OutlookClientSecret, "YOUR CODE HERE", "https://nntn.nl");
        }

        [Test]
        public async Task ConnectTest()
        {
            var res = await _graph.MailService.GetFolders(new OData());
        }
    }
}
