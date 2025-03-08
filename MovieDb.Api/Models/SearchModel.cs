using System.ComponentModel.DataAnnotations;

namespace MovieDb.Api.Models
{
	public class SearchModel
	{
		[MaxLength(100)]
		public required string TitleContains { get; set; }

		[Range(1, 100)]
		public int? MaxNumberOfResults { get; set; }

		[Range(1, 100)]
		public int PageNumber { get; set; }

		[Range(1, 100)]
		public int PageSize { get; set; }

		[AllowedValues(null, "Title", "ReleaseDate")]
		public string? SortBy { get; set; }

		public bool SortDescending { get; set; }

		public string[]? Genres { get; set; }
	}
}
