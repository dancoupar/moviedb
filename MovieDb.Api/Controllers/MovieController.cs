using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MovieDb.Api.DbContexts;
using MovieDb.Api.Entities;
using MovieDb.Api.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace MovieDb.Api.Controllers
{
	[ApiController]
    public class MovieController(MovieDbContext dbContext, IMemoryCache cache, ILogger<MovieController> logger) : ControllerBase
    {
		private readonly MovieDbContext _dbContext = dbContext;
		private readonly IMemoryCache _cache = cache;
		private readonly ILogger<MovieController> _logger = logger;

		[HttpGet]
		[Route("movies")]
		public async Task<ActionResult<SearchResults>> Search([FromQuery] SearchModel searchModel)
		{
			ArgumentNullException.ThrowIfNull(searchModel, nameof(searchModel));

			// Cache results for 10 minutes so if the exact same search is repeated,
			// data does not need to be re-read from the DB.

			List<Movie> searchResults = await _cache.GetOrCreateAsync(CreateCacheKey(searchModel), entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
				return this.SearchDb(searchModel);
			}) ?? [];
			
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

		private async Task<List<Movie>> SearchDb(SearchModel searchModel)
		{
			IQueryable<Movie> query = _dbContext.Movies.AsQueryable().Include(m => m.Genres);

			query = query.Where(m => EF.Functions.Like(m.Title, $"%{searchModel.TitleContains}%"));
			
			if (searchModel.Genres?.Any() == true)
			{
				query = query.Where(m => m.Genres.Select(g => g.Genre).Intersect(searchModel.Genres).Any());
			}

			if (searchModel.SortBy is not null)
			{
				Expression<Func<Movie, object>>? sortExpression = sortExpression = GetSortByExpression(searchModel.SortBy);
				query = searchModel.SortDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);
			}

			return await query.Take(searchModel.MaxNumberOfResults ?? 100).ToListAsync();
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

		private string CreateCacheKey(SearchModel searchModel)
		{
			return JsonSerializer.Serialize(new
			{  
				searchModel.TitleContains,
				searchModel.Genres,
				searchModel.SortBy,
				searchModel.SortDescending
			});			
		}
	}
}
