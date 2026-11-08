using System;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.MicrosoftGraph
{
    public class ChangeSubscriptionResult : ValueResult<ChangeSubscription> { }

    public partial class ChangeSubscription
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("resource")]
        public string Resource { get; set; }

        [JsonPropertyName("applicationId")]
        public string ApplicationId { get; set; }

        [JsonPropertyName("changeType")]
        public string ChangeType { get; set; }

        [JsonPropertyName("clientState")]
        public string ClientState { get; set; }

        [JsonPropertyName("notificationUrl")]
        public Uri NotificationUrl { get; set; }

        [JsonPropertyName("lifecycleNotificationUrl")]
        public Uri LifecycleNotificationUrl { get; set; }

        [JsonPropertyName("expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }

        [JsonPropertyName("creatorId")]
        public string CreatorId { get; set; }

        [JsonPropertyName("latestSupportedTlsVersion")]
        public string LatestSupportedTlsVersion { get; set; }

        [JsonPropertyName("encryptionCertificate")]
        public string EncryptionCertificate { get; set; }

        [JsonPropertyName("encryptionCertificateId")]
        public string EncryptionCertificateId { get; set; }

        [JsonPropertyName("includeResourceData")]
        public bool IncludeResourceData { get; set; }
    }
}
