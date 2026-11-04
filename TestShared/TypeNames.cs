using System;
using System.Linq;

namespace ParityTesting
{
    /// <summary>
    /// Type names with the ".JSON" namespace segment removed, so a type from ApiLibs.JSON or
    /// Martijn.Extensions.JSON gets the same name as its counterpart in the Newtonsoft packages.
    /// </summary>
    public static class TypeNames
    {
        public static string Normalize(string name)
        {
            return name
                .Replace("ApiLibs.JSON.", "ApiLibs.")
                .Replace("ApiLibs.JSON", "ApiLibs")
                .Replace("Martijn.Extensions.JSON.", "Martijn.Extensions.")
                .Replace("Martijn.Extensions.JSON", "Martijn.Extensions");
        }

        public static string Of(Type type)
        {
            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            if (type.IsArray)
            {
                return Of(type.GetElementType()!) + "[" + new string(',', type.GetArrayRank() - 1) + "]";
            }

            if (type.IsByRef)
            {
                return Of(type.GetElementType()!) + "&";
            }

            if (type.IsPointer)
            {
                return Of(type.GetElementType()!) + "*";
            }

            string name;
            if (type.IsNested)
            {
                name = Of(type.DeclaringType!.IsGenericTypeDefinition && !type.IsGenericTypeDefinition && type.IsGenericType
                    ? type.DeclaringType
                    : type.DeclaringType) + "+" + type.Name;
            }
            else
            {
                name = (type.Namespace == null ? "" : type.Namespace + ".") + type.Name;
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                name += "[" + string.Join(",", type.GetGenericArguments().Select(Of)) + "]";
            }

            return Normalize(name);
        }
    }
}
