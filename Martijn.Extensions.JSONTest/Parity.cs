using System;
using System.Collections.Generic;
using System.Reflection;
using Martijn.Extensions.JSON.Compat;
using ParityTesting;

namespace Martijn.Extensions.JSONTest
{
    /// <summary>Runs the same operation through Newtonsoft.Json and through NewtonsoftCompat.</summary>
    public static class Parity
    {
        public static (object? value, Exception? error) Try(Func<object?> f)
        {
            try
            {
                return (f(), null);
            }
            catch (TargetInvocationException e)
            {
                return (null, e.InnerException);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        public static object? OldRead(string json, Type type) => Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);

        public static object? NewRead(string json, Type type) => NewtonsoftCompat.Deserialize(json, type);

        /// <summary>Null when both sides agree (same value, or both throw), else a description.</summary>
        public static string? CompareRead(string json, Type type, Func<object?, object?>? select = null)
        {
            select ??= v => v;
            var (oldValue, oldError) = Try(() => select(OldRead(json, type)));
            var (newValue, newError) = Try(() => select(NewRead(json, type)));
            if (oldError != null || newError != null)
            {
                if (oldError != null && newError != null)
                {
                    return null;
                }
                return oldError != null
                    ? $"Newtonsoft throws {oldError.GetType().Name} ({oldError.Message}), new returns {DeepComparer.Show(newValue)}"
                    : $"Newtonsoft returns {DeepComparer.Show(oldValue)}, new throws {newError!.GetType().Name} ({newError.Message})";
            }
            return DeepComparer.Compare(oldValue, newValue);
        }

        public static void Collect(List<string> failures, string label, string? difference)
        {
            if (difference != null)
            {
                failures.Add($"{label}: {difference}");
            }
        }
    }
}
