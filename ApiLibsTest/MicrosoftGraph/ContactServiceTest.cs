using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.MicrosoftGraph;
using NUnit.Framework;

namespace ApiLibsTest.MicrosoftGraph
{
    [Explicit]
    class ContactServiceTest
    {
        private PeopleService contacts;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            contacts = (await GraphTest.GetGraphService()).PeopleService;
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
