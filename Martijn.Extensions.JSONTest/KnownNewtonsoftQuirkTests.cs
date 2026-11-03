using System;
using System.Collections.Generic;
using Martijn.Extensions.JSON.Compat;
using NUnit.Framework;
using N = Newtonsoft.Json;

namespace Martijn.Extensions.JSONTest
{
    /// <summary>
    /// Newtonsoft behaviour that is arguably wrong but is reproduced on purpose (bug-for-bug
    /// parity). Each test pins the behaviour in both serializers. See QUIRKS.md.
    /// </summary>
    [Category("NoInternet")]
    public class KnownNewtonsoftQuirkTests
    {
        private static T Old<T>(string json) => N.JsonConvert.DeserializeObject<T>(json)!;

        private static T New<T>(string json) => NewtonsoftCompat.Deserialize<T>(json);

        private static void Both<T>(string json, Action<T> assert)
        {
            assert(Old<T>(json));
            assert(New<T>(json));
        }

        [Test]
        public void KnownNewtonsoftQuirk_FractionsAreRoundedIntoLongButRejectedForInt()
        {
            Both<Holder<long>>("{\"V\":2.5}", h => Assert.That(h.V, Is.EqualTo(2)));
            Both<Holder<long>>("{\"V\":3.5}", h => Assert.That(h.V, Is.EqualTo(4)));
            Assert.Throws<N.JsonReaderException>(() => Old<Holder<int>>("{\"V\":2.5}"));
            Assert.Throws<System.Text.Json.JsonException>(() => New<Holder<int>>("{\"V\":2.5}"));
        }

        [Test]
        public void KnownNewtonsoftQuirk_BooleansBecomeNumbersForLongButNotForInt()
        {
            Both<Holder<long>>("{\"V\":true}", h => Assert.That(h.V, Is.EqualTo(1)));
            Assert.Throws<N.JsonReaderException>(() => Old<Holder<int>>("{\"V\":true}"));
            Assert.Throws<System.Text.Json.JsonException>(() => New<Holder<int>>("{\"V\":true}"));
        }

        [Test]
        public void KnownNewtonsoftQuirk_EnumNamesAreCombinedEvenWithoutFlags()
        {
            Both<Holder<Colour>>("{\"V\":\"Green, Blue\"}", h => Assert.That((int)h.V, Is.EqualTo(5 | 7)));
            Both<Holder<Colour>>("{\"V\":true}", h => Assert.That((int)h.V, Is.EqualTo(1)));
            Both<Holder<Colour>>("{\"V\":42}", h => Assert.That((int)h.V, Is.EqualTo(42)));
        }

        [Test]
        public void KnownNewtonsoftQuirk_DateOffsetsAreConvertedToLocalTime()
        {
            Both<Holder<DateTime>>("{\"V\":\"2020-07-01T10:00:00+05:00\"}", h =>
            {
                Assert.That(h.V.Kind, Is.EqualTo(DateTimeKind.Local));
                Assert.That(h.V, Is.EqualTo(new DateTime(2020, 7, 1, 5, 0, 0, DateTimeKind.Utc).ToLocalTime()));
            });
            Both<Holder<object>>("{\"V\":\"2020-07-01T10:00:00Z\"}", h => Assert.That(h.V, Is.TypeOf<DateTime>()));
            Both<Holder<object>>("{\"V\":\"2020-07-01\"}", h => Assert.That(h.V, Is.TypeOf<string>()));
        }

        [Test]
        public void KnownNewtonsoftQuirk_EmptyStringIsNullForObjectsButAnErrorForNumbers()
        {
            Both<Holder<Nested>>("{\"V\":\"\"}", h => Assert.That(h.V, Is.Null));
            Both<Holder<List<int>>>("{\"V\":\"\"}", h => Assert.That(h.V, Is.Null));
            Both<Holder<int?>>("{\"V\":\"\"}", h => Assert.That(h.V, Is.Null));
            Assert.Throws<N.JsonSerializationException>(() => Old<Holder<int>>("{\"V\":\"\"}"));
            Assert.Throws<System.Text.Json.JsonException>(() => New<Holder<int>>("{\"V\":\"\"}"));
        }

        [Test]
        public void KnownNewtonsoftQuirk_NullValueHandlingIgnoreAlsoIgnoresNullsWhenReading()
        {
            Both<Attributed>("{\"count\":null,\"label\":null}", a =>
            {
                Assert.That(a.Count, Is.EqualTo(5));
                Assert.That(a.Label, Is.EqualTo("default"));
            });
        }

        [Test]
        public void KnownNewtonsoftQuirk_StringsKeepTheRawTextOfNumbersAndBooleans()
        {
            Both<Holder<string>>("{\"V\":1.50}", h => Assert.That(h.V, Is.EqualTo("1.50")));
            Both<Holder<string>>("{\"V\":1e2}", h => Assert.That(h.V, Is.EqualTo("1e2")));
            Both<Holder<string>>("{\"V\":true}", h => Assert.That(h.V, Is.EqualTo("true")));
        }

        [Test]
        public void KnownNewtonsoftQuirk_WholeDoublesAreWrittenWithDecimalPlace()
        {
            Assert.That(N.JsonConvert.SerializeObject(1.0), Is.EqualTo("1.0"));
            Assert.That(NewtonsoftCompat.Serialize(1.0), Is.EqualTo("1.0"));
            Assert.That(NewtonsoftCompat.Serialize(double.NaN), Is.EqualTo("\"NaN\""));
        }
    }
}
