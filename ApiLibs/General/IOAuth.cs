using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs
{
    public interface IOAuth
    {
        Task<string> ActivateOAuth(Uri url);
    }
}
