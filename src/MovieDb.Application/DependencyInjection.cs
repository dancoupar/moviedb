using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Services;
using MovieDb.Application.Validators;
using MovieDb.Domain.Models;

namespace MovieDb.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IMovieSearchService, MovieSearchService>();
			services.AddScoped<IValidator<MovieSearchModel>, MovieSearchModelValidator>();
			return services;
		}
	}
}
