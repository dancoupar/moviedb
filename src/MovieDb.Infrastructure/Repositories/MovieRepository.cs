using Microsoft.EntityFrameworkCore;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Domain.Models;
using MovieDb.Infrastructure.DbContexts;

namespace MovieDb.Infrastructure.Repositories
{
	public class MovieRepository(MovieDbContext dbContext) : IMovieRepository
	{
		private readonly MovieDbContext _dbContext = dbContext;

		public async Task<IEnumerable<Movie>> Search(MovieSearchModel searchModel)
		{
			IQueryable<Movie> query = _dbContext.Movies
				.AsQueryable()
				.Include(m => m.Genres)
				.Where(m => EF.Functions.Like(m.Title, $"%{searchModel.TitleContains}%"));

			if (!string.IsNullOrEmpty(searchModel.ActorContains))
			{
				query = query
					.Include(m => m.Actors)
					.Where(m => m.Actors.Any(a => EF.Functions.Like(a.ActorName, $"%{searchModel.ActorContains}%")));
			}

			if (searchModel.Genres?.Length > 0)
			{
				query = query.Where(m => m.Genres.Select(g => g.Genre).Intersect(searchModel.Genres).Any());
			}

			// Initial sort to ensure deterministic results when search is capped
			query = query.OrderBy(m => m.Id);

			return await query
				.Take(searchModel.MaxNumberOfResults ?? 100)
				.AsSingleQuery()
				.ToListAsync();
		}

		public async Task<IEnumerable<string>> GetDistinctGenres()
		{
			return await _dbContext.MovieGenre
				.Select(g => g.Genre)
				.Distinct()
				.Order()
				.AsSingleQuery()
				.ToListAsync();
		}
	}
}
