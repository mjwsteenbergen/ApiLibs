using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using ParityTesting;

namespace ApiLibs.JSONTest
{
    /// <summary>What a service sent, reduced to comparable text.</summary>
    public record SentRequest(string EndPoint, string Method, string Parameters, string Headers, string Content)
    {
        public override string ToString() => $"{Method} {EndPoint}\n        params: {Parameters}\n        headers: {Headers}\n        content: {Content}";
    }

    /// <summary>
    /// Stand-in for the HTTP layer. Records every request and answers with JSON generated for the
    /// type the request expects, built from the old (Newtonsoft) class with a fixed seed, so the
    /// old and new service receive identical response text.
    /// </summary>
    public sealed class FakeHttp
    {
        public const int MaxCalls = 4;

        private readonly int seed;
        public readonly List<SentRequest> Sent = new();

        public FakeHttp(int seed)
        {
            this.seed = seed;
        }

        public string ResponseFor(Type? expectedType, Func<Type, Type?> toOldType)
        {
            if (Sent.Count > MaxCalls)
            {
                throw new TooManyCallsException();
            }

            if (expectedType == null)
            {
                return "";
            }

            var oldType = toOldType(expectedType);
            if (oldType == typeof(string))
            {
                return "response text " + Sent.Count;
            }

            try
            {
                var fixture = GeneratedParityTests.CreateFixture(seed * 100 + Sent.Count);
                return Newtonsoft.Json.JsonConvert.SerializeObject(SeededFixture.CreateInstance(fixture, oldType ?? typeof(object)));
            }
            catch (Exception)
            {
                return "{}";
            }
        }

        public static HttpStatusCode Expected(OneOf.OneOf<HttpStatusCode, IEnumerable<HttpStatusCode>> expected)
            => expected.Match(code => code, codes => codes.First());
    }

    public sealed class TooManyCallsException : Exception
    {
    }

    public sealed class OldFakeImplementation : ApiLibs.ICallImplementation
    {
        public readonly FakeHttp Http;

        public OldFakeImplementation(FakeHttp http)
        {
            Http = http;
        }

        public override Task<ApiLibs.RequestResponse> ExecuteRequest(ApiLibs.Service service, ApiLibs.Request request)
        {
            Http.Sent.Add(new SentRequest(
                request.EndPoint,
                request.Method.ToString(),
                string.Join(" & ", request.Parameters.Select(p => $"{p.GetType().Name}:{p.Name}={p.Value}")),
                string.Join(" & ", request.Headers.Select(p => $"{p.GetType().Name}:{p.Name}={p.Value}")),
                Body(request.Content)));

            var expected = request.GetType().IsGenericType ? request.GetType().GetGenericArguments()[0] : null;
            var content = Http.ResponseFor(expected, t => t);
            return Task.FromResult(new ApiLibs.RequestResponse(FakeHttp.Expected(request.ExpectedStatusCode), "Fake", request.EndPoint, null, content, null, request, service));
        }

