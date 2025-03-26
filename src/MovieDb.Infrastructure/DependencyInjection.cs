using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MovieDb.Application.Interfaces;
using MovieDb.Infrastructure.DbContexts;
using MovieDb.Infrastructure.Queries;

namespace MovieDb.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
		{
			services.AddDbContext<MovieDbContext>();
			services.AddMemoryCache();

			services.AddScoped<IMovieSearchQuery, MovieSearchQuery>();
			services.AddKeyedScoped<IMovieSearchQuery, CachingMovieSearchQuery>("Caching");

			return services;
		}
	}
}
