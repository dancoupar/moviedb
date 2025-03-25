using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Application.Services;
using MovieDb.Application.Validators;

namespace MovieDb.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IMovieSearchService, MovieSearchService>();
			services.AddScoped<AbstractValidator<MovieSearchModel>, MovieSearchModelValidator>();
			return services;
		}
	}
}
