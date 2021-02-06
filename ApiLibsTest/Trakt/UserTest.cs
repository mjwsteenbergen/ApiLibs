using ApiLibs.Trakt;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibsTest.Trakt
{
    [Explicit]
    class UserTest
    {
        private UserService user;

        [SetUp]
        public async Task SetUp()
        {
            this.user = (await TraktTest.GetTrakt()).UserService;
        }

        [Test]
        public async Task GetHistoryTest()
        {
            var history = await user.GetHistory();
            Assert.NotNull(history);
        }

        [Test]
        public async Task GetWatchingTest()
        {
            var watching = await user.Watching();
        }
    }
}
