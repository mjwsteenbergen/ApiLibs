using ApiLibs.JSON.General;
using System.Threading.Tasks;

namespace ApiLibs.JSON.Trakt
{
    public class MovieService : SubService<TraktService>
    {
        public MovieService(TraktService service) : base(service)
        {
        }

        public Task<Movie> GetMovie(string id) => MakeRequest<Movie>($"/movies/{id}?extended=full");
    }
}
