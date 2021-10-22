using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.GitHub
{
    public class ActivityService : SubService<GitHubService>
    {
        public ActivityService(GitHubService service) : base(service)
        {
        }

        /// <summary>
        /// WARNING: DOES NOT WORK
        /// </summary>
        /// <param name="notification"></param>
        public void MarkNotificationRead(NotificationsObject notification)
        {
            //await MakeRequest("")
        }

        /// <summary>
        /// WARNING: DOES NOT WORK
        /// </summary>
        /// <returns></returns>
        public async Task MarkNotificationsReadRepo(NotificationsObject notification)
        {
            await MakeRequest<NotificationsObject>(notification.repository.NotificationsUrl, Call.PUT, new List<Param>());
        }

        /// <summary>
        /// WARNING: DOES NOT WORK
        /// </summary>
        /// <returns></returns>
        public async Task MarkNotificationsRead(NotificationsObject notification)
        {
            await MakeRequest<NotificationsObject>(notification.url, Call.PATCH, new List<Param>());
        }

        public async Task<List<NotificationsObject>> GetNotifications()
        {
            //new Param("all","true") //TODO
            var parameters = new List<Param> { };
            var res = await MakeRequest<List<NotificationsObject>>("notifications", parameters: parameters);
            return res;
        }


        public async Task<List<Event>> GetEvents(string owner, string repo, int issueNumber)
        {
            return await MakeRequest<List<Event>>("/repos/" + owner + "/" + repo + "/issues/" + issueNumber + "/events");
        }
    }
}
