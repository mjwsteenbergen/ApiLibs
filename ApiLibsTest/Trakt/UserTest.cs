using ApiLibs.Trakt;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibsTest.Trakt
{
    class UserTest
    {
        private UserService user;

        [SetUp]
        public void SetUp()
        {
            this.user = TraktTest.GetTrakt().UserService;
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
