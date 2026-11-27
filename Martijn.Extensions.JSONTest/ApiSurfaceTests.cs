using NUnit.Framework;
using ParityTesting;

namespace Martijn.Extensions.JSONTest
{
    [Category("NoInternet")]
    public class ApiSurfaceTests
    {
        [Test]
        public void PublicApiIsIdentical()
        {
            var diff = ApiSurface.Diff(
                typeof(Martijn.Extensions.Memory.AsyncMemory).Assembly,
                typeof(Martijn.Extensions.JSON.Memory.AsyncMemory).Assembly);

            Assert.That(diff, Is.Empty, "Public API differs:\n" + string.Join("\n", diff));
        }
    }
}
