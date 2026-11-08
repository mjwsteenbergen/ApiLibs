using System;
using System.Linq;
using NUnit.Framework;
using ParityTesting;

namespace ApiLibs.JSONTest
{
    [Category("NoInternet")]
    public class ApiSurfaceTests
    {
        /// <summary>Public types of ApiLibs that are deliberately not in ApiLibs.JSON.</summary>
        private static readonly Type[] NotPorted =
        {
            // Read through the Newtonsoft AsyncMemory and used by no service. Stays in ApiLibs until the final rename.
            typeof(ApiLibs.General.Passwords),
        };

        /// <summary>Services written for ApiLibs.JSON that have no ApiLibs version to match.</summary>
        private static readonly string[] OnlyInNew =
        {
            "ApiLibs.JSON.Yamtrack",
        };

        private static Type Outermost(Type t) => t.DeclaringType == null ? t : Outermost(t.DeclaringType);

        [Test]
        public void PublicApiIsIdentical()
        {
            var diff = ApiSurface.Diff(Apis.Old, Apis.New,
                includeExpected: t => Apis.IsPorted(Outermost(t)) && !NotPorted.Contains(Outermost(t)),
                includeActual: t => !OnlyInNew.Contains(Outermost(t).Namespace));
            Assert.That(diff, Is.Empty, "Public API differs:\n" + string.Join("\n", diff));
        }

        [Test]
        public void PortingProgress()
        {
            var oldNamespaces = Apis.Old.GetTypes().Where(t => t.IsVisible && t.Namespace != null).Select(t => t.Namespace!).Distinct().OrderBy(n => n).ToList();
            var pending = oldNamespaces.Where(n => !Apis.PortedNamespaces.Contains(n)).ToList();
            Assert.Pass($"{oldNamespaces.Count - pending.Count} of {oldNamespaces.Count} namespaces ported. Pending: {string.Join(", ", pending)}");
        }
    }
}
