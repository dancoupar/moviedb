using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Domain.DataModels;

namespace MovieDb.Application.Validators
{
	public class MovieSearchModelValidator : AbstractValidator<MovieSearchModel>
	{
		private readonly IMovieRepository _movieRepository;

		public MovieSearchModelValidator([FromKeyedServices("Caching")] IMovieRepository movieRepository)
		{
			_movieRepository = movieRepository;

			this.RuleFor(m => m.TitleContains).MinimumLength(3).MaximumLength(100);
			this.RuleFor(m => m.ActorContains).MinimumLength(3).MaximumLength(100);
			this.RuleFor(m => m.PageSize).InclusiveBetween(1, 100);
			this.RuleFor(m => m.PageNumber).InclusiveBetween(1, 100);
			
			string[] validSortBys = ["Title", "ReleaseDate"];
			this.RuleFor(m => m.SortBy).Must(s => s is null || validSortBys.Contains(s)).WithMessage("Invalid sort expression.");

			this.RuleFor(m => m.Genres).Must(g => !(g?.Length > 20)).WithMessage("No more than 20 genres may be specified.");
			this.RuleFor(m => m.Genres).MustAsync(async (g, _) => await AllGenresValid(g)).WithMessage("One or more genres were not valid.");
		}

		private async Task<bool> AllGenresValid(string[]? genres)
		{
			if (genres is null)
			{
				return true;
			}

			IEnumerable<Genre> validGenres = await _movieRepository.GetDistinctGenres();

			if (genres.Except(validGenres.Select(g => g.Name)).Any())
			{
				return false;
			}

			return true;
		}
	}
}
