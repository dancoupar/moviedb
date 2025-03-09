using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MovieDb.Api.DbContexts;
using MovieDb.Api.Entities;
using MovieDb.Api.Models;
using System.Text.Json;

namespace MovieDb.Api.Controllers
{
	[ApiController]
	[Produces("application/json")]
	public class MovieController(MovieDbContext dbContext, IMemoryCache cache, ILogger<MovieController> logger) : ControllerBase
    {
		private readonly MovieDbContext _dbContext = dbContext;
		private readonly IMemoryCache _cache = cache;
		private readonly ILogger<MovieController> _logger = logger;

		/// <summary>
		/// Searches for movies based on specified search criteria.
		/// </summary>
		/// <returns>Search results, including pagination info.</returns>
		/// <response code="200">If search results are returned successfully.</response>
		/// <response code="400">If one or more search criteria are missing or invalid.</response>
		[HttpGet]
		[Route("movies")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<SearchResults>> SearchMovies([FromQuery] SearchModel searchModel)
		{
			ArgumentNullException.ThrowIfNull(searchModel, nameof(searchModel));

			if (searchModel.Genres is not null)
			{
				IEnumerable<string> validGenres = await this.GetDistinctGenres();
				
				if (searchModel.Genres.Except(validGenres).Any())
				{
					throw new BadHttpRequestException("One or more genres were not valid.");
				}
			}

			// Cache results for 10 minutes so if the exact same search is repeated,
			// data does not need to be re-read from the DB.

			IEnumerable<Movie> searchResults = await _cache.GetOrCreateAsync(CreateCacheKey(searchModel), entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
				return this.SearchDb(searchModel);
			}) ?? [];

			if (searchModel.SortBy is not null)
			{
				Func<Movie, object>? sortExpression = sortExpression = GetSortByExpression(searchModel.SortBy);
				searchResults = searchModel.SortDescending ? searchResults.OrderByDescending(sortExpression) : searchResults.OrderBy(sortExpression);
			}

			return new SearchResults()
			{
				PageNumber = searchModel.PageNumber,
				PageSize = searchModel.PageSize,
				TotalElements = searchResults.Count(),
				Content = searchResults
					.Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
					.Take(searchModel.PageSize)
					.Select(m => ConvertToSearchResult(m))
			};
        }

		[HttpGet]
		[Route("genres")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<string>>> GetAllGenres()
		{
			return this.Ok(await this.GetDistinctGenres());
		}

		[Route("/error")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public IActionResult HandleError()
		{
			var exceptionHandlerFeature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
			
			if (exceptionHandlerFeature?.Error is not null)
			{
				string message = exceptionHandlerFeature.Error.Message!;

				if (exceptionHandlerFeature.Error is BadHttpRequestException)
				{
					return this.Problem(detail: message, statusCode: 400);
				}
				else
				{
					_logger.LogError(exceptionHandlerFeature.Error, "An unhandled exception occurred: {Message}", message);
				}

				return this.Problem(detail: message);
			}

			return this.Problem();
		}

		private async Task<List<Movie>> SearchDb(SearchModel searchModel)
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
			
			return await query.Take(searchModel.MaxNumberOfResults ?? 100).ToListAsync();
		}

		private static Func<Movie, object> GetSortByExpression(string? sortBy)
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

		private static string CreateCacheKey(SearchModel searchModel)
		{
			return JsonSerializer.Serialize(new
			{  
				searchModel.TitleContains,
				searchModel.ActorContains,
				searchModel.Genres,
				searchModel.MaxNumberOfResults
			});			
		}

		private async Task<IEnumerable<string>> GetDistinctGenres()
		{
			return await _cache.GetOrCreateAsync("genres", entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
				return _dbContext.MovieGenre
					.AsQueryable()
					.Select(g => g.Genre)
					.Distinct()
					.Order()
					.ToListAsync();
			}) ?? [];
		}
	}
}
