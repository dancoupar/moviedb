using Microsoft.EntityFrameworkCore;
using MovieDb.Application.Interfaces;
using MovieDb.Infrastructure.DbContexts;

namespace MovieDb.Infrastructure.Queries
{
	public class GenresQuery(MovieDbContext dbContext) : IGenresQuery
	{
		private readonly MovieDbContext _dbContext = dbContext;

		public async Task<IEnumerable<string>> GetAllGenres()
		{
			return await _dbContext.Genres
				.AsSingleQuery()
				.Select(g => g.Name)
				.Order()
				.ToListAsync();
		}
	}
}
