using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class MovieService : SubService
    {
        public MovieService(TraktService service) : base(service)
        {
        }

        public Task<Movie> GetMovie(string id) => MakeRequest<Movie>($"/movies/{id}?extended=full");
    }
}
