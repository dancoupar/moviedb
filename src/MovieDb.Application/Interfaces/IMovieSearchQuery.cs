using MovieDb.Application.Models;

namespace MovieDb.Application.Interfaces
{
	public interface IMovieSearchQuery
	{
		Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel);
	}
}
