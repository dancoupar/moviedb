using MovieDb.Application.Models;

namespace MovieDb.Application.Interfaces
{
	public interface IMovieSearchService
	{
		Task<IEnumerable<MovieSearchResult>> SearchMovies(MovieSearchModel searchModel);

		Task<IEnumerable<string>> GetDistinctGenres();
	}
}