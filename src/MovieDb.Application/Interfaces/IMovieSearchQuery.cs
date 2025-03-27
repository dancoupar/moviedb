using MovieDb.Domain.Models;

namespace MovieDb.Application.Interfaces
{
	/// <summary>
	/// Describes a query for searching a database for movies.
	/// </summary>
	public interface IMovieSearchQuery
	{
		/// <summary>
		/// Queries the database for movies matching the specified search criteria.
		/// </summary>
		/// <param name="searchModel">
		/// An object containing the search criteria along with page and sort information.
		/// </param>
		Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel);
	}
}
