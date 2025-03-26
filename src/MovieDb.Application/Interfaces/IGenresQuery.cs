namespace MovieDb.Application.Interfaces
{
	public interface IGenresQuery
	{
		Task<IEnumerable<string>> GetAllGenres();
	}
}
