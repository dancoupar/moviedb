using Microsoft.AspNetCore.Mvc;
using MovieDb.Api.Models;

namespace MovieDb.Api.Controllers
{
	[ApiController]
    public class MovieController(ILogger<MovieController> logger) : ControllerBase
    {
        private readonly ILogger<MovieController> _logger = logger;

        [HttpGet]
        [Route("movies")]
        public IEnumerable<Movie> Search()
        {
            return Enumerable.Empty<Movie>().ToArray();
        }
    }
}
