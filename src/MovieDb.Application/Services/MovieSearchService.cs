using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Domain.Models;

namespace MovieDb.Application.Services
{
	public class MovieSearchService(IMovieRepository movieRepository) : IMovieSearchService
	{
		private readonly IMovieRepository _movieRepository = movieRepository;

		public async Task<IEnumerable<MovieSearchResult>> SearchMovies(MovieSearchModel searchModel)
		{
			ArgumentNullException.ThrowIfNull(nameof(searchModel));

			IEnumerable<Movie> searchResults = await _movieRepository.Search(searchModel);

			if (searchModel.SortBy is not null)
			{
				Func<Movie, object>? sortExpression = GetSortByExpression(searchModel.SortBy);
				searchResults = searchModel.SortDescending ? searchResults.OrderByDescending(sortExpression) : searchResults.OrderBy(sortExpression);
			}

			return searchResults.Select(r => ConvertToSearchResult(r)).ToList();
		}

		public async Task<IEnumerable<string>> GetDistinctGenres()
		{
			IEnumerable<string> genres = await _movieRepository.GetDistinctGenres();
			return genres;
		}

		private static Func<Movie, object> GetSortByExpression(string? sortBy)
		{
			return sortBy switch
			{
				"Title" => m => m.Title,
				"ReleaseDate" => m => m.ReleaseDate,
				_ => throw new NotSupportedException($"Sorting by '{sortBy}' is not supported.")
			};
		}

		private static MovieSearchResult ConvertToSearchResult(Movie entity)
		{
			return new MovieSearchResult()
			{
				Id = entity.Id,
				Title = entity.Title,
				ReleaseDate = entity.ReleaseDate,
				Overview = entity.Overview,
				Popularity = entity.Popularity,
				VoteCount = entity.VoteCount,
				VoteAverage = entity.VoteAverage,
				OriginalLanguage = entity.OriginalLanguage,
				Genre = string.Join(", ", entity.Genres.Select(g => g.Genre)),
				PosterUrl = entity.PosterUrl
			};
		}
	}
}
