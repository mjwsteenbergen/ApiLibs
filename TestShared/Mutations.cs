using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ParityTesting
{
    /// <summary>
    /// Mechanical variants of a valid JSON document: every property set to null, every property
    /// removed, every array emptied, and an unknown property added to every object.
    /// Tokens are located by their position in document order, so any key works.
    /// </summary>
    public static class Mutations
    {
        public static IEnumerable<(string name, string json)> Of(string json, int maxPerKind = 80)
        {
            var root = JsonTrees.Parse(json);
            yield return ("base", json);

            var propertyCount = Math.Min(AllTokens(root).OfType<JProperty>().Count(), maxPerKind);
            for (int i = 0; i < propertyCount; i++)
            {
                var copy = root.DeepClone();
                var property = AllTokens(copy).OfType<JProperty>().ElementAt(i);
                var path = property.Path;
                property.Value = JValue.CreateNull();
                yield return ($"null {path}", copy.ToString(Newtonsoft.Json.Formatting.None));
            }

            for (int i = 0; i < propertyCount; i++)
            {
                var copy = root.DeepClone();
                var property = AllTokens(copy).OfType<JProperty>().ElementAt(i);
                var path = property.Path;
                property.Remove();
                yield return ($"remove {path}", copy.ToString(Newtonsoft.Json.Formatting.None));
            }

            var arrayCount = Math.Min(AllTokens(root).OfType<JArray>().Count(a => a.Count > 0), maxPerKind);
            for (int i = 0; i < arrayCount; i++)
            {
                var copy = root.DeepClone();
                var array = AllTokens(copy).OfType<JArray>().Where(a => a.Count > 0).ElementAt(i);
                var path = array.Path;
                array.RemoveAll();
                yield return ($"empty {path}", copy.ToString(Newtonsoft.Json.Formatting.None));
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
    }
}
