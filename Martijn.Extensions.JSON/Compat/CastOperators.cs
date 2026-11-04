using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Martijn.Extensions.JSON.Compat
{
    /// <summary>
    /// Newtonsoft converts a JSON primitive into a class or struct through a public
    /// <c>op_Implicit</c>/<c>op_Explicit</c> on the target whose parameter is exactly the CLR type of
    /// the value (bool, long, double, string, DateTime or BigInteger).
    /// </summary>
    internal static class CastOperators
    {
        private static readonly ConcurrentDictionary<Type, bool> HasOperators = new();

        public static bool Has(Type type)
        {
            var target = Nullable.GetUnderlyingType(type) ?? type;
            if (target.IsPrimitive || target.IsEnum || target.Namespace == "System" || target.Namespace?.StartsWith("System.") == true)
            {
                // Framework types (DateTimeOffset has an implicit operator from DateTime) have their own converters.
                return false;
            }
            return HasOperators.GetOrAdd(target, t => t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Any(m => (m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.ReturnType == t && m.GetParameters().Length == 1));
        }

        public static bool IsPrimitiveToken(JsonTokenType token)
            => token is JsonTokenType.String or JsonTokenType.Number or JsonTokenType.True or JsonTokenType.False;

        public static object? Read(ref Utf8JsonReader reader, Type type)
        {
            var target = Nullable.GetUnderlyingType(type) ?? type;
            bool nullable = !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
            bool isString = reader.TokenType == JsonTokenType.String;
            var value = Tokens.ReadValue(ref reader);
            if (value == null || (isString && value is string { Length: 0 } && nullable))
            {
                return nullable ? null : throw new JsonException($"Error converting value {{null}} to type '{type}'.");
            }

            var valueType = value.GetType();
            if (target.IsAssignableFrom(valueType))
            {
                return value;
            }

            var cast = target.GetMethod("op_Implicit", BindingFlags.Public | BindingFlags.Static, null, new[] { valueType }, null)
                       ?? target.GetMethod("op_Explicit", BindingFlags.Public | BindingFlags.Static, null, new[] { valueType }, null);
            if (cast == null || cast.ReturnType != target)
            {
                throw new JsonException($"Could not cast or convert from {valueType} to {target}.");
            }

            try
            {
                return cast.Invoke(null, new[] { value });
            }
            catch (TargetInvocationException e)
            {
                throw new JsonException($"Error converting value {value} to type '{target}'.", e.InnerException);
            }
        }
    }
}
