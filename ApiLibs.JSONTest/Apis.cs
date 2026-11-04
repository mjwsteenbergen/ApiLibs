using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParityTesting;

namespace ApiLibs.JSONTest
{
    /// <summary>The two ApiLibs assemblies and how their types relate.</summary>
    public static class Apis
    {
        public static readonly Assembly Old = typeof(ApiLibs.Service).Assembly;
        public static readonly Assembly New = typeof(ApiLibs.JSON.Service).Assembly;

        private static readonly Dictionary<string, Type> NewTypesByName = New.GetTypes()
            .Where(t => t.FullName != null)
            .GroupBy(t => TypeNames.Normalize(t.FullName!))
            .ToDictionary(g => g.Key, g => g.First());

        private static readonly Dictionary<string, Type> OldTypesByName = Old.GetTypes()
            .Where(t => t.FullName != null)
            .GroupBy(t => t.FullName!)
            .ToDictionary(g => g.Key, g => g.First());

        public static Type? OldTypeFor(Type newType)
        {
            if (newType.IsGenericType && !newType.IsGenericTypeDefinition)
            {
                var definition = OldTypeFor(newType.GetGenericTypeDefinition());
                var args = newType.GetGenericArguments().Select(OldTypeFor).ToArray();
                return definition == null || args.Any(a => a == null) ? null : definition.MakeGenericType(args!);
            }

            if (newType.IsArray)
            {
                return OldTypeFor(newType.GetElementType()!)?.MakeArrayType();
            }

            if (newType.Assembly != New)
            {
                return newType;
            }

            return OldTypesByName.TryGetValue(TypeNames.Normalize(newType.FullName!), out var t) ? t : null;
        }

        /// <summary>Namespaces (in old naming) that have been ported to ApiLibs.JSON.</summary>
        public static readonly HashSet<string> PortedNamespaces = new(New.GetTypes()
            .Where(t => t.Namespace != null)
            .Select(t => TypeNames.Normalize(t.Namespace!)));

        public static bool IsPorted(Type oldType) => oldType.Namespace != null && PortedNamespaces.Contains(oldType.Namespace);

        public static Type? NewTypeFor(Type oldType)
        {
            if (oldType.IsGenericType && !oldType.IsGenericTypeDefinition)
            {
                var definition = NewTypeFor(oldType.GetGenericTypeDefinition());
                var args = oldType.GetGenericArguments().Select(a => NewTypeFor(a)).ToArray();
                return definition == null || args.Any(a => a == null) ? null : definition.MakeGenericType(args!);
            }

            if (oldType.IsArray)
            {
                var element = NewTypeFor(oldType.GetElementType()!);
                return element?.MakeArrayType();
            }

            if (oldType.Assembly != Old)
            {
                return oldType;
            }

            return NewTypesByName.TryGetValue(oldType.FullName!, out var t) ? t : null;
        }

        private static readonly Type[] NotDataTypes =
        {
            typeof(ApiLibs.Service), typeof(Exception), typeof(Delegate), typeof(Attribute), typeof(ApiLibs.Request),
            typeof(ApiLibs.Param), typeof(ApiLibs.RequestResponse), typeof(ApiLibs.ICallImplementation),
            typeof(ApiLibs.RequestContent), typeof(ApiLibs.IOAuth), typeof(Newtonsoft.Json.JsonConverter),
        };

        /// <summary>Classes in ported service namespaces that are (de)serialized from or to JSON.</summary>
        public static IEnumerable<Type> DataTypes()
        {
            return Old.GetTypes()
                .Where(t => t.IsVisible && (t.IsClass || (t.IsValueType && !t.IsEnum)) && !t.IsAbstract && !t.ContainsGenericParameters)
                .Where(t => t.Namespace != null && t.Namespace != "ApiLibs" && t.Namespace != "ApiLibs.General" && t.Namespace.StartsWith("ApiLibs"))
                .Where(t => !NotDataTypes.Any(n => n.IsAssignableFrom(t)))
                .Where(t => !t.Name.StartsWith("<"))
                .Where(IsPorted)
                .OrderBy(t => t.FullName, StringComparer.Ordinal);
        }
    }
}
