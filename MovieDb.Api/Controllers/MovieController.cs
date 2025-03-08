using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDb.Api.DbContexts;
using MovieDb.Api.Entities;
using MovieDb.Api.Models;
using System.Linq.Expressions;

namespace MovieDb.Api.Controllers
{
	[ApiController]
    public class MovieController(MovieDbContext dbContext, ILogger<MovieController> logger) : ControllerBase
    {
		private readonly MovieDbContext _dbContext = dbContext;
        private readonly ILogger<MovieController> _logger = logger;

		[HttpGet]
		[Route("movies")]
		public async Task<ActionResult<SearchResults>> Search([FromQuery] SearchModel searchModel)
		{
			ArgumentNullException.ThrowIfNull(searchModel, nameof(searchModel));

			IQueryable<Movie> query = _dbContext.Movies.AsQueryable().Include(m => m.Genres);			

			if (!string.IsNullOrEmpty(searchModel.TitleContains))
			{
				query = query.Where(m => EF.Functions.Like(m.Title, $"%{searchModel.TitleContains}%"));
			}

			if (searchModel.Genres?.Any() == true)
			{
				query = query.Where(m => m.Genres.Select(g => g.Genre).Intersect(searchModel.Genres).Any());
			}

			if (!string.IsNullOrEmpty(searchModel.SortBy))
			{
				Expression<Func<Movie, object>>? sortExpression = null;

				try
				{
					sortExpression = GetSortByExpression(searchModel.SortBy);
				}
				catch (NotSupportedException e)
				{
					return this.BadRequest(e.Message);
				}

				query = searchModel.SortDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);
			}

			List<Movie> searchResults = await query.Take(searchModel.MaxNumberOfResults ?? 100).ToListAsync();

			return new SearchResults()
			{
				PageNumber = searchModel.PageNumber,
				PageSize = searchModel.PageSize,
				TotalElements = searchResults.Count,
				Content = searchResults
					.Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
					.Take(searchModel.PageSize)
					.Select(m => ConvertToSearchResult(m))
			};
        }

		private static Expression<Func<Movie, object>> GetSortByExpression(string sortBy)
		{
			return sortBy switch
			{
				"Title" => m => m.Title,
				"ReleaseDate" => m => m.ReleaseDate,
				_ => throw new NotSupportedException($"Sorting by '{sortBy}' is not supported.")
			};
		}

		private static MovieSearchResult ConvertToSearchResult(Movie entity)
		{
			return new MovieSearchResult()
			{
				Id = entity.Id,
				Title = entity.Title,
				ReleaseDate = entity.ReleaseDate,
				Overview = entity.Overview,
				Popularity = entity.Popularity,
				VoteCount = entity.VoteCount,
				VoteAverage = entity.VoteAverage,
				OriginalLanguage = entity.OriginalLanguage,
				Genre = string.Join(", ", entity.Genres.Select(g => g.Genre)),
				PosterUrl = entity.PosterUrl
			};
		}
	}
}
