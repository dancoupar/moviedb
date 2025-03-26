using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;

namespace MovieDb.Application.Validators
{
	public class MovieSearchModelValidator : AbstractValidator<MovieSearchModel>
	{
		private readonly IGenresQuery _genresQuery;

		public MovieSearchModelValidator([FromKeyedServices("Caching")] IGenresQuery genresQuery)
		{
			_genresQuery = genresQuery;

			this.RuleFor(m => m.TitleContains).NotEmpty();
			this.RuleFor(m => m.TitleContains).MinimumLength(3).MaximumLength(100);
			this.RuleFor(m => m.ActorContains).MinimumLength(3).MaximumLength(100);
			this.RuleFor(m => m.PageSize).InclusiveBetween(10, 100);
			this.RuleFor(m => m.PageNumber).InclusiveBetween(1, 1000);
			
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

			IEnumerable<string> validGenres = await _genresQuery.GetAllGenres();

			if (genres.Except(validGenres).Any())
			{
				return false;
			}

			return true;
		}
	}
}
