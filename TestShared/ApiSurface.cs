using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ParityTesting
{
    /// <summary>
    /// Describes the public API of an assembly as a set of strings, with ".JSON" removed from
    /// namespaces, so the Newtonsoft and System.Text.Json versions of a package can be diffed.
    /// </summary>
    public static class ApiSurface
    {
        /// <summary>
        /// Newtonsoft types that are allowed to be replaced by their System.Text.Json counterpart.
        /// Both sides are rewritten to the same placeholder before comparing.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> AllowedTypeSwaps = new Dictionary<string, string>
        {
            ["Newtonsoft.Json.Linq.JToken"] = "<json-value>",
            ["System.Text.Json.JsonElement"] = "<json-value>",
            ["Newtonsoft.Json.JsonConverter"] = "<json-converter>",
            ["System.Text.Json.Serialization.JsonConverter"] = "<json-converter>",
            ["Newtonsoft.Json.JsonConverter`1"] = "<json-converter`1>",
            ["System.Text.Json.Serialization.JsonConverter`1"] = "<json-converter`1>",
        };

        public static SortedSet<string> Describe(Assembly assembly)
        {
            var result = new SortedSet<string>(StringComparer.Ordinal);
            foreach (var type in assembly.GetTypes().Where(t => t.IsVisible))
            {
                foreach (var line in DescribeType(type))
                {
                    result.Add(line);
                }
            }
            return result;
        }

        private static string Name(Type t)
        {
            var name = TypeNames.Of(t);
            foreach (var swap in AllowedTypeSwaps.OrderByDescending(s => s.Key.Length))
            {
                name = name.Replace(swap.Key, swap.Value);
            }
            return name;
        }

        private static bool IsJsonConverter(Type type)
        {
            for (var t = type.BaseType; t != null; t = t.BaseType)
            {
                var name = t.IsGenericType ? t.GetGenericTypeDefinition().FullName : t.FullName;
                if (name is "Newtonsoft.Json.JsonConverter" or "System.Text.Json.Serialization.JsonConverter")
                {
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<string> DescribeType(Type type)
        {
            string typeName = Name(type);
            string kind = type.IsInterface ? "interface"
                : type.IsEnum ? "enum"
                : type.IsValueType ? "struct"
                : typeof(Delegate).IsAssignableFrom(type) ? "delegate"
                : (type.IsAbstract && type.IsSealed) ? "static class"
                : type.IsAbstract ? "abstract class"
                : type.IsSealed ? "sealed class"
                : "class";
            yield return $"{typeName} : {kind}";

            if (type.BaseType != null && type.BaseType != typeof(object) && !type.IsEnum && !type.IsValueType)
            {
                // Converters may change their base class from Newtonsoft's to System.Text.Json's.
                yield return $"{typeName} : base {Name(type.BaseType)}";
            }

            foreach (var i in type.GetInterfaces().Select(Name).OrderBy(n => n))
            {
                yield return $"{typeName} : implements {i}";
            }

            if (type.IsEnum)
            {
                foreach (var name in Enum.GetNames(type))
                {
                    var value = Convert.ChangeType(Enum.Parse(type, name), Enum.GetUnderlyingType(type));
                    yield return $"{typeName}.{name} = {value}";
                }
                yield break;
            }

            bool converter = IsJsonConverter(type);
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

            foreach (var member in type.GetMembers(flags))
            {
                switch (member)
                {
                    case ConstructorInfo c when Visible(c):
                        yield return $"{typeName}..ctor({Params(c)}) {Access(c)}";
                        break;
                    case MethodInfo m when Visible(m) && !m.IsSpecialName:
                        if (converter && m.GetBaseDefinition().DeclaringType != m.DeclaringType)
                        {
                            // ReadJson/WriteJson become Read/Write: the overrides are implementation details.
                            break;
                        }
                        yield return $"{typeName}.{m.Name}{GenericArity(m)}({Params(m)}) : {Name(m.ReturnType)} {Access(m)}{(m.IsStatic ? " static" : "")}{(m.IsAbstract ? " abstract" : m.IsVirtual && !m.IsFinal ? " virtual" : "")}";
                        break;
                    case PropertyInfo p:
                    {
                        var get = p.GetMethod != null && Visible(p.GetMethod) ? "get;" : "";
                        var set = p.SetMethod != null && Visible(p.SetMethod) ? "set;" : "";
                        if (get == "" && set == "")
                        {
                            break;
                        }
                        var accessor = p.GetMethod ?? p.SetMethod!;
                        if (converter && accessor.GetBaseDefinition().DeclaringType != accessor.DeclaringType)
                        {
                            break;
                        }
                        var index = p.GetIndexParameters();
                        var indexText = index.Length == 0 ? "" : "[" + string.Join(", ", index.Select(i => Name(i.ParameterType))) + "]";
                        yield return $"{typeName}.{p.Name}{indexText} : {Name(p.PropertyType)} {{ {get}{set} }}";
                        break;
                    }
                    case FieldInfo f when (f.IsPublic || f.IsFamily || f.IsFamilyOrAssembly) && !f.IsSpecialName:
                        yield return $"{typeName}.{f.Name} : {Name(f.FieldType)} field{(f.IsStatic ? " static" : "")}{(f.IsInitOnly ? " readonly" : "")}{(f.IsLiteral ? " const" : "")}";
                        break;
                    case EventInfo e when e.AddMethod != null && Visible(e.AddMethod):
                        yield return $"{typeName}.{e.Name} : event {Name(e.EventHandlerType!)}";
                        break;
                }
            }
        }

        private static bool Visible(MethodBase m) => m.IsPublic || m.IsFamily || m.IsFamilyOrAssembly;

        private static string Access(MethodBase m) => m.IsPublic ? "public" : "protected";

        private static string GenericArity(MethodInfo m) => m.IsGenericMethodDefinition ? "`" + m.GetGenericArguments().Length : "";

        private static string Params(MethodBase m)
        {
            return string.Join(", ", m.GetParameters().Select(p =>
                (p.IsOut ? "out " : p.ParameterType.IsByRef ? "ref " : "") + Name(p.ParameterType) + (p.HasDefaultValue ? " = " + (p.DefaultValue ?? "null") : "")));
        }

        /// <summary>Lines present in only one of the two descriptions.</summary>
        public static List<string> Diff(Assembly expected, Assembly actual)
        {
            var a = Describe(expected);
            var b = Describe(actual);
            var result = new List<string>();
            result.AddRange(a.Except(b).Select(l => "- " + l));
            result.AddRange(b.Except(a).Select(l => "+ " + l));
            return result;
        }
    }
}
