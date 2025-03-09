using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieDb.Api.Models
{
	public class SearchModel
	{
		[MaxLength(100)]		
		[DisplayName("Title contains")]
		public required string TitleContains { get; set; }

		[MaxLength(100)]
		[DisplayName("Starring")]
		public string? ActorContains { get; set; }

		[Range(1, 100)]
		[DisplayName("Limit results to")]
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
