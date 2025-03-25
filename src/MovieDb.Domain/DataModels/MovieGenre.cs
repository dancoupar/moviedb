namespace MovieDb.Domain.DataModels
{
	public class MovieGenre
	{
		public int Id { get; set; }

		public int MovieId { get; set; }

		public required string GenreName { get; set; }

		public int GenreId { get; set; }

		public Movie? Movie { get; set; }

		public Genre? Genre { get; set; }
	}
}
