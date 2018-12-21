using ApiLibs.Feedly;
using ApiLibs.General;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibsTest.Feedly
{
    class FeedlyServiceTest
    {
        private FeedlyService feedly;

        [OneTimeSetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords();
            feedly = new FeedlyService(passwords.GetPasssword("FeedlyToken"));
        }

        [Test]
        public async Task GetSavedTestAsync()
        {
            var res = await feedly.GetSavedArticles();
        }

        [Test]
        public async Task GetUserTestAsync()
        {
            var res = await feedly.GetUser();
        }

        [Test]
        public async Task GetArticlesFromFeedTest()
        {
            var res = await feedly.GetItemsFromFeed("feed%2Fhttps%3A%2F%2Fcommons.wikimedia.org%2Fw%2Fapi.php%3Faction%3Dfeaturedfeed%26feed%3Dpotd%26feedformat%3Drss%26language%3Den");
        }

        [Test]
        public async Task SaveTestAsync()
        {
            var wikiStream = await feedly.GetItemsFromFeed("feed%2Fhttps%3A%2F%2Fcommons.wikimedia.org%2Fw%2Fapi.php%3Faction%3Dfeaturedfeed%26feed%3Dpotd%26feedformat%3Drss%26language%3Den");
            await feedly.MarkAsSaved(wikiStream.Items.First());
        }

        [Test]
        public async Task UnSaveTestAsync()
        {
            var wikiStream = await feedly.GetItemsFromFeed("feed%2Fhttps%3A%2F%2Fcommons.wikimedia.org%2Fw%2Fapi.php%3Faction%3Dfeaturedfeed%26feed%3Dpotd%26feedformat%3Drss%26language%3Den");
            await feedly.MarkAsUnsaved(wikiStream.Items.First());
        }
    }
}
