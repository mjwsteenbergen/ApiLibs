using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace ParityTesting
{
    /// <summary>
    /// Compares an object graph produced by the Newtonsoft packages with one produced by the
    /// System.Text.Json packages, property by property. Types are matched by name with the
    /// ".JSON" namespace segment removed. Returns null when equal, otherwise the first difference.
    /// </summary>
    public static class DeepComparer
    {
        public static string? Compare(object? expected, object? actual)
        {
            return Compare(expected, actual, "$", 0, new HashSet<(object, object)>(new PairComparer()));
        }

        private static string? Compare(object? a, object? b, string path, int depth, HashSet<(object, object)> visited)
        {
            if (depth > 40)
            {
                return null;
            }

            if (a == null && b == null)
            {
                return null;
            }

            if (a == null || b == null)
            {
                return $"{path}: {Show(a)} vs {Show(b)}";
            }

            if (a is JToken token)
            {
                if (b is System.Text.Json.JsonElement element)
                {
                    return JsonTrees.Equal(token, JToken.Parse(element.GetRawText()))
                        ? null
                        : $"{path}: {token.ToString(Newtonsoft.Json.Formatting.None)} vs {element.GetRawText()}";
                }
                return $"{path}: JToken {token.ToString(Newtonsoft.Json.Formatting.None)} vs {Show(b)} ({b.GetType().Name})";
            }

            Type ta = a.GetType(), tb = b.GetType();
            if (TypeNames.Of(ta) != TypeNames.Of(tb))
            {
                return $"{path}: type {TypeNames.Of(ta)} ({Show(a)}) vs {TypeNames.Of(tb)} ({Show(b)})";
            }

            if (ta.IsEnum)
            {
                var va = Convert.ToDecimal(Convert.ChangeType(a, Enum.GetUnderlyingType(ta)), CultureInfo.InvariantCulture);
                var vb = Convert.ToDecimal(Convert.ChangeType(b, Enum.GetUnderlyingType(tb)), CultureInfo.InvariantCulture);
                return va == vb ? null : $"{path}: {a} ({va}) vs {b} ({vb})";
            }

            switch (a)
            {
                case DateTime da:
                {
                    var db = (DateTime)b;
                    return da.Ticks == db.Ticks && da.Kind == db.Kind ? null : $"{path}: {Show(da)} vs {Show(db)}";
                }
                case DateTimeOffset oa:
                {
                    var ob = (DateTimeOffset)b;
                    return oa.Ticks == ob.Ticks && oa.Offset == ob.Offset ? null : $"{path}: {oa:o} vs {ob:o}";
                }
                case Uri ua:
                    return ua.OriginalString == ((Uri)b).OriginalString ? null : $"{path}: {ua.OriginalString} vs {((Uri)b).OriginalString}";
                case string or decimal or Guid or TimeSpan or BigInteger:
                    return a.Equals(b) ? null : $"{path}: {Show(a)} vs {Show(b)}";
            }

            if (ta.IsPrimitive)
            {
                return a.Equals(b) ? null : $"{path}: {Show(a)} vs {Show(b)}";
            }

            if (a is IDictionary dictA)
            {
                var dictB = (IDictionary)b;
                if (dictA.Count != dictB.Count)
                {
                    return $"{path}: {dictA.Count} entries vs {dictB.Count}";
                }
                foreach (DictionaryEntry entry in dictA)
                {
                    var keyB = dictB.Keys.Cast<object>().FirstOrDefault(k => Compare(entry.Key, k, path, depth + 1, visited) == null);
                    if (keyB == null)
                    {
                        return $"{path}: key {entry.Key} missing";
                    }
                    var diff = Compare(entry.Value, dictB[keyB], $"{path}[{entry.Key}]", depth + 1, visited);
                    if (diff != null)
                    {
                        return diff;
                    }
                }
                return null;
            }

            if (a is IEnumerable enumA && !IsTuple(ta))
            {
                var listA = enumA.Cast<object?>().ToList();
                var listB = ((IEnumerable)b).Cast<object?>().ToList();
                if (listA.Count != listB.Count)
                {
                    return $"{path}: {listA.Count} items vs {listB.Count}";
                }
                for (int i = 0; i < listA.Count; i++)
                {
                    var diff = Compare(listA[i], listB[i], $"{path}[{i}]", depth + 1, visited);
                    if (diff != null)
                    {
                        return diff;
                    }
                }
                return null;
            }

            if (!ta.IsValueType && !visited.Add((a, b)))
            {
                return null;
            }

            foreach (var pa in Properties(ta))
            {
                var pb = Properties(tb).FirstOrDefault(p => p.Name == pa.Name);
                if (pb == null)
                {
                    return $"{path}.{pa.Name}: missing on {TypeNames.Of(tb)}";
                }

                var (va, ea) = Get(() => pa.GetValue(a));
                var (vb, eb) = Get(() => pb.GetValue(b));
                if (ea != null || eb != null)
                {
                    if (ea == null || eb == null)
                    {
                        return $"{path}.{pa.Name}: {(ea == null ? Show(va) : "throws " + ea.GetType().Name)} vs {(eb == null ? Show(vb) : "throws " + eb.GetType().Name)}";
                    }
                    continue;
                }

                var diff = Compare(va, vb, $"{path}.{pa.Name}", depth + 1, visited);
                if (diff != null)
                {
                    return diff;
                }
            }

            foreach (var fa in ta.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var fb = tb.GetField(fa.Name, BindingFlags.Public | BindingFlags.Instance);
                if (fb == null)
                {
                    return $"{path}.{fa.Name}: missing on {TypeNames.Of(tb)}";
                }
                var diff = Compare(fa.GetValue(a), fb.GetValue(b), $"{path}.{fa.Name}", depth + 1, visited);
                if (diff != null)
                {
                    return diff;
                }
            }

            return null;
        }

        private static bool IsTuple(Type t) => t.FullName?.StartsWith("System.ValueTuple") == true || t.FullName?.StartsWith("System.Tuple") == true;

        private static IEnumerable<PropertyInfo> Properties(Type t)
        {
            // Most derived declaration first, so a property hidden with 'new' is read through the hiding one.
            return t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0 && p.GetMethod!.IsPublic)
                .GroupBy(p => p.Name)
                .Select(g => g.OrderByDescending(p => Depth(p.DeclaringType!)).First());
        }

        private static int Depth(Type t)
        {
            int d = 0;
            while (t.BaseType != null)
            {
                d++;
                t = t.BaseType;
            }
            return d;
        }

        private static (object? value, Exception? error) Get(Func<object?> get)
        {
            try
            {
                return (get(), null);
            }
            catch (TargetInvocationException e)
            {
                return (null, e.InnerException ?? e);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public static string Show(object? value)
        {
            return value switch
            {
                null => "null",
                string s => "\"" + s + "\"",
                DateTime d => d.ToString("o", CultureInfo.InvariantCulture) + " (" + d.Kind + ")",
                IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                _ => value.ToString() ?? ""
            };
        }

        private sealed class PairComparer : IEqualityComparer<(object, object)>
        {
            public bool Equals((object, object) x, (object, object) y) => ReferenceEquals(x.Item1, y.Item1) && ReferenceEquals(x.Item2, y.Item2);

            public int GetHashCode((object, object) obj) => HashCode.Combine(RuntimeHelpers.GetHashCode(obj.Item1), RuntimeHelpers.GetHashCode(obj.Item2));
        }
    }

    public static class JsonTrees
    {
        /// <summary>Parses JSON text without turning date-like strings into dates.</summary>
        public static JToken Parse(string json)
        {
            using var reader = new Newtonsoft.Json.JsonTextReader(new System.IO.StringReader(json))
            {
                DateParseHandling = Newtonsoft.Json.DateParseHandling.None,
                FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Decimal,
            };
            return JToken.ReadFrom(reader);
        }

        /// <summary>Structural JSON equality; property order is ignored and numbers compare by value.</summary>
        public static bool Equal(JToken? x, JToken? y)
        {
            return Difference(x, y, "$") == null;
        }

        public static string? Difference(JToken? x, JToken? y, string path)
        {
            if (x == null || y == null)
            {
                return x == y ? null : $"{path}: {x} vs {y}";
            }

            if (IsNumber(x) && IsNumber(y))
            {
                var dx = ToDecimal(x);
                var dy = ToDecimal(y);
                if (dx != null && dy != null)
                {
                    return dx == dy ? null : $"{path}: {x} vs {y}";
                }
                return x.ToString(Newtonsoft.Json.Formatting.None) == y.ToString(Newtonsoft.Json.Formatting.None) ? null : $"{path}: {x} vs {y}";
            }

            if (x.Type != y.Type)
            {
                return $"{path}: {x.ToString(Newtonsoft.Json.Formatting.None)} ({x.Type}) vs {y.ToString(Newtonsoft.Json.Formatting.None)} ({y.Type})";
            }

            switch (x)
            {
                case JObject ox:
                {
                    var oy = (JObject)y;
                    foreach (var p in ox.Properties())
                    {
                        if (!oy.TryGetValue(p.Name, out var other))
                        {
                            return $"{path}.{p.Name}: missing in second";
                        }
                        var d = Difference(p.Value, other, $"{path}.{p.Name}");
                        if (d != null)
                        {
                            return d;
                        }
                    }
                    foreach (var p in oy.Properties())
                    {
                        if (!ox.ContainsKey(p.Name))
                        {
                            return $"{path}.{p.Name}: missing in first";
                        }
                    }
                    return null;
                }
                case JArray ax:
                {
                    var ay = (JArray)y;
                    if (ax.Count != ay.Count)
                    {
                        return $"{path}: {ax.Count} items vs {ay.Count}";
                    }
                    for (int i = 0; i < ax.Count; i++)
                    {
                        var d = Difference(ax[i], ay[i], $"{path}[{i}]");
                        if (d != null)
                        {
                            return d;
                        }
                    }
                    return null;
                }
                default:
                    return JToken.DeepEquals(x, y) ? null : $"{path}: {x.ToString(Newtonsoft.Json.Formatting.None)} vs {y.ToString(Newtonsoft.Json.Formatting.None)}";
            }
        }

        private static bool IsNumber(JToken t) => t.Type is JTokenType.Integer or JTokenType.Float;

        private static decimal? ToDecimal(JToken t)
        {
            try
            {
                return decimal.Parse(t.ToString(Newtonsoft.Json.Formatting.None), NumberStyles.Float, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}
