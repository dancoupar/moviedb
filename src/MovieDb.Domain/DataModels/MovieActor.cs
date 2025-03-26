namespace MovieDb.Domain.DataModels
{
	public class MovieActor
	{
		public int Id { get; set; }

		public int MovieId { get; set; }

		public Movie? Movie { get; set; }

		public Actor? Actor { get; set; }
	}
}