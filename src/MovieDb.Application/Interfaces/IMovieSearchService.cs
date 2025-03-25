using MovieDb.Application.Models;
using MovieDb.Domain.Models;

namespace MovieDb.Application.Interfaces
{
	public interface IMovieSearchService
	{
		Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel);

		Task<IEnumerable<string>> GetDistinctGenres();
	}
}