using Microsoft.AspNetCore.Mvc;
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
		public async Task<ActionResult<IEnumerable<MovieSearchResult>>> Search([FromQuery] SearchModel searchModel)
		{
			ArgumentNullException.ThrowIfNull(searchModel, nameof(searchModel));

			IQueryable<Movie> query = _dbContext.Movies.AsQueryable();

			if (!string.IsNullOrEmpty(searchModel.TitleContains))
			{
				query = query.Where(m => m.Title.ToLower().Contains(searchModel.TitleContains.ToLower()));
			}

			if (searchModel.Genres?.Any() == true)
			{
				query = query.Where(m => m.Genre.Split(", ", StringSplitOptions.RemoveEmptyEntries).Intersect(searchModel.Genres).Any());
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

			return await Task.FromResult(query
				.Take(searchModel.MaxNumberOfResults ?? 100)
				.Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
				.Take(searchModel.PageSize)
				.Select(m => ConvertToSearchResult(m))
				.ToList());
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
				Genre = entity.Genre,
				PosterUrl = entity.PosterUrl
			};
		}
	}
}
