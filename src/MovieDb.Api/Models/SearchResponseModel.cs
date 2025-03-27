using MovieDb.Domain.Models;

namespace MovieDb.Api.Models
{
	/// <summary>
	/// A model for returning search results.
	/// </summary>
	public class SearchResponseModel
	{
		/// <summary>
		/// The page size of the search, as specified in the request.
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// The current page number, as specified in the request.
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		/// The total number of matching movies found by the search.
		/// </summary>
		public int TotalElements { get; set; }

		/// <summary>
		/// The list of search results for the current page.
		/// </summary>
		public required IEnumerable<MovieSearchResult> Content { get; set; }
	}
}
