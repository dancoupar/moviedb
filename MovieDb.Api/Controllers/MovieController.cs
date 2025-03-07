using Microsoft.AspNetCore.Mvc;
using MovieDb.Api.Models;

namespace MovieDb.Api.Controllers
{
	[ApiController]
    public class MovieController(ILogger<MovieController> logger) : ControllerBase
    {
        private readonly ILogger<MovieController> _logger = logger;

		private static IEnumerable<Movie> DummyData => [
			new Movie()
			{
				ReleaseDate = new DateOnly(2022, 3, 1),
				Title = "The Batman",
				Overview = string.Empty,
				Popularity = 3827.658m,
				VoteCount = 1151,
				VoteAverage = 8.1m,
				OriginalLanguage = "en",
				Genre = "Crime, Mystery, Thriller",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/74xTEgt7R36Fpooo50r9T25onhq.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(1989, 6, 23),
				Title = "Batman",
				Overview = string.Empty,
				Popularity = 338.272m,
				VoteCount = 6109,
				VoteAverage = 7.2m,
				OriginalLanguage = "en",
				Genre = "Fantasy, Action",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/jUhGHv4YihieVjhU2TmFaBsZ4Xg.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(2005, 6, 10),
				Title = "Batman Begins",
				Overview = string.Empty,
				Popularity = 265.806m,
				VoteCount = 17338,
				VoteAverage = 7.7m,
				OriginalLanguage = "en",
				Genre = "Action, Crime, Drama",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/8RW2runSEc34IwKN2D1aPcJd2UL.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(2016, 3, 23),
				Title = "Batman v Superman: Dawn of Justice",
				Overview = string.Empty,
				Popularity = 178.212m,
				VoteCount = 15596,
				VoteAverage = 5.9m,
				OriginalLanguage = "en",
				Genre = "Action, Adventure, Fantasy",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/5UsK3grJvtQrtzEgqNlDljJW96w.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(1992, 6, 19),
				Title = "Batman Returns",
				Overview = string.Empty,
				Popularity = 161.321m,
				VoteCount = 5075,
				VoteAverage = 6.9m,
				OriginalLanguage = "en",
				Genre = "Action, Fantasy",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/mnihMYFydSUDm5LMnavkaaZqYKp.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(1997, 6, 20),
				Title = "Batman & Robin",
				Overview = string.Empty,
				Popularity = 116.71m,
				VoteCount = 4044,
				VoteAverage = 4.3m,
				OriginalLanguage = "en",
				Genre = "Science Fiction, Action, Fantasy",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/pc6Y42r8AGAT8iv7V24AkYdcbb8.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(1972, 3, 14),
				Title = "The Godfather",
				Overview = string.Empty,
				Popularity = 93.136m,
				VoteCount = 15614,
				VoteAverage = 8.7m,
				OriginalLanguage = "en",
				Genre = "Drama, Crime",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/eEslKSwcqmiNS6va24Pbxf2UKmJ.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(1974, 12, 20),
				Title = "The Godfather: Part II",
				Overview = string.Empty,
				Popularity = 65.324m,
				VoteCount = 9393,
				VoteAverage = 8.6m,
				OriginalLanguage = "en",
				Genre = "Drama, Crime",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/hek3koDUyRQk7FIhPXsa6mT2Zc3.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(1990, 12, 25),
				Title = "The Godfather: Part III",
				Overview = string.Empty,
				Popularity = 48.643m,
				VoteCount = 4777,
				VoteAverage = 7.4m,
				OriginalLanguage = "en",
				Genre = "Crime, Drama, Thriller",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/lm3pQ2QoQ16pextRsmnUbG2onES.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(2021, 12, 22),
				Title = "The King's Man",
				Overview = string.Empty,
				Popularity = 1895.511m,
				VoteCount = 1793,
				VoteAverage = 7,
				OriginalLanguage = "en",
				Genre = "Action, Adventure, Thriller, War",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/aq4Pwv5Xeuvj6HZKtxyd23e6bE9.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(2005, 7, 13),
				Title = "Charlie and the Chocolate Factory",
				Overview = string.Empty,
				Popularity = 125.496m,
				VoteCount = 12320,
				VoteAverage = 7,
				OriginalLanguage = "en",
				Genre = "Adventure, Comedy, Family, Fantasy",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/wfGfxtBkhBzQfOZw4S8IQZgrH0a.jpg")
			},
			new Movie()
			{
				ReleaseDate = new DateOnly(1993, 6, 11),
				Title = "Jurassic Park",
				Overview = string.Empty,
				Popularity = 30.4m,
				VoteCount = 13222,
				VoteAverage = 7.9m,
				OriginalLanguage = "en",
				Genre = "Adventure, Science Fiction",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/oU7Oq2kFAAlGqbU4VoAE36g4hoI.jpg")
			}
		];

		[HttpGet]
        [Route("movies")]
        public IEnumerable<Movie> Search(string titleContains, int? maxNumberOfResults, int pageNumber, int pageSize)
		{
			IQueryable<Movie> query = DummyData.AsQueryable();

			if (!string.IsNullOrEmpty(titleContains))
			{
				query = query.Where(m => m.Title.ToLower().Contains(titleContains.ToLower()));
			}

			return query
				.Take(maxNumberOfResults ?? 100)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
        }
    }
}
