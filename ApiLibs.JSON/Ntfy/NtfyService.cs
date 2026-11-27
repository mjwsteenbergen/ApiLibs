using System.Threading.Tasks;

namespace ApiLibs.JSON.Ntfy
{
    /// <summary>
    /// ntfy's JSON publish API (https://docs.ntfy.sh/publish/): one POST to the server's root, with the
    /// topic in the body. Works against ntfy.sh and a self-hosted server, with or without a path prefix.
    /// </summary>
    public class NtfyService : RestSharpService
    {
        private readonly string topic;

        /// <param name="baseUrl">The server, e.g. https://ntfy.sh/. Keep the trailing slash if it has a path.</param>
        /// <param name="topic">Every message goes to this topic.</param>
        /// <param name="accessToken">Sent as a bearer token. Leave null or blank for an open topic.</param>
        public NtfyService(string baseUrl, string topic, string accessToken = null) : base(baseUrl)
        {
            this.topic = topic;
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                AddStandardHeader("Authorization", "Bearer " + accessToken);
            }
        }

        /// <summary>Sets the message's topic and publishes it. Null fields are left out of the body.</summary>
        public virtual Task Publish(NtfyMessage message)
        {
            message.Topic = topic;
            return MakeRequest("", Call.POST, content: message);
        }
    }
}
