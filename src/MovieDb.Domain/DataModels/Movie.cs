namespace MovieDb.Domain.DataModels
{
	public class Movie
	{
		public int Id { get; set; }

		public DateOnly ReleaseDate { get; set; }

		public required string Title { get; set; }

		public required string Overview { get; set; }

		public decimal Popularity { get; set; }

		public int VoteCount { get; set; }

		public decimal VoteAverage { get; set; }

		public required string OriginalLanguage { get; set; }

		public required Uri PosterUrl { get; set; }

		public List<MovieGenre> Genres { get; set; } = [];

		public required ICollection<MovieActor> Actors { get; set; }
	}
}
