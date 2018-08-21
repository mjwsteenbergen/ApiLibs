using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Outlook;
using NUnit.Framework;

namespace ApiLibsTest.Outlook
{
    class ContactServiceTest
    {
        private GraphService _graph;
        private ContactService contacts;

        [OneTimeSetUp]
        public void SetUp()
        {
            Passwords passwords =
                Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            _graph = new GraphService(passwords.OutlookRefreshToken, passwords.OutlookClientID,
                passwords.OutlookClientSecret, passwords.OutlookEmail);
            contacts = _graph.ContactService;
        }

        [Test]
        public async Task GetContacts()
        {
            var contacts = await this.contacts.GetAllContacts();
        }

        [Test]
        public async Task GetContactsFromSearch()
        {
            var contacts = await this.contacts.GetContacts("Meneer");
        }
    }
}
