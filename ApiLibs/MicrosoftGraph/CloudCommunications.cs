using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLibs.MicrosoftGraph
{
    public class CloudCommunicationsService : GraphSubService
    {
        public CloudCommunicationsService(GraphService service) : base(service, "beta") { }

        public Task<TeamsMeeting> CreateOnlineMeeting(TeamsMeeting meeting) => MakeRequest<TeamsMeeting>("me/onlineMeetings", Call.POST, content: meeting);

    }
}
