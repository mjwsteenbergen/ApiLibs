using ApiLibs.MicrosoftGraph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibsTest.MicrosoftGraph
{
    public class OneNoteServiceTest
    {
        private OneNoteService onenote;


        [OneTimeSetUp]
        public async Task SetUp()
        {
            onenote = (await GraphTest.GetGraphService()).OneNoteService;
        }

        [Test]
        public async Task GetNoteBooks()
        {
            await onenote.GetMyNotebooks();
        }

        [Test]
        public async Task GetInkContent()
        {
            var res = await onenote.GetPageInk("0-0047023f108505720316b90d27a41e80!1-BE0D5B5E6901EACC!315051");
        }
    }
}
