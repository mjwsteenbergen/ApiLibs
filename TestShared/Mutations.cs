using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ParityTesting
{
    /// <summary>
    /// Mechanical variants of a valid JSON document: every property set to null, every property
    /// removed, every array emptied, and an unknown property added to every object.
    /// </summary>
    public static class Mutations
    {
        public static IEnumerable<(string name, string json)> Of(string json, int maxPerKind = 80)
        {
            var root = JsonTrees.Parse(json);
            yield return ("base", json);

            var properties = AllTokens(root).OfType<JProperty>().Take(maxPerKind).Select(p => p.Path).ToList();

            foreach (var path in properties)
            {
                var copy = root.DeepClone();
                if (copy.SelectToken(Escape(path))?.Parent is JProperty p)
                {
                    p.Value = JValue.CreateNull();
                    yield return ($"null {path}", copy.ToString(Newtonsoft.Json.Formatting.None));
                }
            }

            foreach (var path in properties)
            {
                var copy = root.DeepClone();
                if (copy.SelectToken(Escape(path))?.Parent is JProperty p)
                {
                    p.Remove();
                    yield return ($"remove {path}", copy.ToString(Newtonsoft.Json.Formatting.None));
                }
            }

            var arrays = AllTokens(root).OfType<JArray>().Where(a => a.Count > 0).Take(maxPerKind).Select(a => a.Path).ToList();
            foreach (var path in arrays)
            {
                var copy = root.DeepClone();
                if (copy.SelectToken(Escape(path)) is JArray a)
                {
                    a.RemoveAll();
                    yield return ($"empty {path}", copy.ToString(Newtonsoft.Json.Formatting.None));
                }
            }

            var withUnknown = root.DeepClone();
            foreach (var obj in AllTokens(withUnknown).OfType<JObject>().ToList())
            {
                obj["zz_unknown_property"] = new JObject { ["nested"] = new JArray(1, "two", null, new JObject()) };
            }
            yield return ("unknown properties", withUnknown.ToString(Newtonsoft.Json.Formatting.None));
        }

        private static IEnumerable<JToken> AllTokens(JToken root)
            => root is JContainer container ? container.DescendantsAndSelf() : new[] { root };

        private static string Escape(string path) => path.Length == 0 ? "$" : path;
    }
}
