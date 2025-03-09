using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieDb.Api.Entities
{
	[Index(nameof(Genre))]
	public class MovieGenre
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int MovieId { get; set; }

		public required string Genre { get; set; }

		public Movie? Movie { get; set; }
	}
}
