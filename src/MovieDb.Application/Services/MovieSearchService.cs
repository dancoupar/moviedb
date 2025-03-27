using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using MovieDb.Application.Interfaces;
using MovieDb.Domain.Models;

namespace MovieDb.Application.Services
{
	public class MovieSearchService(
		[FromKeyedServices("Caching")] IMovieSearchQuery movieSearchQuery,
		[FromKeyedServices("Caching")] IGenresQuery genresQuery,
		IValidator<MovieSearchModel> validator) : IMovieSearchService
	{
		private readonly IMovieSearchQuery _movieSearchQuery = movieSearchQuery;
		private readonly IGenresQuery _genresQuery = genresQuery;
		private readonly IValidator<MovieSearchModel> _validator = validator;

		public async Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(searchModel);

			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			return await _movieSearchQuery.Search(searchModel);
		}

		public async Task<IEnumerable<string>> GetAllGenres()
		{
			return await _genresQuery.GetAllGenres();
		}
	}
}
