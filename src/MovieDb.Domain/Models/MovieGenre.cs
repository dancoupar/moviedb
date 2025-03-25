using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieDb.Domain.Models
{
	//[Index(nameof(Genre))]
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
