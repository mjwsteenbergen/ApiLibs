using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.BingMaps;
using ApiLibs.General;
using NUnit.Framework;

namespace ApiLibsTest.BingMaps
{
    [Explicit]
    class BingMapsTest
    {
        private BingMapsService bingMaps;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            bingMaps = new BingMapsService(passwords.GetPasssword("bing_maps"));
        }

        [Test]
        public async Task TestLocation()
        {
            var res = await bingMaps.Location("Mekelweg 4 Delft");
        }

        [Test]
        public async Task TestRoute()
        {
            var res = await bingMaps.Route(new List<string> {"Mekelweg 4 Delft", "Schiphol"}, mode: BingMapsService.TravelMode.Walking);
        }

        [Test]
        public async Task TestransitRoute()
        {
            var res = await bingMaps.TransitRoute(new List<string> { "Mekelweg 4 Delft", "Schiphol" });
        }
    }
}
