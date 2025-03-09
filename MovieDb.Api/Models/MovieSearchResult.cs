namespace MovieDb.Api.Models
{
	public class MovieSearchResult
	{
		/// <summary>
		/// The unique ID of the movie.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The original release date.
		/// </summary>
		public required DateOnly ReleaseDate { get; init; }

		/// <summary>
		/// The title of the movie.
		/// </summary>
		public required string Title { get; init; }

		/// <summary>
		/// An overview/synopsis of the movie.
		/// </summary>
		public required string Overview { get; init; }

		/// <summary>
		/// A metric indicating the popularity of the movie.
		/// </summary>
		public required decimal Popularity { get; init; }

		/// <summary>
		/// The overall number of votes the movie received.
		/// </summary>
		public required int VoteCount { get; init; }

		/// <summary>
		/// The average of all votes.
		/// </summary>
		public required decimal VoteAverage { get; init; }

		/// <summary>
		/// The original language of the movie.
		/// </summary>
		public required string OriginalLanguage { get; init; }

		/// <summary>
		/// The genre(s) of the movie.
		/// </summary>
		public required string Genre { get; init; }

		/// <summary>
		/// The URL to an image of the movie's poster.
		/// </summary>
		public required Uri PosterUrl { get; init; }
	}
}
