using System;
using System.Collections.Generic;
using ApiLibs.General;
using Newtonsoft.Json;

namespace ApiLibs.MicrosoftGraph
{
    public class AccessTokenObject
    {
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }

    public class ValueResult<T> : ObjectSearcher
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("@odata.nextLink")]
        public Uri OdataNextLink { get; set; }

        [JsonProperty("value")]
        public List<T> Value { get; set; }
    }
}
