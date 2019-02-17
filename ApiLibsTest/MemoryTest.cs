using ApiLibs.General;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        public void TestWriteSimpleObject()
        {
            mem.WriteFile("SimpleObject.json", new Simple
            {
                MyProperty = 1,
                MyString = "1",
                S = "1"
            });
        }

        [Test]
        public void TestRead()
        {
            mem.WriteFile("TestRead.json", new Simple
            {
                MyProperty = 1,
                MyString = "hello"
            });
            Simple s = mem.ReadFile<Simple>("TestRead.json");
            Assert.AreEqual(s.MyProperty, 1);
            Assert.AreEqual(s.MyString, "hello");
        }

        [Test]
        public void TestReadNonExistingObject()
        {
            Assert.Catch(typeof(FileNotFoundException), () =>
            {
                Simple s = mem.ReadFile<Simple>("TestReadNonExistingObject.json");
            });
        }

        [Test]
        public void TestReadNonExistingObjectWithAutomaticDefault()
        {
            Simple s = mem.ReadFileWithDefault<Simple>("TestReadNonExistingObjectWithAutomaticDefault.json");

            Assert.AreEqual(s.MyProperty, 0);
            Assert.AreEqual(s.MyString, null);
        }

        [Test]
        public void TestReadNonExistingObjectWithDefault()
        {
            Simple s = mem.ReadFile("TestReadNonExistingObjectWithDefault.json", new Simple
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
