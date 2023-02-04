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

        public void ActivateOAuth(string url)
        {
            ActivateOAuth(new Uri(url));
        }

        public abstract void ActivateOAuth(Uri url);
        public abstract void ActivateOAuth(Uri url, string redirectUrl);

    }
}
