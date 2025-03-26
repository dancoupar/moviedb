namespace MovieDb.Application.Interfaces
{
	/// <summary>
	/// Describes a query retrieving movie genres from a database.
	/// </summary>
	public interface IGenresQuery
	{
		/// <summary>
		/// Queries the database for a list of all distinct movie genres.
		/// </summary>
		Task<IEnumerable<string>> GetAllGenres();
	}
}
