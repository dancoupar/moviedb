namespace MovieDb.Domain.DataModels
{
	public class MovieActor
	{
		public int Id { get; set; }

		public int MovieId { get; set; }

		public required string ActorName { get; set; }

		public Movie? Movie { get; set; }
	}
}
