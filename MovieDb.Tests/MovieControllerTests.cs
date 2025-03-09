using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using MovieDb.Api.Controllers;
using MovieDb.Api.DbContexts;
using MovieDb.Api.Entities;
using MovieDb.Api.Models;

namespace MovieDb.Tests
{
	public class MovieControllerTests
	{
		private static IEnumerable<Movie> TestData => [
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
				Genres = [new MovieGenre() { Genre = "Crime" }, new MovieGenre() { Genre = "Mystery" }, new MovieGenre() { Genre = "Thriller" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Fantasy" }, new MovieGenre() { Genre = "Action" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Action" }, new MovieGenre() { Genre = "Crime" }, new MovieGenre() { Genre = "Drama" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Action" }, new MovieGenre() { Genre = "Adventure" }, new MovieGenre() { Genre = "Fantasy" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Action" }, new MovieGenre() { Genre = "Fantasy" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Science Fiction" }, new MovieGenre() { Genre = "Action" }, new MovieGenre() { Genre = "Fantasy" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Drama" }, new MovieGenre() { Genre = "Crime" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Drama" }, new MovieGenre() { Genre = "Crime" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Crime" }, new MovieGenre() { Genre = "Drama" }, new MovieGenre() { Genre = "Thriller" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Action" }, new MovieGenre() { Genre = "Adventure" }, new MovieGenre() { Genre = "Thriller" }, new MovieGenre() { Genre = "War" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Adventure" }, new MovieGenre() { Genre = "Comedy" }, new MovieGenre() { Genre = "Family" }, new MovieGenre() { Genre = "Fantasy" }],
				Actors = []
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
				Genres = [new MovieGenre() { Genre = "Adventure" }, new MovieGenre() { Genre = "Science Fiction" }],
				Actors = []
			}
		];

		private static MovieDbContext GetFakeDbContext()
		{
			var options = new DbContextOptionsBuilder<MovieDbContext>()
				.UseSqlite("Filename=:memory:")
				.Options;

			var dbContext = new MovieDbContext(options);
			dbContext.Database.OpenConnection();
			dbContext.Database.EnsureCreated();

			return dbContext;
		}

