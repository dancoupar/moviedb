using MovieDb.Application.Models;
using MovieDb.Domain.DataModels;

namespace MovieDb.Application.Interfaces
{
	public interface IMovieSearchQuery
	{
		Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel);

		Task<IEnumerable<Genre>> GetDistinctGenres();
	}
}
