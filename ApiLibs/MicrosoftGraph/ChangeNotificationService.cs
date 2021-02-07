using System.Threading.Tasks;

namespace ApiLibs.MicrosoftGraph
{
    public class ChangeNotificationService : GraphSubService
    {
        public ChangeNotificationService(GraphService service) : base(service, "v1.0")
        {
        }

        public Task<ChangeSubscriptionResult> Get() => MakeRequest<ChangeSubscriptionResult>("subscriptions");
        public Task<ChangeSubscription> Get(string id) => MakeRequest<ChangeSubscription>("subscriptions");
        public Task<ChangeSubscription> Create(ChangeSubscription sub) => MakeRequest<ChangeSubscription>("subscriptions", Call.POST, content: sub, statusCode: System.Net.HttpStatusCode.Created);
        public Task<ChangeSubscription> Update(ChangeSubscription sub) => MakeRequest<ChangeSubscription>("subscriptions/" + sub.Id, Call.PATCH, content: sub);
    }
}
