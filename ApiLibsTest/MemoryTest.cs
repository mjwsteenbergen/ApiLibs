using Martijn.Extensions.Memory;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiLibsTest
{
    [Category("NoInternet")]
    class MemoryTest
    {
        Memory mem;

        public string dirpath => AppDomain.CurrentDomain.BaseDirectory + "MemoryTest" + Path.DirectorySeparatorChar;

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(dirpath);
            mem = new Memory(dirpath);
        }

        [Test]
        public async Task ReadIsWrite() 
        {
            await mem.Write("ReadIsWrite", "ReadIsWrite");

            var readed = await mem.Read("ReadIsWrite");

            Assert.AreEqual("ReadIsWrite", readed);
        }


        [Test]
        public async Task TestWriteSimpleObject()
        {
            await mem.Write("SimpleObject.json", new Simple
            {
                MyProperty = 1,
                MyString = "1",
                S = "1"
            });
        }

        [Test]
        public async Task TestRead()
        {
            await mem.Write("TestRead.json", new Simple
            {
                MyProperty = 1,
                MyString = "hello"
            });
            Simple s = await mem.Read<Simple>("TestRead.json");
            Assert.AreEqual(s.MyProperty, 1);
            Assert.AreEqual(s.MyString, "hello");
        }

        [Test]
        public void TestReadNonExistingObject()
        {
            Assert.CatchAsync(typeof(FileNotFoundException), () => mem.Read<Simple>("TestReadNonExistingObject.json"));
        }

        [Test]
        public async Task TestReadNonExistingObjectWithAutomaticDefault()
        {
            Simple s = await mem.ReadWithDefault<Simple>("TestReadNonExistingObjectWithAutomaticDefault.json");

            Assert.AreEqual(s.MyProperty, 0);
            Assert.AreEqual(s.MyString, null);
        }

        [Test]
        public async Task TestReadNonExistingObjectWithDefault()
        {
            Simple s = await mem.Read("TestReadNonExistingObjectWithDefault.json", new Simple
            {
                MyString = "string"
            });

            Assert.AreEqual(s.MyProperty, 0);
            Assert.AreEqual(s.MyString, "string");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(dirpath, true);
        }

        class Simple
        {
            public int MyProperty { get; set; }
            public string MyString { get; set; }

            public string S;
        }
    }
}
