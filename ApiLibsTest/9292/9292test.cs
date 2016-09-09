using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs._9292;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ApiLibsTest._9292
{
    [TestFixture]
    class _9292test
    {
        _9292Service serv;

        [SetUp]
        public void BeforeEach()
        {
            serv = new _9292Service();
        }

        [Test]
        public async Task TestStatus()
        {
            Status s = await serv.GetStatus();
            Assert.NotNull(s.version);
        }

        [Test]
        public async Task TestJourney()
        {
            await serv.GetJourney("station-delft", "station-leiden-centraal", byTrain: true);
        }

        [Test]
        public async Task GetLocations()
        {
            Assert.NotNull((await serv.GetLocation("schiphol"))[0].name);
        }
    }
}
