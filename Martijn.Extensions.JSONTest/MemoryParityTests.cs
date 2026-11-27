using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using ParityTesting;

namespace Martijn.Extensions.JSONTest
{
    public class OldMemory : Martijn.Extensions.Memory.AsyncMemory
    {
        public readonly Dictionary<string, string> Files = new();

        public override Task<string> Read(string filename) => Task.FromResult(Files.TryGetValue(filename, out var text) ? text : "");

        public override Task WriteString(string filePath, string text)
        {
            Files[filePath] = text;
            return Task.CompletedTask;
        }
    }

    public class NewMemory : Martijn.Extensions.JSON.Memory.AsyncMemory
    {
        public readonly Dictionary<string, string> Files = new();

        public override Task<string> Read(string filename) => Task.FromResult(Files.TryGetValue(filename, out var text) ? text : "");

        public override Task WriteString(string filePath, string text)
        {
            Files[filePath] = text;
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Data written by one AsyncMemory must read back the same through the other. Atlas has
    /// years of blobs written by the Newtonsoft version that the new version has to read.
    /// </summary>
    [Category("NoInternet")]
    public class MemoryParityTests
    {
        public static readonly Type[] StoredTypes =
        {
            typeof(Attributed), typeof(Zoo), typeof(SyncState), typeof(Nested), typeof(List<Nested>),
            typeof(Dictionary<string, object>), typeof(List<(Nested, Animal, DateTime)>), typeof(DateTime), typeof(Colour),
            typeof(Holder<DateTimeOffset?>), typeof(Holder<Dictionary<string, List<DateTime>>>), typeof(Holder<double[]>),
            typeof(Holder<Guid>), typeof(Holder<TimeSpan>), typeof(Holder<Uri>), typeof(Holder<decimal>),
        };

        public static IEnumerable<TestCaseData> Types() => StoredTypes.Select(t => new TestCaseData(t).SetName($"Memory {Names.Short(t)}"));

        private static IEnumerable<object?> Samples(Type type)
        {
            for (int seed = 0; seed < 5; seed++)
            {
                yield return SeededFixture.CreateInstance(SeededFixture.Create(SeededFixture.Seed + seed).Map<IShape, Square>(), type);
            }
        }

        [TestCaseSource(nameof(Types))]
        public async Task WrittenByNewtonsoftReadByNew(Type type)
        {
            foreach (var sample in Samples(type))
            {
                foreach (var indented in new bool?[] { null, false })
                {
                    var oldMemory = new OldMemory { WriteIndented = indented };
                    await oldMemory.Write("file.json", sample);
                    var newMemory = new NewMemory();
                    newMemory.Files["file.json"] = oldMemory.Files["file.json"];

                    Assert.That(await CompareReads(oldMemory, newMemory, type), Is.Null, oldMemory.Files["file.json"]);
                }
            }
        }

        [TestCaseSource(nameof(Types))]
        public async Task WrittenByNewReadByNewtonsoft(Type type)
        {
            foreach (var sample in Samples(type))
            {
                var oldMemory = new OldMemory();
                var newMemory = new NewMemory();
                await oldMemory.Write("file.json", sample);
                await newMemory.Write("file.json", sample);

                var difference = JsonTrees.Difference(JsonTrees.Parse(oldMemory.Files["file.json"]), JsonTrees.Parse(newMemory.Files["file.json"]), "$");
                Assert.That(difference, Is.Null, $"Newtonsoft wrote:\n{oldMemory.Files["file.json"]}\nNew wrote:\n{newMemory.Files["file.json"]}");

                oldMemory.Files["file.json"] = newMemory.Files["file.json"];
                Assert.That(await CompareReads(oldMemory, newMemory, type), Is.Null, newMemory.Files["file.json"]);
            }
        }

        [Test]
        public async Task EmptyFileIsFileNotFound()
        {
            var oldMemory = new OldMemory();
            var newMemory = new NewMemory();
            Assert.ThrowsAsync<FileNotFoundException>(() => oldMemory.Read<Nested>("missing"));
            Assert.ThrowsAsync<FileNotFoundException>(() => newMemory.Read<Nested>("missing"));

            var oldDefault = await oldMemory.Read("missing", new Nested { Id = 3 });
            var newDefault = await newMemory.Read("missing", new Nested { Id = 3 });
            Assert.That(DeepComparer.Compare(oldDefault, newDefault), Is.Null);
            Assert.That(newMemory.Files["missing"], Is.EqualTo(oldMemory.Files["missing"]));

            var oldCreated = await oldMemory.ReadWithDefault<List<int>>("list");
            var newCreated = await newMemory.ReadWithDefault<List<int>>("list");
            Assert.That(DeepComparer.Compare(oldCreated, newCreated), Is.Null);
        }

        [Test]
        public async Task StringsAreStoredAsIs()
        {
            var oldMemory = new OldMemory();
            var newMemory = new NewMemory();
            await oldMemory.Write("s", "not json");
            await newMemory.Write("s", "not json");
            Assert.That(newMemory.Files["s"], Is.EqualTo(oldMemory.Files["s"]));
            Assert.That(await newMemory.Read<string>("s"), Is.EqualTo(await oldMemory.Read<string>("s")));
        }

        /// <summary>Null when both reads give the same value or both throw.</summary>
        private static async Task<string?> CompareReads(OldMemory oldMemory, NewMemory newMemory, Type type)
        {
            object? expected = null, actual = null;
            Exception? oldError = null, newError = null;
            try { expected = await ReadOld(oldMemory, type); } catch (Exception e) { oldError = e; }
            try { actual = await ReadNew(newMemory, type); } catch (Exception e) { newError = e; }
            if (oldError != null || newError != null)
            {
                return oldError != null && newError != null ? null : $"Newtonsoft: {oldError?.Message ?? "ok"}, new: {newError?.Message ?? "ok"}";
            }
            return DeepComparer.Compare(expected, actual);
        }

        private static async Task<object?> ReadOld(OldMemory memory, Type type)
        {
            var task = (Task)typeof(OldMemory).GetMethods().Single(m => m.Name == "Read" && m.IsGenericMethod && m.GetParameters().Length == 1)
                .MakeGenericMethod(type).Invoke(memory, new object[] { "file.json" })!;
            await task;
            return task.GetType().GetProperty("Result")!.GetValue(task);
        }

        private static async Task<object?> ReadNew(NewMemory memory, Type type)
        {
            var task = (Task)typeof(NewMemory).GetMethods().Single(m => m.Name == "Read" && m.IsGenericMethod && m.GetParameters().Length == 1)
                .MakeGenericMethod(type).Invoke(memory, new object[] { "file.json" })!;
            await task;
            return task.GetType().GetProperty("Result")!.GetValue(task);
        }
    }
}
