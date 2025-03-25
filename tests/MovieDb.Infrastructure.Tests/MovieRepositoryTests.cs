using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieDb.Application.Models;
using MovieDb.Domain.DataModels;
using MovieDb.Infrastructure.DbContexts;
using MovieDb.Infrastructure.Repositories;
using MovieDb.Tests.Common;

namespace MovieDb.Infrastructure.Tests
{
	public class MovieRepositoryTests
	{
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
		[InlineData("Batman", 6)]
		[InlineData("Godfather", 3)]
		[InlineData("father", 3)]
		[InlineData("the", 6)]
		[InlineData("foo", 0)]
		[InlineData("", 12)]
		public async Task Movies_can_be_searched_by_title(string searchTerm, int expectedNumberOfResults)
		{
			// Arrange			
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = searchTerm,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<Movie> movies = results.Results;
			movies.Should().NotBeNull();
			movies.Should().AllSatisfy(m => m.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
			movies.Count().Should().Be(expectedNumberOfResults);
		}

		[Fact]
		public async Task Searching_by_title_is_not_case_sensitive()
		{
			// Arrange			
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = "godfather",
				PageNumber = 1,
				PageSize = 100
			});

			IEnumerable<Movie> movies = results.Results;
			movies.Should().NotBeNull();
			movies.Count().Should().Be(3);
		}

		[Theory]
		[InlineData("Marlon", 1)]
		[InlineData("Pacino", 3)]
		[InlineData("Laura Dern", 1)]
		[InlineData("Michael Keaton", 2)]
		[InlineData("", 12)]
		public async Task Movies_can_be_searched_by_actor(string searchTerm, int expectedNumberOfResults)
		{
			// Arrange			
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				ActorContains = searchTerm,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<Movie> movies = results.Results;
			movies.Should().NotBeNull();
			movies.Should().AllSatisfy(m => m.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
			movies.Count().Should().Be(expectedNumberOfResults);
		}

		[Fact]
		public async Task Searching_by_actor_is_not_case_sensitive()
		{
			// Arrange			
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				ActorContains = "marlon",
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<Movie> movies = results.Results;
			movies.Should().NotBeNull();
			movies.Count().Should().Be(1);
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
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				PageNumber = pageNumber,
				PageSize = pageSize
			});

			// Assert
			IEnumerable<Movie> movies = results.Results;
			movies.Should().NotBeNull();
			movies.Select(m => m.Id).Should().BeEquivalentTo(TestData.Movies.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(m => m.Id));
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
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				Genres = genres,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<Movie> movies = results.Results;
			movies.Should().NotBeNull();
			movies.Should().AllSatisfy(m => m.Genres.Select(g => g.Genre).Intersect(genres).Should().NotBeEmpty());
			movies.Count().Should().Be(expectedNumberOfResults);
		}

		[Fact]
		public async Task Search_results_can_be_sorted_by_title()
		{
			// Arrange
			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				SortBy = "Title",
				PageNumber = 1,
				PageSize = 100
			});

			//	// Assert			
			IEnumerable<Movie> movies = results.Results;
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
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				SortBy = "Title",
				SortDescending = true,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<Movie> movies = results.Results;
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
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				SortBy = "ReleaseDate",
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<Movie> movies = results.Results;
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
			fakeDbContext.Movies.AddRange(TestData.Movies);
			fakeDbContext.SaveChanges();

			var sut = new MovieRepository(fakeDbContext);

			// Act
			SearchResults<Movie> results = await sut.Search(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				SortBy = "ReleaseDate",
				SortDescending = true,
				PageNumber = 1,
				PageSize = 100
			});

			// Assert
			IEnumerable<Movie> movies = results.Results;
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

		//[Fact]
		//public void Searching_by_an_invalid_genre_throws_bad_request()
		//{
		//	// Arrange
		//	using var fakeDbContext = GetFakeDbContext();
		//	fakeDbContext.Movies.AddRange(TestData);
		//	fakeDbContext.SaveChanges();

		//	var sut = new MovieRepository(fakeDbContext);

		//	// Act
		//	Func<Task> act = async () =>
		//	{
		//		await sut.SearchMovies(new SearchModel()
		//		{
		//			TitleContains = string.Empty,
		//			Genres = ["InvalidGenre"],
		//			PageNumber = 1,
		//			PageSize = 100
		//		});
		//	};

		//	act.Should().ThrowAsync<BadHttpRequestException>();
		//}
	}
}