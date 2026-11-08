using System;
using System.Collections.Generic;
using System.Linq;
using Martijn.Extensions.JSON.Compat;
using NUnit.Framework;
using N = Newtonsoft.Json;

namespace Martijn.Extensions.JSONTest
{
    /// <summary>
    /// Writes values with both serializers and requires byte-identical text. This matters for
    /// ApiLibs' Param, which sends the serialized text of a value as a query or form parameter.
    /// </summary>
    [Category("NoInternet")]
    public class PrimitiveWriteParityTests
    {
        public static IEnumerable<object?> Values()
        {
            yield return null;
            yield return "plain";
            yield return "quote \" backslash \\ slash / tab \t newline \n cr \r bell \u0007 nul \0 del \u007f";
            yield return "html <b>&amp;</b> 'single' +plus";
            yield return "unicode é ü 漢字 😀 \u0085 \u2028 \u2029 \u00a0";
            yield return "";
            yield return true;
            yield return 0;
            yield return -12;
            yield return int.MaxValue;
            yield return long.MinValue;
            yield return (short)3;
            yield return (byte)200;
            yield return 4u;
            yield return ulong.MaxValue;
            yield return 1.0;
            yield return 1.5;
            yield return -0.0;
            yield return 0.1 + 0.2;
            yield return 1e20;
            yield return 1e-7;
            yield return 123456789012345678.0;
            yield return double.NaN;
            yield return double.PositiveInfinity;
            yield return double.NegativeInfinity;
            yield return double.MaxValue;
            yield return 1.5f;
            yield return 3f;
            yield return 0.1f;
            yield return 1m;
            yield return 1.50m;
            yield return -0.001m;
            yield return 79228162514264337593543950335m;
            yield return 'c';
            yield return '"';
            yield return new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            yield return new DateTime(2020, 7, 2, 3, 4, 5, 123, DateTimeKind.Local);
            yield return new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified).AddTicks(1);
            yield return DateTime.MinValue;
            yield return DateTime.MaxValue;
            yield return new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.FromHours(5.5));
            yield return new DateTimeOffset(2020, 1, 2, 3, 4, 5, 600, TimeSpan.Zero);
            yield return TimeSpan.FromMilliseconds(90061001);
            yield return TimeSpan.FromTicks(-1);
            yield return new Guid("d2719f6b-5c8b-4d39-9a0c-0d3d7b0f4f50");
            yield return new Uri("https://example.com/a b?c=d&e=é");
            yield return new Uri("/relative", UriKind.Relative);
            yield return Colour.Blue;
            yield return (Colour)42;
            yield return Rights.Read | Rights.Write;
            yield return new byte[] { 1, 2, 3 };
            yield return new int[0];
            yield return new List<string?> { "a", null };
            yield return new Dictionary<string, int> { ["k \"q\""] = 1 };
            yield return new Nested { Id = 1, Name = null };
            yield return new object();
            yield return (1, "tuple");
        }

        public static IEnumerable<TestCaseData> Cases() => Values().Select((v, i) => new TestCaseData(v).SetName($"Write #{i} {v?.GetType().Name ?? "null"}"));

        [TestCaseSource(nameof(Cases))]
        public void RootValue(object? value)
        {
            Assert.That(NewtonsoftCompat.Serialize(value), Is.EqualTo(N.JsonConvert.SerializeObject(value)));
            Assert.That(NewtonsoftCompat.Serialize(value, ignoreNullValues: true),
                Is.EqualTo(N.JsonConvert.SerializeObject(value, new N.JsonSerializerSettings { NullValueHandling = N.NullValueHandling.Ignore })));
        }

        [TestCaseSource(nameof(Cases))]
        public void PropertyValue(object? value)
        {
            var holder = new Holder<object?> { V = value };
            Assert.That(NewtonsoftCompat.Serialize(holder), Is.EqualTo(N.JsonConvert.SerializeObject(holder)));
        }

        [TestCaseSource(nameof(Cases))]
        public void TypedPropertyValue(object? value)
        {
            if (value == null)
            {
                Assert.Pass();
            }
            var holder = Activator.CreateInstance(typeof(Holder<>).MakeGenericType(value!.GetType()))!;
            holder.GetType().GetProperty("V")!.SetValue(holder, value);
            Assert.That(NewtonsoftCompat.Serialize(holder), Is.EqualTo(N.JsonConvert.SerializeObject(holder)));
        }
    }
}
