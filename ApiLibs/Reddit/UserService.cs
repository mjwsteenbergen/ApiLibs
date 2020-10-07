using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Reddit
{
    public class UserService : SubService<RedditService>
    {
        private readonly string _user;

        public UserService(RedditService service, string user) : base(service)
        {
            _user = user;
        }

        /// <summary>
        /// Get all saved posts by your user
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContentWrapper>> GetSaved(string user = null) => (await MakeRequest<SavedObject>("/user/" + (user ?? _user) + "/saved")).data.children;

    }
}
