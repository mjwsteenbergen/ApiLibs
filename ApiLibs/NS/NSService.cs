using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.NS
{
    class NSService : Service
    {

        public NSService(string username, string password)
        {
            AddStandardHeader(new Param("CURLOPT_USERPWD", username + ":" + password));
        }
    }
}
