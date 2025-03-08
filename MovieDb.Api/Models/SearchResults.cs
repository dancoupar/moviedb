namespace MovieDb.Api.Models
{
	public class SearchResults
	{
		public int PageSize { get; set; }

		public int PageNumber { get; set; }

		public int TotalElements { get; set; }

		public required IEnumerable<MovieSearchResult> Content { get; set; }
	}
}
