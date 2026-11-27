using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ApiLibs.JSON.Ntfy;
using NUnit.Framework;

namespace ApiLibs.JSONTest.Ntfy
{
    /// <summary>Publishes to a local listener and checks what reached it.</summary>
    public class NtfyServiceTest
    {
        private record Received(string Path, string? Authorization, string Body);

        private static async Task<Received> PublishTo(string pathPrefix, string baseUrlSuffix, string? accessToken)
        {
            int port;
            using (var probe = new TcpListener(IPAddress.Loopback, 0))
            {
                probe.Start();
                port = ((IPEndPoint)probe.LocalEndpoint).Port;
                probe.Stop();
            }

            using var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
            listener.Start();

            var ntfy = new NtfyService($"http://localhost:{port}{pathPrefix}{baseUrlSuffix}", "atlas", accessToken);
            var publish = ntfy.Publish(new NtfyMessage { Title = "Title", Message = "Hello" });

            var context = await listener.GetContextAsync();
            using var reader = new StreamReader(context.Request.InputStream);
            var received = new Received(context.Request.Url!.AbsolutePath, context.Request.Headers["Authorization"], await reader.ReadToEndAsync());
            context.Response.StatusCode = 200;
            context.Response.Close();
            await publish;
            return received;
        }

        [Test]
        public async Task PostsToTheServerRoot()
        {
            var received = await PublishTo("", "/", "tk_token");
            Assert.That(received.Path, Is.EqualTo("/"));
            Assert.That(received.Authorization, Is.EqualTo("Bearer tk_token"));
            StringAssert.Contains("\"topic\":\"atlas\"", received.Body);
            StringAssert.Contains("\"message\":\"Hello\"", received.Body);
            StringAssert.DoesNotContain("priority", received.Body);
        }

        [TestCase("/")]
        [TestCase("")]
        public async Task KeepsThePathOfTheServer(string trailingSlash)
        {
            var received = await PublishTo("/ntfy", trailingSlash, "tk_token");
            Assert.That(received.Path, Is.EqualTo("/ntfy/").Or.EqualTo("/ntfy"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public async Task SendsNoAuthorizationWithoutAToken(string? accessToken)
        {
            var received = await PublishTo("", "/", accessToken);
            Assert.That(received.Authorization, Is.Null);
        }
    }
}
