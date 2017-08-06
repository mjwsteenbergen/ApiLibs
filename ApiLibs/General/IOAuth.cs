using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs
{
    public abstract class IOAuth
    {
        public abstract string RedirectUrl { get; }

        public string ActivateOAuth(string url)
        {
            return ActivateOAuth(new Uri(url));
        }

        public abstract string ActivateOAuth(Uri url);
        public abstract string ActivateOAuth(Uri url, string redirectUrl);

    }
}
