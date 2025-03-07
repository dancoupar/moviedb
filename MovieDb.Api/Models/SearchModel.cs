namespace MovieDb.Api.Models
{
	public class SearchModel
	{
		public required string TitleContains { get; set; }

		public int? MaxNumberOfResults { get; set; }

		public int PageNumber { get; set; }

		public int PageSize { get; set; }

		public string? SortBy { get; set; }

		public bool SortDescending { get; set; }

		public string[]? Genres { get; set; }
	}
}
