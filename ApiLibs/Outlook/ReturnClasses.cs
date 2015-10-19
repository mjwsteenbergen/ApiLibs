using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Outlook
{
    class ReturnClasses
    {
        public class AccessTokenObject
        {
            public string expires_in { get; set; }
            public string token_type { get; set; }
            public string scope { get; set; }
            public string access_token { get; set; }
        }

    }
}
