using MovieDb.Domain.DataModels;

namespace MovieDb.Tests.Common
{
	public class TestData
	{
		public static IEnumerable<Movie> Movies => [
			new Movie()
			{
				Id = 1,
				ReleaseDate = new DateOnly(2022, 3, 1),
				Title = "The Batman",
				Overview = string.Empty,
				Popularity = 3827.658m,
				VoteCount = 1151,
				VoteAverage = 8.1m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/74xTEgt7R36Fpooo50r9T25onhq.jpg"),
				Genres = [new MovieGenre() { GenreName = "Crime" }, new MovieGenre() { GenreName = "Mystery" }, new MovieGenre() { GenreName = "Thriller" }],
				Actors = [new MovieActor() { ActorName = "Robert Pattinson" }, new MovieActor() { ActorName = "Zoë Kravitz" }]
			},
			new Movie()
			{
				Id = 2,
				ReleaseDate = new DateOnly(1989, 6, 23),
				Title = "Batman",
				Overview = string.Empty,
				Popularity = 338.272m,
				VoteCount = 6109,
				VoteAverage = 7.2m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/jUhGHv4YihieVjhU2TmFaBsZ4Xg.jpg"),
				Genres = [new MovieGenre() { GenreName = "Fantasy" }, new MovieGenre() { GenreName = "Action" }],
				Actors = [new MovieActor() { ActorName = "Michael Keaton" }, new MovieActor() { ActorName = "Jack Nicholson" }]
			},
			new Movie()
			{
				Id = 3,
				ReleaseDate = new DateOnly(2005, 6, 10),
				Title = "Batman Begins",
				Overview = string.Empty,
				Popularity = 265.806m,
				VoteCount = 17338,
				VoteAverage = 7.7m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/8RW2runSEc34IwKN2D1aPcJd2UL.jpg"),
				Genres = [new MovieGenre() { GenreName = "Action" }, new MovieGenre() { GenreName = "Crime" }, new MovieGenre() { GenreName = "Drama" }],
				Actors = [new MovieActor() { ActorName = "Christian Bale" }, new MovieActor() { ActorName = "Katie Holmes" }, new MovieActor() { ActorName = "Liam Neeson" }]
			},
			new Movie()
			{
				Id = 4,
				ReleaseDate = new DateOnly(2016, 3, 23),
				Title = "Batman v Superman: Dawn of Justice",
				Overview = string.Empty,
				Popularity = 178.212m,
				VoteCount = 15596,
				VoteAverage = 5.9m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/5UsK3grJvtQrtzEgqNlDljJW96w.jpg"),
				Genres = [new MovieGenre() { GenreName = "Action" }, new MovieGenre() { GenreName = "Adventure" }, new MovieGenre() { GenreName = "Fantasy" }],
				Actors = [new MovieActor() { ActorName = "Ben Affleck" }, new MovieActor() { ActorName = "Henry Cavill" }, new MovieActor() { ActorName = "Amy Adams" }]
			},
			new Movie()
			{
				Id = 5,
				ReleaseDate = new DateOnly(1992, 6, 19),
				Title = "Batman Returns",
				Overview = string.Empty,
				Popularity = 161.321m,
				VoteCount = 5075,
				VoteAverage = 6.9m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/mnihMYFydSUDm5LMnavkaaZqYKp.jpg"),
				Genres = [new MovieGenre() { GenreName = "Action" }, new MovieGenre() { GenreName = "Fantasy" }],
				Actors = [new MovieActor() { ActorName = "Michael Keaton" }, new MovieActor() { ActorName = "Danny DeVito" }]
			},
			new Movie()
			{
				Id = 6,
				ReleaseDate = new DateOnly(1997, 6, 20),
				Title = "Batman & Robin",
				Overview = string.Empty,
				Popularity = 116.71m,
				VoteCount = 4044,
				VoteAverage = 4.3m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/pc6Y42r8AGAT8iv7V24AkYdcbb8.jpg"),
				Genres = [new MovieGenre() { GenreName = "Science Fiction" }, new MovieGenre() { GenreName = "Action" }, new MovieGenre() { GenreName = "Fantasy" }],
				Actors = [new MovieActor() { ActorName = "George Cloone" }, new MovieActor() { ActorName = "Chris O'Donnel" },  new MovieActor() { ActorName = "Alicia Silverstone" }]
			},
			new Movie()
			{
				Id = 7,
				ReleaseDate = new DateOnly(1972, 3, 14),
				Title = "The Godfather",
				Overview = string.Empty,
				Popularity = 93.136m,
				VoteCount = 15614,
				VoteAverage = 8.7m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/eEslKSwcqmiNS6va24Pbxf2UKmJ.jpg"),
				Genres = [new MovieGenre() { GenreName = "Drama" }, new MovieGenre() { GenreName = "Crime" }],
				Actors = [new MovieActor() { ActorName = "Marlon Brando" }, new MovieActor() { ActorName = "Al Pacino" }]
			},
			new Movie()
			{
				Id = 8,
				ReleaseDate = new DateOnly(1974, 12, 20),
				Title = "The Godfather: Part II",
				Overview = string.Empty,
				Popularity = 65.324m,
				VoteCount = 9393,
				VoteAverage = 8.6m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/hek3koDUyRQk7FIhPXsa6mT2Zc3.jpg"),
				Genres = [new MovieGenre() { GenreName = "Drama" }, new MovieGenre() { GenreName = "Crime" }],
				Actors = [new MovieActor() { ActorName = "Al Pacino" }, new MovieActor() { ActorName = "Robert De Niro" }]
			},
			new Movie()
			{
				Id = 9,
				ReleaseDate = new DateOnly(1990, 12, 25),
				Title = "The Godfather: Part III",
				Overview = string.Empty,
				Popularity = 48.643m,
				VoteCount = 4777,
				VoteAverage = 7.4m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/lm3pQ2QoQ16pextRsmnUbG2onES.jpg"),
				Genres = [new MovieGenre() { GenreName = "Crime" }, new MovieGenre() { GenreName = "Drama" }, new MovieGenre() { GenreName = "Thriller" }],
				Actors = [new MovieActor() { ActorName = "Al Pacino" }, new MovieActor() { ActorName = "Sofia Coppola" }, new MovieActor() { ActorName = "Andy Garcia" }]
			},
			new Movie()
			{
				Id = 10,
				ReleaseDate = new DateOnly(2021, 12, 22),
				Title = "The King's Man",
				Overview = string.Empty,
				Popularity = 1895.511m,
				VoteCount = 1793,
				VoteAverage = 7,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/aq4Pwv5Xeuvj6HZKtxyd23e6bE9.jpg"),
				Genres = [new MovieGenre() { GenreName = "Action" }, new MovieGenre() { GenreName = "Adventure" }, new MovieGenre() { GenreName = "Thriller" }, new MovieGenre() { GenreName = "War" }],
				Actors = [new MovieActor() { ActorName = "Ralph Fiennes" }, new MovieActor() { ActorName = "Gemma Arterton" }, new MovieActor() { ActorName = "Rhys Ifans" }]
			},
			new Movie()
			{
				Id = 11,
				ReleaseDate = new DateOnly(2005, 7, 13),
				Title = "Charlie and the Chocolate Factory",
				Overview = string.Empty,
				Popularity = 125.496m,
				VoteCount = 12320,
				VoteAverage = 7,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/wfGfxtBkhBzQfOZw4S8IQZgrH0a.jpg"),
				Genres = [new MovieGenre() { GenreName = "Adventure" }, new MovieGenre() { GenreName = "Comedy" }, new MovieGenre() { GenreName = "Family" }, new MovieGenre() { GenreName = "Fantasy" }],
				Actors = [new MovieActor() { ActorName = "Johnny Depp" }, new MovieActor() { ActorName = "Freddie Highmore" }]
			},
			new Movie()
			{
				Id = 12,
				ReleaseDate = new DateOnly(1993, 6, 11),
				Title = "Jurassic Park",
				Overview = string.Empty,
				Popularity = 30.4m,
				VoteCount = 13222,
				VoteAverage = 7.9m,
				OriginalLanguage = "en",
				PosterUrl = new Uri("https://image.tmdb.org/t/p/original/oU7Oq2kFAAlGqbU4VoAE36g4hoI.jpg"),
				Genres = [new MovieGenre() { GenreName = "Adventure" }, new MovieGenre() { GenreName = "Science Fiction" }],
				Actors = [new MovieActor() { ActorName = "Sam Neill" }, new MovieActor() { ActorName = "Laura Dern" }, new MovieActor() { ActorName = "Jeff Goldblum" }]
			}
		];
	}
}
