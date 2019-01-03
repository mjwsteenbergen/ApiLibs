using ApiLibs.MicrosoftGraph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibsTest.MicrosoftGraph
{
    class OneNoteServiceTest
    {
        private OneNoteService onenote;


        [OneTimeSetUp]
        public void SetUp()
        {
            onenote = GraphTest.GetGraphService().OneNoteService;
        }

        [Test]
        public async Task GetNoteBooks()
        {
            await onenote.GetMyNotebooks();
        }
    }
}
