using Microsoft.Extensions.Caching.Memory;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Domain.DataModels;
using System.Text.Json;

namespace MovieDb.Infrastructure.Queries
{
	public class CachingMovieSearchQuery(IMovieSearchQuery repository, IMemoryCache cache) : IMovieSearchQuery
	{
		private readonly IMovieSearchQuery _repository = repository;
		private readonly IMemoryCache _cache = cache;

		public async Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel)
		{
			SearchResults<MovieSearchResult>? searchResults = await _cache.GetOrCreateAsync(CreateSearchResultsCacheKey(searchModel), entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
				return _repository.Search(searchModel);
			});

			return searchResults is null ? throw new ApplicationException() : searchResults;
		}

		public async Task<IEnumerable<Genre>> GetDistinctGenres()
		{
			return await _cache.GetOrCreateAsync("genres", entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
				return _repository.GetDistinctGenres();

			}) ?? [];
		}

		private static string CreateSearchResultsCacheKey(MovieSearchModel searchModel)
		{
			return JsonSerializer.Serialize(searchModel);
		}
	}
}
