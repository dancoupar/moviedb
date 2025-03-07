using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieDb.Api.Entities
{
	[Index(nameof(Id))]
	[Index(nameof(Title))]
	public class Movie
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateOnly ReleaseDate { get; set; }

		public required string Title { get; set; }

		public required string Overview { get; set; }

		public decimal Popularity { get; set; }

		public int VoteCount { get; set; }

		public decimal VoteAverage { get; set; }

		public required string OriginalLanguage { get; set; }

		public required string Genre { get; set; }

		public required Uri PosterUrl { get; set; }
	}
}
