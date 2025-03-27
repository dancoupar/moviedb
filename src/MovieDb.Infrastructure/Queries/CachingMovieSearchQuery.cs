using Microsoft.Extensions.Caching.Memory;
using MovieDb.Application.Interfaces;
using MovieDb.Domain.Models;
using System.Text.Json;

namespace MovieDb.Infrastructure.Queries
{
	public class CachingMovieSearchQuery(IMovieSearchQuery query, IMemoryCache cache) : IMovieSearchQuery
	{
		private readonly IMovieSearchQuery _query = query;
		private readonly IMemoryCache _cache = cache;

		public async Task<SearchResults<MovieSearchResult>> Search(MovieSearchModel searchModel)
		{
			SearchResults<MovieSearchResult>? searchResults = await _cache.GetOrCreateAsync(CreateSearchResultsCacheKey(searchModel), entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
				return _query.Search(searchModel);
			});

			return searchResults is null ? throw new ApplicationException() : searchResults;
		}

		private static string CreateSearchResultsCacheKey(MovieSearchModel searchModel)
		{
			return JsonSerializer.Serialize(searchModel);
		}
	}
}
