namespace MovieDb.Api.Models
{
	public class Movie
	{
		public required DateOnly ReleaseDate { get; init; }

		public required string Title { get; init; }

		public required string Overview { get; init; }

		public required decimal Popularity { get; init; }

		public required int VoteCount { get; init; }

		public required decimal VoteAverage { get; init; }

		public required string OriginalLanguage { get; init; }

		public required string Genre { get; init; }

		public required Uri PosterUrl { get; init; }
	}
}
