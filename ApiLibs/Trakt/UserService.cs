using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class UserService : SubService
    {
        public UserService(TraktService service) : base(service)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Possible values movies, shows, seasons or episodes</param>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Task<List<WrappedMediaObject>> GetHistory(string type = null, string id = null, DateTime? start = null, DateTime? end = null)
        {
            string typeUrlPart = "";
            switch(type)
            {
                case "movies":
                    typeUrlPart = "movies/";
                    break;
                case "shows":
                    typeUrlPart = "shows/";
                    break;
                case "seasons":
                    typeUrlPart = "seasons/";
                    break;
                case "episodes":
                    typeUrlPart = "episodes/";
                    break;
                case null:
                    typeUrlPart = "";
                    break;
                default:
                    throw new Exception("Invalid type " + type);
            }
            return MakeRequest<List<WrappedMediaObject>>("sync/history/" + typeUrlPart, Call.GET, new List<Param>
            {
                new OParam("id", id),
                new OParam("start_at", start?.ToString("u")),
                new OParam("end_at", end?.ToString("u"))
            });
        }

        public async Task<Watching> Watching(string id = "me")
        {
            try
            {
                return await MakeRequest<Watching>($"users/{id}/watching/");
            }
            catch (NoContentRequestException)
            {
                return null;
            }
        }
    }
}
