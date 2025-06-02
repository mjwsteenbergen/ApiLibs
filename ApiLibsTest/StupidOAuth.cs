using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ApiLibs;

namespace ApiLibsTest
{
    public class StupidOAuth : IOAuth
    {
        public override string RedirectUrl => "";

        public override void ActivateOAuth(Uri url)
        {
            try
            {
                Process.Start(url.AbsoluteUri);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    string rurl = url.AbsoluteUri.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {rurl}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url.AbsoluteUri);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url.AbsoluteUri);
                }
                else
                {
                    throw;
                }
            }
        }

        public override void ActivateOAuth(Uri url, string redirectUrl)
        {
            throw new System.NotImplementedException();
        }
    }
}
