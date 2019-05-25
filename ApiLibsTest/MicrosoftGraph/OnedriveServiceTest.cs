using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.MicrosoftGraph;
using NUnit.Framework;

namespace ApiLibsTest.MicrosoftGraph
{
    class OnedriveServiceTest
    {
        private OneDriveService oneDrive;


        [OneTimeSetUp]
        public async Task SetUp()
        {
            oneDrive = (await GraphTest.GetGraphService()).OneDriveService;
        }

        [Test]
        public async Task GetPictureFolder()
        {
            var s = await oneDrive.GetFolder("Pictures");
        }

        [Test]
        public async Task GetFolderChildren()
        {
            var s = await oneDrive.GetFolderChildren("Pictures");
        }

        [Test]
        public async Task TestGetContentById()
        {
            var items = await oneDrive.GetFolderChildren("Appdata/Todoist");
            foreach (DriveItem driveItem in items)
            {
                if (driveItem.File != null)
                {
                    var contents = await oneDrive.GetContents(driveItem);
                }
            }
        }

        [Test]
        public async Task TestGetContent()
        {
            var contents = await oneDrive.GetContents("Appdata/Todoist/Kamer Opruimen.txt");
        }
    }
}
