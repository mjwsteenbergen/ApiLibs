using System;
using System.Collections.Generic;
using ApiLibs.JSON.General;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.MicrosoftGraph
{
    public class AccessTokenObject
    {
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }

    public class ValueResult<T> : ObjectSearcher<GraphService>
    {
        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("@odata.nextLink")]
        public Uri OdataNextLink { get; set; }

        [JsonPropertyName("value")]
        public List<T> Value { get; set; }
    }
}
