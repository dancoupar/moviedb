using MovieDb.Application.Models;

namespace MovieDb.Application.Interfaces
{
	/// <summary>
	/// Describes a service for searching for movies.
	/// </summary>
	public interface IMovieSearchService
	{
		/// <summary>
		/// Searches for movies matching the specified search criteria.
		/// </summary>
		/// <param name="searchModel">
		/// An object containing the search criteria along with page and sort information.
		/// </param>
		Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel);

		/// <summary>
		/// Retrieves a list of all movie genres to aid with searching.
		/// </summary>
		Task<IEnumerable<string>> GetAllGenres();
	}
}