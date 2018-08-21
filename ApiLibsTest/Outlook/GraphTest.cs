using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Outlook;
using NUnit.Framework;

namespace ApiLibsTest.Outlook
{
    class GraphTest
    {
        private GraphService _graph;

        [SetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            _graph = new GraphService(passwords.OutlookRefreshToken, passwords.OutlookClientID, passwords.OutlookClientSecret, passwords.OutlookEmail);
        }

        [Test]
        [Ignore("Startup")]
        public void OauthTest()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            _graph.Connect(passwords.OutlookClientID, "https://nntn.nl", new StupidOAuth(), new List<GraphService.Scopes>
            {
                GraphService.Scopes.Calendars_ReadWrite,
                GraphService.Scopes.Contacts_ReadWrite,
                GraphService.Scopes.Device_Read,
                GraphService.Scopes.Mail_ReadWrite,
                GraphService.Scopes.Notes_ReadWrite_All
            });
        }

        //Ma36824d6-484d-4679-c4e1-5405e3dd09bf

        [Test]
//        [Ignore("Startup")]
        public async Task ChangeToToken()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            var res = await _graph.ConvertToToken(passwords.OutlookClientID, passwords.OutlookClientSecret, "YOUR CODE HERE", "https://nntn.nl");
        }

        [Test]
        public async Task ConnectTest()
        {
            var res = await _graph.GetFolders(new OData());
        }
    }
}
