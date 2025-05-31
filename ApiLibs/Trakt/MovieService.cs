using ApiLibs.General;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class MovieService : SubService<TraktService>
    {
        public MovieService(TraktService service) : base(service)
        {
        }

        public Task<Movie> GetMovie(string id) => MakeRequest<Movie>($"/movies/{id}?extended=full");
    }
}