        private static string Body(object? content) => content switch
        {
            null => "",
            string s => "text:" + s,
            ApiLibs.TextRequestContent t => $"{t.GetType().Name}:{t.ContentType}:{t.Content}",
            ApiLibs.FileByteRequestContent f => $"file:{f.Name}:{f.ContentType}:{System.Convert.ToBase64String(f.Bytes)}",
            ApiLibs.FileStreamRequestContent f => $"stream:{f.Name}:{f.Filename}:{f.ContentType}",
            _ => "json:" + Newtonsoft.Json.JsonConvert.SerializeObject(content, new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }),
        };
    }

    public sealed class NewFakeImplementation : ApiLibs.JSON.ICallImplementation
    {
        public readonly FakeHttp Http;

        public NewFakeImplementation(FakeHttp http)
        {
            Http = http;
        }

        public override Task<ApiLibs.JSON.RequestResponse> ExecuteRequest(ApiLibs.JSON.Service service, ApiLibs.JSON.Request request)
        {
            Http.Sent.Add(new SentRequest(
                request.EndPoint,
                request.Method.ToString(),
                string.Join(" & ", request.Parameters.Select(p => $"{p.GetType().Name}:{p.Name}={p.Value}")),
                string.Join(" & ", request.Headers.Select(p => $"{p.GetType().Name}:{p.Name}={p.Value}")),
                Body(request.Content)));

            var expected = request.GetType().IsGenericType ? request.GetType().GetGenericArguments()[0] : null;
            var content = Http.ResponseFor(expected, Apis.OldTypeFor);
            return Task.FromResult(new ApiLibs.JSON.RequestResponse(FakeHttp.Expected(request.ExpectedStatusCode), "Fake", request.EndPoint, null, content, null, request, service));
        }

        private static string Body(object? content) => content switch
        {
            null => "",
            string s => "text:" + s,
            ApiLibs.JSON.TextRequestContent t => $"{t.GetType().Name}:{t.ContentType}:{t.Content}",
            ApiLibs.JSON.FileByteRequestContent f => $"file:{f.Name}:{f.ContentType}:{System.Convert.ToBase64String(f.Bytes)}",
            ApiLibs.JSON.FileStreamRequestContent f => $"stream:{f.Name}:{f.Filename}:{f.ContentType}",
            _ => "json:" + Martijn.Extensions.JSON.Compat.NewtonsoftCompat.Serialize(content, ignoreNullValues: true),
        };
    }

    /// <summary>
    /// Calls every public method of every ported service in both packages with the same
    /// arguments and the same canned responses, and requires the same requests to be sent and
    /// the same results (or both an exception) to come back.
    /// </summary>
    [Category("NoInternet")]
    public class ServiceParityTests
    {
        /// <summary>Methods that bypass ApiLibs' HTTP layer and would reach the internet.</summary>
        private static readonly HashSet<string> TalksToTheInternetDirectly = new()
        {
            "ApiLibs.Instapaper.InstapaperService.Connect",
        };

        private const int Seeds = 2;

        public static IEnumerable<TestCaseData> Services() => Apis.Old.GetTypes()
            .Where(t => t.IsVisible && !t.IsAbstract && typeof(ApiLibs.Service).IsAssignableFrom(t) && Apis.IsPorted(t))
            .Where(t => !t.IsGenericTypeDefinition && t != typeof(ApiLibs.BlandService))
            .Where(t => !IsSubService(t))
            .OrderBy(t => t.FullName)
            .Select(t => new TestCaseData(t).SetName(TypeNames.Of(t).Replace("ApiLibs.", "")));

        public static bool IsSubService(Type t)
        {
            for (var b = t.BaseType; b != null; b = b.BaseType)
            {
                if (b.IsGenericType && b.GetGenericTypeDefinition() == typeof(ApiLibs.General.SubService<>))
                {
                    return true;
                }
            }
            return false;
        }

        [TestCaseSource(nameof(Services))]
        public async Task SameRequestsAndResults(Type oldServiceType)
        {
            var newServiceType = Apis.NewTypeFor(oldServiceType)!;
            var failures = new List<string>();
            int methodsRun = 0;
            var skipped = new List<string>();

            // Methods of the service itself and of its sub-services (exposed as properties).
            var targets = PublicMethods(oldServiceType).Select(m => ((PropertyInfo?)null, m)).ToList();
            foreach (var property in oldServiceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (typeof(ApiLibs.Service).IsAssignableFrom(property.PropertyType) && property.GetIndexParameters().Length == 0 && IsSubService(property.PropertyType))
                {
                    targets.AddRange(PublicMethods(property.PropertyType).Select(m => ((PropertyInfo?)property, m)));
                }
            }

            foreach (var (subServiceProperty, oldMethod) in targets)
            {
                var key = $"{oldMethod.DeclaringType!.FullName}.{oldMethod.Name}";
                if (TalksToTheInternetDirectly.Contains(key))
                {
                    skipped.Add(oldMethod.Name + " (talks to the internet)");
                    continue;
                }

                var newTargetType = subServiceProperty == null ? newServiceType : newServiceType.GetProperty(subServiceProperty.Name)!.PropertyType;
                var newMethod = Counterpart(newTargetType, oldMethod);
                if (newMethod == null)
                {
                    failures.Add($"{oldMethod}: no counterpart");
                    continue;
                }

                for (int seed = 0; seed < Seeds; seed++)
                {
                    var (oldArgs, newArgs, problem) = Arguments(oldMethod, seed);
                    if (problem != null)
                    {
                        skipped.Add($"{oldMethod.Name} ({problem})");
                        break;
                    }

                    var oldHttp = new FakeHttp(seed);
                    var newHttp = new FakeHttp(seed);
                    var oldService = CreateService(oldServiceType, seed, s => Inject(s, typeof(ApiLibs.Service), new OldFakeImplementation(oldHttp)));
                    var newService = CreateService(newServiceType, seed, s => Inject(s, typeof(ApiLibs.JSON.Service), new NewFakeImplementation(newHttp)));

                    object oldTarget = subServiceProperty == null ? oldService : subServiceProperty.GetValue(oldService)!;
                    object newTarget = subServiceProperty == null ? newService : newService.GetType().GetProperty(subServiceProperty.Name)!.GetValue(newService)!;
                    var (oldResult, oldError) = await Call(oldMethod, oldTarget, oldArgs!);
                    var (newResult, newError) = await Call(newMethod, newTarget, newArgs!);
                    methodsRun++;

                    var label = $"{subServiceProperty?.Name}{(subServiceProperty == null ? "" : ".")}{oldMethod.Name}({string.Join(", ", oldMethod.GetParameters().Select(p => p.ParameterType.Name))}) seed {seed}";
                    if (oldHttp.Sent.Count != newHttp.Sent.Count)
                    {
                        failures.Add($"{label}: {oldHttp.Sent.Count} requests vs {newHttp.Sent.Count}");
                    }
                    for (int i = 0; i < Math.Min(oldHttp.Sent.Count, newHttp.Sent.Count); i++)
                    {
                        var difference = CompareRequests(oldHttp.Sent[i], newHttp.Sent[i]);
                        if (difference != null)
                        {
                            failures.Add($"{label} request {i}: {difference}\n      old: {oldHttp.Sent[i]}\n      new: {newHttp.Sent[i]}");
                        }
                    }

                    var resultDifference = Parsing.Outcome(oldResult, oldError, newResult, newError);
                    if (resultDifference != null)
                    {
                        failures.Add($"{label} result: {resultDifference}" +
                                     (oldError != null ? $"\n      old error: {oldError}" : "") +
                                     (newError != null ? $"\n      new error: {newError}" : ""));
                    }
                }
            }

            TestContext.WriteLine($"{methodsRun} method calls compared. Skipped: {string.Join("; ", skipped)}");
            Assert.That(failures, Is.Empty, string.Join("\n", failures.Take(12)));
        }

        public static IEnumerable<MethodInfo> PublicMethods(Type serviceType)
        {
            var stop = new HashSet<Type> { typeof(ApiLibs.Service), typeof(ApiLibs.RestSharpService), typeof(object) };
            return serviceType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => !stop.Contains(m.DeclaringType!) && !(m.DeclaringType!.IsGenericType && m.DeclaringType.GetGenericTypeDefinition() == typeof(ApiLibs.General.SubService<>)))
                .Where(m => !m.IsSpecialName && !m.IsGenericMethodDefinition)
                .OrderBy(m => m.Name).ThenBy(m => m.ToString());
        }

        private static MethodInfo? Counterpart(Type newServiceType, MethodInfo oldMethod)
        {
            var oldParams = oldMethod.GetParameters().Select(p => TypeNames.Of(p.ParameterType)).ToList();
            return newServiceType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == oldMethod.Name && m.GetParameters().Select(p => TypeNames.Of(p.ParameterType)).SequenceEqual(oldParams));
        }

        private static object CreateService(Type type, int seed, Action<object> inject)
        {
            var constructor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
            var fixture = GeneratedParityTests.CreateFixture(SeededFixture.Seed + seed);
            var args = constructor.GetParameters().Select(p => p.ParameterType == typeof(string) ? $"{p.Name}-value" : SeededFixture.CreateInstance(fixture, p.ParameterType)).ToArray();
            var service = constructor.Invoke(args);
            inject(service);
            return service;
        }

        private static void Inject(object service, Type serviceBase, object implementation)
        {
            var field = serviceBase.GetField("<Implementation>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
            field.SetValue(service, implementation);

            // Sub-services created in the constructor copied the original implementation.
            foreach (var property in service.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (serviceBase.IsAssignableFrom(property.PropertyType) && property.GetIndexParameters().Length == 0)
                {
                    var sub = property.GetValue(service);
                    if (sub != null && sub != service)
                    {
                        field.SetValue(sub, implementation);
                    }
                }
            }
        }

        private static (object?[]? oldArgs, object?[]? newArgs, string? problem) Arguments(MethodInfo oldMethod, int seed)
        {
            var fixture = GeneratedParityTests.CreateFixture(SeededFixture.Seed + 1000 + seed);
            var oldArgs = new List<object?>();
            var newArgs = new List<object?>();
            foreach (var parameter in oldMethod.GetParameters())
            {
                var type = parameter.ParameterType;
                if (typeof(Delegate).IsAssignableFrom(type))
                {
                    return (null, null, "takes a delegate");
                }
                if (type == typeof(CancellationToken))
                {
                    oldArgs.Add(CancellationToken.None);
                    newArgs.Add(CancellationToken.None);
                    continue;
                }

                object? oldArg;
                try
                {
                    oldArg = SeededFixture.CreateInstance(fixture, type);
                    oldArg = Materialize(oldArg, type);
                }
                catch (Exception e)
                {
                    return (null, null, $"cannot build {type.Name}: {e.GetType().Name}");
                }

                var newType = Apis.NewTypeFor(type);
                if (newType == null)
                {
                    return (null, null, $"no counterpart for {type.Name}");
                }

                object? newArg = oldArg;
                if (newType != type)
                {
                    newArg = ObjectMapper.ToNew(oldArg, newType);
                    var difference = DeepComparer.Compare(oldArg, newArg);
                    if (difference != null)
                    {
                        return (null, null, $"argument {parameter.Name} does not round-trip: {difference}");
                    }
                }

                oldArgs.Add(oldArg);
                newArgs.Add(newArg);
            }
            return (oldArgs.ToArray(), newArgs.ToArray(), null);
        }

        /// <summary>AutoFixture builds IEnumerable&lt;T&gt; lazily, with new items on every enumeration.</summary>
        private static object? Materialize(object? value, Type parameterType)
        {
            if (value is IEnumerable enumerable && value is not string && parameterType.IsInterface && parameterType.IsGenericType)
            {
                var element = parameterType.GetGenericArguments()[0];
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(element))!;
                foreach (var item in enumerable)
                {
                    list.Add(item);
                }
                return list;
            }
            return value;
        }

        /// <summary>Reads an IAsyncEnumerable&lt;T&gt; into a list (at most 50 items).</summary>
        private static async Task<object?> Drain(object value)
        {
            var asyncEnumerable = value.GetType().GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>));
            if (asyncEnumerable == null)
            {
                return value;
            }

            var element = asyncEnumerable.GetGenericArguments()[0];
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(element))!;
            var enumerator = asyncEnumerable.GetMethod("GetAsyncEnumerator")!.Invoke(value, new object[] { CancellationToken.None })!;
            var enumeratorType = typeof(IAsyncEnumerator<>).MakeGenericType(element);
            var moveNext = enumeratorType.GetMethod("MoveNextAsync")!;
            var current = enumeratorType.GetProperty("Current")!;
            while (list.Count < 50)
            {
                var step = (ValueTask<bool>)moveNext.Invoke(enumerator, null)!;
                if (!await step)
                {
                    break;
                }
                list.Add(current.GetValue(enumerator));
            }
            return list;
        }

        private static async Task<(object? result, Exception? error)> Call(MethodInfo method, object service, object?[] args)
        {
            try
            {
                var value = method.Invoke(service, args);
                if (value is Task task)
                {
                    var finished = await Task.WhenAny(task, Task.Delay(TimeSpan.FromSeconds(20)));
                    if (finished != task)
                    {
                        return (null, new TimeoutException("Method did not finish"));
                    }
                    await task;
                    var resultProperty = task.GetType().GetProperty("Result");
                    var result = resultProperty?.GetValue(task);
                    return (result?.GetType().FullName == "System.Threading.Tasks.VoidTaskResult" ? null : result, null);
                }
                return (value == null ? null : await Drain(value), null);
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

        /// <summary>
        /// Some services send the current time (Pocket sends DateTime.Now.ToFileTime()), which
        /// differs between the two runs. Runs of 15 or more digits are treated as equal.
        /// </summary>
        private static string WithoutClockTicks(string text) => System.Text.RegularExpressions.Regex.Replace(text, @"\d{15,}", "#");

        private static string? CompareRequests(SentRequest oldRequest, SentRequest newRequest)
        {
            if (oldRequest.EndPoint != newRequest.EndPoint)
            {
                return "endpoint";
            }
            if (oldRequest.Method != newRequest.Method)
            {
                return "method";
            }
            if (WithoutClockTicks(oldRequest.Parameters) != WithoutClockTicks(newRequest.Parameters))
            {
                return "parameters";
            }
            if (WithoutClockTicks(oldRequest.Headers) != WithoutClockTicks(newRequest.Headers))
            {
                return "headers";
            }
            if (oldRequest.Content.StartsWith("json:") && newRequest.Content.StartsWith("json:"))
            {
                return JsonTrees.Difference(JsonTrees.Parse(oldRequest.Content.Substring(5)), JsonTrees.Parse(newRequest.Content.Substring(5)), "$");
            }
            return oldRequest.Content == newRequest.Content ? null : "content";
        }
    }
}
