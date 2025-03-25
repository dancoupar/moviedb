namespace MovieDb.Domain.DataModels
{
	public class MovieGenre
	{
		public int Id { get; set; }

		public int MovieId { get; set; }

		public required string Genre { get; set; }

		public Movie? Movie { get; set; }
	}
}
