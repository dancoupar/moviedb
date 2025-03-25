namespace MovieDb.Domain.DataModels
{
	public class Genre
	{
		public int Id { get; set; }

		public required string Name { get; set; }

		public List<MovieGenre> MovieGenres { get; set; } = [];
	}
}
