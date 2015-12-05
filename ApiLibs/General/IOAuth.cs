using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs
{
    public interface IOAuth
    {
        string RedirectUrl();
        string ActivateOAuth(Uri url);
        string ActivateOAuth(Uri url, string redirectUrl);

    }
}
