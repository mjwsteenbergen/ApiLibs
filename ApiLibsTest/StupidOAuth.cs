using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs;

namespace ApiLibsTest
{
    public class StupidOAuth : IOAuth
    {
        public override string RedirectUrl => "";

        public override string ActivateOAuth(Uri url)
        {
            System.Diagnostics.Process.Start(url.ToString());
            return "fail";
        }

        public override string ActivateOAuth(Uri url, string redirectUrl)
        {
            throw new NotImplementedException();
        }
    }
}
