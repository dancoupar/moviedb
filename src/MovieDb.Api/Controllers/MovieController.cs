using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieDb.Api.Models;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;

namespace MovieDb.Api.Controllers
{
	[ApiController]
	[Produces("application/json")]
	public class MovieController(IMovieSearchService movieSearchService, ILogger<MovieController> logger) : ControllerBase
    {
		private readonly IMovieSearchService _movieSearchService = movieSearchService;
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
		public async Task<ActionResult<SearchResponseModel>> SearchMovies([FromQuery] SearchRequestModel searchModel)
		{
			ArgumentNullException.ThrowIfNull(searchModel, nameof(searchModel));

			SearchResults<MovieSearchResult> searchResults = await _movieSearchService.Search(new MovieSearchModel()
			{
				TitleContains = searchModel.TitleContains,
				ActorContains = searchModel.ActorContains,
				PageSize = searchModel.PageSize,
				PageNumber = searchModel.PageNumber,
				SortBy = searchModel.SortBy,
				SortDescending = searchModel.SortDescending,
				Genres = searchModel.Genres,
			});

			return new SearchResponseModel()
			{
				PageNumber = searchModel.PageNumber,
				PageSize = searchModel.PageSize,
				TotalElements = searchResults.TotalRecords,
				Content = searchResults.Results
			};
		}

		/// <summary>
		/// Gets a distinct list of all movie genres.
		/// </summary>
		/// <returns>A distinct list of all genres.</returns>
		/// <response code="200">If genres are returned successfully.</response>
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

		private async Task<IEnumerable<string>> GetDistinctGenres()
		{			
			return await _movieSearchService.GetDistinctGenres();
		}
	}
}
