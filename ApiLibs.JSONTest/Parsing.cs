using System;
using System.Linq;
using System.Reflection;
using ParityTesting;

namespace ApiLibs.JSONTest
{
    internal sealed class OldDummyService : ApiLibs.Service
    {
        public OldDummyService() : base(null!) { }
    }

    internal sealed class NewDummyService : ApiLibs.JSON.Service
    {
        public NewDummyService() : base(null!) { }
    }

    /// <summary>
    /// Runs JSON text through the production parsing path of both packages
    /// (<c>Service.Convert&lt;R&gt;</c>, which is what every MakeRequest&lt;T&gt; ends in) and compares.
    /// </summary>
    public static class Parsing
    {
        private static readonly MethodInfo OldConvert = typeof(ApiLibs.Service).GetMethod("Convert", BindingFlags.Instance | BindingFlags.NonPublic)!;
        private static readonly MethodInfo NewConvert = typeof(ApiLibs.JSON.Service).GetMethod("Convert", BindingFlags.Instance | BindingFlags.NonPublic)!;
        private static readonly OldDummyService OldService = new();
        private static readonly NewDummyService NewService = new();

        public static object? Old(string text, Type oldType) => Invoke(OldConvert.MakeGenericMethod(oldType), OldService, text);

        public static object? New(string text, Type newType) => Invoke(NewConvert.MakeGenericMethod(newType), NewService, text);

        private static object? Invoke(MethodInfo method, object target, string text)
        {
            try
            {
                return method.Invoke(target, new object[] { text });
            }
            catch (TargetInvocationException e) when (e.InnerException != null)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }

        public static (object? value, Exception? error) Try(Func<object?> f)
        {
            try
            {
                return (f(), null);
            }
            catch (Exception e)
            {
                return (null, e);
            }
        }

        /// <summary>Null when both packages parse <paramref name="json"/> the same (or both fail).</summary>
        public static string? Compare(string json, Type oldType)
        {
            var newType = Apis.NewTypeFor(oldType) ?? throw new InvalidOperationException($"{oldType} has no counterpart in ApiLibs.JSON");
            var (oldValue, oldError) = Try(() => Old(json, oldType));
            var (newValue, newError) = Try(() => New(json, newType));
            return Outcome(oldValue, oldError, newValue, newError);
        }

        public static string? Outcome(object? oldValue, Exception? oldError, object? newValue, Exception? newError)
        {
            if (oldError != null || newError != null)
            {
                if (oldError != null && newError != null)
                {
                    return null;
                }
                return oldError != null
                    ? $"Newtonsoft throws {oldError.GetType().Name} ({First(oldError.Message)}), new returns a value"
                    : $"Newtonsoft returns a value, new throws {newError!.GetType().Name} ({First(newError.Message)})";
            }
            return DeepComparer.Compare(oldValue, newValue);
        }

        private static string First(string message) => message.Split('\n').First();
    }
}
