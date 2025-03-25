using MovieDb.Application.Models;
using MovieDb.Domain.Models;

namespace MovieDb.Application.Interfaces
{
	public interface IMovieRepository
	{
		Task<IEnumerable<Movie>> Search(MovieSearchModel searchModel);

		Task<IEnumerable<string>> GetDistinctGenres();
	}
}
