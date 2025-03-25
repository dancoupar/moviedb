using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieDb.Domain.DataModels
{
	//[Index(nameof(ActorName))]
	public class MovieActor
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int MovieId { get; set; }

		public required string ActorName { get; set; }

		public Movie? Movie { get; set; }
	}
}
