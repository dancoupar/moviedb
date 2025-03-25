using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Domain.Models;

namespace MovieDb.Application.Services
{
	public class MovieSearchService([FromKeyedServices("Caching")] IMovieRepository movieRepository, AbstractValidator<MovieSearchModel> validator) : IMovieSearchService
	{
		private readonly IMovieRepository _movieRepository = movieRepository;
		private readonly AbstractValidator<MovieSearchModel> _validator = validator;

		public async Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(searchModel);

			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			SearchResults<Movie> searchResults = await _movieRepository.Search(searchModel);

			return new SearchResults<MovieSearchResult>()
			{
				PageSize = searchResults.PageSize,
				PageNumber = searchResults.PageNumber,
				TotalRecords = searchResults.TotalRecords,
				Results = searchResults.Results.Select(r => ConvertToSearchResult(r)).ToList()
			};
		}

		public async Task<IEnumerable<string>> GetDistinctGenres()
		{
			IEnumerable<string> genres = await _movieRepository.GetDistinctGenres();
			return genres;
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
