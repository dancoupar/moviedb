using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Domain.DataModels;

namespace MovieDb.Application.Services
{
	public class MovieSearchService([FromKeyedServices("Caching")] IMovieSearchQuery movieRepository, AbstractValidator<MovieSearchModel> validator) : IMovieSearchService
	{
		private readonly IMovieSearchQuery _movieRepository = movieRepository;
		private readonly AbstractValidator<MovieSearchModel> _validator = validator;

		public async Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel)
		{
			ValidationResult validationResult = await _validator.ValidateAsync(searchModel);

			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			return await _movieRepository.Search(searchModel);
		}

		public async Task<IEnumerable<string>> GetDistinctGenres()
		{
			IEnumerable<Genre> genres = await _movieRepository.GetDistinctGenres();
			return genres.Select(g => g.Name).ToList();
		}		
	}
}
