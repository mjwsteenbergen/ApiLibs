using System;
using Newtonsoft.Json;

namespace ApiLibs.MicrosoftGraph
{
    public class ChangeSubscriptionResult : ValueResult<ChangeSubscription> { }

    public partial class ChangeSubscription
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }

        [JsonProperty("changeType")]
        public string ChangeType { get; set; }

        [JsonProperty("clientState")]
        public string ClientState { get; set; }

        [JsonProperty("notificationUrl")]
        public Uri NotificationUrl { get; set; }

        [JsonProperty("lifecycleNotificationUrl")]
        public Uri LifecycleNotificationUrl { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }

        [JsonProperty("creatorId")]
        public string CreatorId { get; set; }

        [JsonProperty("latestSupportedTlsVersion")]
        public string LatestSupportedTlsVersion { get; set; }

        [JsonProperty("encryptionCertificate")]
        public string EncryptionCertificate { get; set; }

        [JsonProperty("encryptionCertificateId")]
        public string EncryptionCertificateId { get; set; }

        [JsonProperty("includeResourceData")]
        public bool IncludeResourceData { get; set; }
    }
}