		[Theory]
		[InlineData("batman", 6)]
		[InlineData("godfather", 3)]
		[InlineData("the", 6)]
		[InlineData("foo", 0)]
		[InlineData("", 12)]
		public async Task Movies_can_be_searched_by_title(string searchTerm, int expectedNumberOfResults)
		{
			// Arrange			
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<SearchResults> result = await sut.SearchMovies(new SearchModel()
			{
				TitleContains = searchTerm,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<MovieSearchResult>? movies = result.Value?.Content;
			movies.Should().NotBeNull();
			movies.Should().AllSatisfy(m => m.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
			movies.Count().Should().Be(expectedNumberOfResults);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(10)]
		public async Task Search_results_can_be_limited_to_a_specified_number(int maxNumberOfResults)
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<SearchResults> result = await sut.SearchMovies(new SearchModel()
			{
				TitleContains = string.Empty,
				MaxNumberOfResults = maxNumberOfResults,				
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<MovieSearchResult>? movies = result.Value?.Content;
			movies.Should().NotBeNull();
			movies.Count().Should().Be(maxNumberOfResults);
		}

		[Theory]
		[InlineData(1, 10)]
		[InlineData(2, 10)]
		[InlineData(1, 5)]
		[InlineData(2, 5)]
		[InlineData(3, 5)]
		public async Task Search_results_can_be_paged(int pageNumber, int pageSize)
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<SearchResults> result = await sut.SearchMovies(new SearchModel()
			{
				TitleContains = string.Empty,
				PageNumber = pageNumber,
				PageSize = pageSize
			});

			// Assert
			IEnumerable<MovieSearchResult>? movies = result.Value?.Content;
			movies.Should().NotBeNull();

			var first = movies.Select(m => m.Id).ToList();
			var second = TestData.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(m => m.Id).ToList();

			movies.Select(m => m.Id).Should().BeEquivalentTo(TestData.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(m => m.Id));
		}

		[Theory]
		[InlineData(5, "Crime")]
		[InlineData(6, "Action")]
		[InlineData(4, "Drama")]
		[InlineData(5, "Crime", "Drama")]
		[InlineData(10, "Thriller", "Adventure", "Action")]
		public async Task Movies_can_be_filtered_by_genre(int expectedNumberOfResults, params string[] genres)
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<SearchResults> result = await sut.SearchMovies(new SearchModel()
			{
				TitleContains = string.Empty,
				Genres = genres,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<MovieSearchResult>? movies = result.Value?.Content;
			movies.Should().NotBeNull();
			movies.Should().AllSatisfy(m => m.Genre.Split(", ").Intersect(genres).Should().NotBeEmpty());
			movies.Count().Should().Be(expectedNumberOfResults);
		}

		[Fact]
		public async Task Search_results_can_be_sorted_by_title()
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<SearchResults> result = await sut.SearchMovies(new SearchModel()
			{
				TitleContains = string.Empty,
				SortBy = "Title",
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<MovieSearchResult>? movies = result.Value?.Content;
			movies.Should().NotBeNull();
			movies.ElementAt(0).Id.Should().Be(2);
			movies.ElementAt(1).Id.Should().Be(6);
			movies.ElementAt(2).Id.Should().Be(3);
			movies.ElementAt(3).Id.Should().Be(5);
			movies.ElementAt(4).Id.Should().Be(4);
			movies.ElementAt(5).Id.Should().Be(11);
			movies.ElementAt(6).Id.Should().Be(12);
			movies.ElementAt(7).Id.Should().Be(1);
			movies.ElementAt(8).Id.Should().Be(7);
			movies.ElementAt(9).Id.Should().Be(8);
			movies.ElementAt(10).Id.Should().Be(9);
			movies.ElementAt(11).Id.Should().Be(10);
		}

		[Fact]
		public async Task Search_results_can_be_sorted_by_title_descending()
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<SearchResults> result = await sut.SearchMovies(new SearchModel()
			{
				TitleContains = string.Empty,
				SortBy = "Title",
				SortDescending = true,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<MovieSearchResult>? movies = result.Value?.Content;
			movies.Should().NotBeNull();
			movies.ElementAt(11).Id.Should().Be(2);
			movies.ElementAt(10).Id.Should().Be(6);
			movies.ElementAt(9).Id.Should().Be(3);
			movies.ElementAt(8).Id.Should().Be(5);
			movies.ElementAt(7).Id.Should().Be(4);
			movies.ElementAt(6).Id.Should().Be(11);
			movies.ElementAt(5).Id.Should().Be(12);
			movies.ElementAt(4).Id.Should().Be(1);
			movies.ElementAt(3).Id.Should().Be(7);
			movies.ElementAt(2).Id.Should().Be(8);
			movies.ElementAt(1).Id.Should().Be(9);
			movies.ElementAt(0).Id.Should().Be(10);
		}

		[Fact]
		public async Task Search_results_can_be_sorted_by_release_date()
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<SearchResults> result = await sut.SearchMovies(new SearchModel()
			{
				TitleContains = string.Empty,
				SortBy = "ReleaseDate",
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<MovieSearchResult>? movies = result.Value?.Content;
			movies.Should().NotBeNull();
			movies.ElementAt(0).Id.Should().Be(7);
			movies.ElementAt(1).Id.Should().Be(8);
			movies.ElementAt(2).Id.Should().Be(2);
			movies.ElementAt(3).Id.Should().Be(9);
			movies.ElementAt(4).Id.Should().Be(5);
			movies.ElementAt(5).Id.Should().Be(12);
			movies.ElementAt(6).Id.Should().Be(6);
			movies.ElementAt(7).Id.Should().Be(3);
			movies.ElementAt(8).Id.Should().Be(11);
			movies.ElementAt(9).Id.Should().Be(4);
			movies.ElementAt(10).Id.Should().Be(10);
			movies.ElementAt(11).Id.Should().Be(1);
		}

		[Fact]
		public async Task Search_results_can_be_sorted_by_release_date_descending()
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<SearchResults> result = await sut.SearchMovies(new SearchModel()
			{
				TitleContains = string.Empty,
				SortBy = "ReleaseDate",
				SortDescending = true,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<MovieSearchResult>? movies = result.Value?.Content;
			movies.Should().NotBeNull();			
			movies.ElementAt(11).Id.Should().Be(7);
			movies.ElementAt(10).Id.Should().Be(8);
			movies.ElementAt(9).Id.Should().Be(2);
			movies.ElementAt(8).Id.Should().Be(9);
			movies.ElementAt(7).Id.Should().Be(5);
			movies.ElementAt(6).Id.Should().Be(12);
			movies.ElementAt(5).Id.Should().Be(6);
			movies.ElementAt(4).Id.Should().Be(3);
			movies.ElementAt(3).Id.Should().Be(11);
			movies.ElementAt(2).Id.Should().Be(4);
			movies.ElementAt(1).Id.Should().Be(10);
			movies.ElementAt(0).Id.Should().Be(1);
		}

		[Fact]
		public async Task A_distinct_list_of_genres_can_be_retrieved_alphabetically()
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData);
			fakeDbContext.SaveChanges();

			var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

			// Act
			ActionResult<IEnumerable<string>> result = await sut.GetDistinctGenres();

			// Assert
			IEnumerable<string>? genres = result.Value;
			genres.Should().NotBeNull();
			genres.Count().Should().Be(11);
			genres.ElementAt(0).Should().Be("Action");			
			genres.ElementAt(1).Should().Be("Adventure");
			genres.ElementAt(2).Should().Be("Comedy");
			genres.ElementAt(3).Should().Be("Crime");
			genres.ElementAt(4).Should().Be("Drama");
			genres.ElementAt(5).Should().Be("Family");
			genres.ElementAt(6).Should().Be("Fantasy");
			genres.ElementAt(7).Should().Be("Mystery");
			genres.ElementAt(8).Should().Be("Science Fiction");
			genres.ElementAt(9).Should().Be("Thriller");
			genres.ElementAt(10).Should().Be("War");
		}
	}
}