namespace MovieDb.Application.Models
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
		/// The genre(s) of the movie.
		/// </summary>
		public required string Genre { get; init; }
	}
}
