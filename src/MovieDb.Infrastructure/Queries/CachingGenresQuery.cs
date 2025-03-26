using Microsoft.Extensions.Caching.Memory;
using MovieDb.Application.Interfaces;

namespace MovieDb.Infrastructure.Queries
{
	public class CachingGenresQuery(IGenresQuery query, IMemoryCache cache) : IGenresQuery
	{
		private readonly IGenresQuery _query = query;
		private readonly IMemoryCache _cache = cache;

		public async Task<IEnumerable<string>> GetAllGenres()
		{
			return await _cache.GetOrCreateAsync("genres", entry =>
			{
				entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
				return _query.GetAllGenres();

			}) ?? [];
		}
	}
}
