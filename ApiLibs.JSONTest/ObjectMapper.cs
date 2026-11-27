using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ApiLibs.JSONTest
{
    /// <summary>
    /// Copies an object built from ApiLibs classes into the equivalent ApiLibs.JSON classes,
    /// field by field (including private and backing fields), without going through JSON.
    /// </summary>
    public static class ObjectMapper
    {
        public static object? ToNew(object? value, Type newType) => Map(value, newType, new Dictionary<object, object>(ReferenceEqualityComparer.Instance));

        private static object? Map(object? value, Type targetType, Dictionary<object, object> seen)
        {
            if (value == null)
            {
                return null;
            }

            var sourceType = value.GetType();
            if (sourceType.Assembly != Apis.Old && !(sourceType.IsGenericType && sourceType.GetGenericArguments().Any(a => a.Assembly == Apis.Old)) && !sourceType.IsArray)
            {
                if (value is not IEnumerable || value is string)
                {
                    return value;
                }
            }

            var runtimeTarget = Apis.NewTypeFor(sourceType) ?? targetType;

            if (runtimeTarget.IsEnum)
            {
                return Enum.ToObject(runtimeTarget, Convert.ChangeType(value, Enum.GetUnderlyingType(sourceType)));
            }

            if (seen.TryGetValue(value, out var done))
            {
                return done;
            }

            if (value is Array array)
            {
                var element = runtimeTarget.GetElementType()!;
                var result = Array.CreateInstance(element, array.Length);
                seen[value] = result;
                for (int i = 0; i < array.Length; i++)
                {
                    result.SetValue(Map(array.GetValue(i), element, seen), i);
                }
                return result;
            }

            if (value is IDictionary dictionary && runtimeTarget.IsGenericType)
            {
                var args = runtimeTarget.GetGenericArguments();
                var result = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(args))!;
                seen[value] = result;
                foreach (DictionaryEntry entry in dictionary)
                {
                    result[Map(entry.Key, args[0], seen)!] = Map(entry.Value, args[1], seen);
                }
                return result;
            }

            if (value is IList list && runtimeTarget.IsGenericType)
            {
                var element = runtimeTarget.GetGenericArguments()[0];
                var result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(element))!;
                seen[value] = result;
                foreach (var item in list)
                {
                    result.Add(Map(item, element, seen));
                }
                return result;
            }

            var target = RuntimeHelpers.GetUninitializedObject(runtimeTarget);
            seen[value] = target;
            for (var (s, t) = (sourceType, runtimeTarget); s != null && t != null && s != typeof(object); s = s.BaseType, t = t.BaseType)
            {
                foreach (var field in s.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    var targetField = t.GetField(field.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    targetField?.SetValue(target, Map(field.GetValue(value), targetField.FieldType, seen));
                }
            }
            return target;
        }
    }
}
