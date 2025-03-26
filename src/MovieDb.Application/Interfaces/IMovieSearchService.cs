using MovieDb.Application.Models;
using MovieDb.Domain;

namespace MovieDb.Application.Interfaces
{
	public interface IMovieSearchService
	{
		Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel);

		Task<IEnumerable<string>> GetAllGenres();
	}
}