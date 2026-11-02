using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Martijn.Extensions.JSONTest
{
    public enum Colour
    {
        Red,
        Green = 5,
        Blue = 7,
        second = 9,
    }

    [Flags]
    public enum Rights
    {
        None = 0,
        Read = 1,
        Write = 2,
    }

    public class Holder<T>
    {
        public T V { get; set; } = default!;
    }

    public class Nested
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    /// <summary>
    /// For every target type, feeds a list of JSON literals through both serializers, as a
    /// property value and as the root value, and requires the same outcome.
    /// </summary>
    [Category("NoInternet")]
    public class PrimitiveReadParityTests
    {
        public static readonly string[] Literals =
        {
            "null", "true", "false",
            "0", "1", "-1", "1.0", "1.5", "2.5", "-0", "1e2", "1E-2", "2147483648", "12345678901",
            "99999999999999999999", "3.5e38", "1e400", "0.1", "123.456789012345678901",
            "\"\"", "\" \"", "\"0\"", "\"1\"", "\"-1\"", "\"1.5\"", "\" 12 \"", "\"1,000\"", "\"1e3\"", "\"abc\"",
            "\"true\"", "\"True\"", "\"false\"", "\"FALSE\"", "\"yes\"", "\"NaN\"", "\"Infinity\"", "\"-Infinity\"",
            "\"2020-01-02T03:04:05\"", "\"2020-01-02T03:04:05Z\"", "\"2020-01-02T03:04:05.1234567+02:00\"",
            "\"2020-01-02T03:04:05.12345678Z\"", "\"2020-01-02T03:04:05-0530\"", "\"2020-01-02T03:04:05+01\"",
            "\"2020-01-02T24:00:00\"", "\"2020-01-02\"", "\"2020-01-02 03:04:05\"", "\"01/02/2020\"",
            "\"Thu, 02 Jan 2020 03:04:05 GMT\"", "\"/Date(1577934245000)/\"", "\"/Date(1577934245000+0200)/\"",
            "\"/Date(-1577934245000)/\"", "\"1577934245\"", "\"00:01:02\"", "\"1.02:03:04\"",
            "\"d2719f6b-5c8b-4d39-9a0c-0d3d7b0f4f50\"", "\"{d2719f6b-5c8b-4d39-9a0c-0d3d7b0f4f50}\"",
            "\"https://example.com/a?b=c\"", "\"/relative/path\"", "\"not a uri\"",
            "\"A\"", "\"a\"", "\"ab\"", "\"Red\"", "\"red\"", "\"Green\"", "\" Blue \"", "\"second\"", "\"Second\"",
            "\"Read, Write\"", "\"Red, Blue\"", "\"5\"", "\"6\"", "\"-5\"", "\"é€😀\"", "\"line\\nbreak\"",
            "{}", "{\"Id\":1,\"Name\":\"x\"}", "{\"id\":\"2\",\"name\":3}", "[]", "[1,2]", "[\"x\",null]", "[{\"Id\":1}]",
        };

        public static readonly Type[] TargetTypes =
        {
            typeof(string), typeof(object),
            typeof(int), typeof(int?), typeof(long), typeof(long?), typeof(short), typeof(short?), typeof(byte), typeof(byte?),
            typeof(uint), typeof(ulong), typeof(ushort), typeof(sbyte),
            typeof(double), typeof(double?), typeof(float), typeof(float?), typeof(decimal), typeof(decimal?),
            typeof(bool), typeof(bool?), typeof(char), typeof(char?),
            typeof(DateTime), typeof(DateTime?), typeof(DateTimeOffset), typeof(DateTimeOffset?),
            typeof(Guid), typeof(Guid?), typeof(TimeSpan), typeof(TimeSpan?), typeof(Uri),
            typeof(Colour), typeof(Colour?), typeof(Rights),
            typeof(Nested), typeof(List<int>), typeof(List<string>), typeof(List<object>), typeof(int[]), typeof(IEnumerable<long>),
            typeof(Dictionary<string, int>), typeof(Dictionary<string, object>), typeof(Dictionary<string, string>),
            typeof(List<Nested>), typeof(Nested[]),
        };

        public static IEnumerable<TestCaseData> Types() => TargetTypes.Select(t => new TestCaseData(t).SetName($"Read {Martijn.Extensions.JSONTest.Names.Short(t)}"));

        [TestCaseSource(nameof(Types))]
        public void PropertyValue(Type type)
        {
            var holder = typeof(Holder<>).MakeGenericType(type);
            var failures = new List<string>();
            foreach (var literal in Literals)
            {
                var json = "{\"V\":" + literal + "}";
                Parity.Collect(failures, literal, Parity.CompareRead(json, holder, h => h == null ? null : holder.GetProperty("V")!.GetValue(h)));
            }
            Assert.That(failures, Is.Empty, string.Join("\n", failures));
        }

        [TestCaseSource(nameof(Types))]
        public void RootValue(Type type)
        {
            var failures = new List<string>();
            foreach (var literal in Literals.Concat(new[] { "", "   " }))
            {
                Parity.Collect(failures, "'" + literal + "'", Parity.CompareRead(literal, type));
            }
            Assert.That(failures, Is.Empty, string.Join("\n", failures));
        }

        [TestCaseSource(nameof(Types))]
        public void ListElement(Type type)
        {
            var list = typeof(List<>).MakeGenericType(type);
            var failures = new List<string>();
            foreach (var literal in Literals)
            {
                Parity.Collect(failures, literal, Parity.CompareRead("[" + literal + "]", list));
            }
            Assert.That(failures, Is.Empty, string.Join("\n", failures));
        }
    }

    public static class Names
    {
        public static string Short(Type t)
        {
            if (Nullable.GetUnderlyingType(t) is Type u)
            {
                return Short(u) + "?";
            }
            if (t.IsGenericType)
            {
                return t.Name.Split('`')[0] + "<" + string.Join(",", t.GetGenericArguments().Select(Short)) + ">";
            }
            return t.Name;
        }
    }
}
