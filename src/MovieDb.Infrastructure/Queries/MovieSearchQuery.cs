using Microsoft.EntityFrameworkCore;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Domain.DataModels;
using MovieDb.Infrastructure.DbContexts;
using System.Linq.Expressions;

namespace MovieDb.Infrastructure.Queries
{
	public class MovieSearchQuery(MovieDbContext dbContext) : IMovieSearchQuery
	{
		private readonly MovieDbContext _dbContext = dbContext;

		public async Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel)
		{
			ArgumentNullException.ThrowIfNull(searchModel, nameof(searchModel));

			IQueryable<Movie> query = _dbContext.Movies
				.Include(m => m.Genres)
				.ThenInclude(m => m.Genre)
				.Where(m => EF.Functions.Like(m.Title, $"%{searchModel.TitleContains}%"));

			if (!string.IsNullOrEmpty(searchModel.ActorContains))
			{
				query = query
					.Include(m => m.Actors)
					.Where(m => m.Actors.Any(a => EF.Functions.Like(a.ActorName, $"%{searchModel.ActorContains}%")));
			}

			if (searchModel.Genres?.Length > 0)
			{
				query = query.Where(m => m.Genres.Select(g => g.Genre == null ? null : g.Genre.Name).Intersect(searchModel.Genres).Any());
			}

			var totalRecords = await query.CountAsync();
			
			query = ApplySort(query, searchModel.SortBy, searchModel.SortDescending);

			IEnumerable<MovieSearchResult> results = await query
				.Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
				.Take(searchModel.PageSize)
				.Select(m => new MovieSearchResult()
				{
					Id = m.Id,
					Title = m.Title,
					ReleaseDate = m.ReleaseDate,
					Genre = string.Join(", ", m.Genres.Select(g => g.Genre != null ? g.Genre.Name : string.Empty))
				})
				.ToListAsync();

			return new SearchResults<MovieSearchResult>()
			{
				PageSize = searchModel.PageSize,
				PageNumber = searchModel.PageNumber,
				TotalRecords = totalRecords,
				Results = results
			};
		}

		private static IQueryable<Movie> ApplySort(IQueryable<Movie> query, string? sortBy, bool sortDescending)
		{
			if (sortBy is null)
			{
				// Sort by ID in the absence of any specified sort order, to ensure consistent results
				return query.OrderBy(m => m.Id);
			}

			Expression<Func<Movie, object>> sortExpression = sortBy switch
			{
				"Title" => m => m.Title,
				"ReleaseDate" => m => m.ReleaseDate,
				_ => throw new NotSupportedException($"Sorting by '{sortBy}' is not supported.")
			};

			return sortDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);
		}
	}
}
