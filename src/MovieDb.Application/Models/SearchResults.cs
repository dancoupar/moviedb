namespace MovieDb.Application.Models
{
	public class SearchResults<T>
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
		public int TotalRecords { get; set; }

		/// <summary>
		/// The list of search results for the current page.
		/// </summary>
		public required IEnumerable<T> Results { get; set; }
	}
}
