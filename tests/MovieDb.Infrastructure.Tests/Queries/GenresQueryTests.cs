using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieDb.Domain.DataModels;
using MovieDb.Infrastructure.DbContexts;
using MovieDb.Infrastructure.Queries;

namespace MovieDb.Infrastructure.Tests.Queries
{
	public class GenresQueryTests
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

		[Fact]
		public async Task Genres_are_returned_in_alphabetical_order()
		{
			// Arrange
			var genres = new List<Genre>()
			{
				new() { Id = 1, Name = "Action" },
				new() { Id = 2, Name = "Drama" },
				new() { Id = 3, Name = "Crime" }
			};

			using var fakeDbContext = GetFakeDbContext();
			fakeDbContext.Genres.AddRange(genres);
			fakeDbContext.SaveChanges();
			
			var sut = new GenresQuery(fakeDbContext);

			// Act
			IEnumerable<string> results = await sut.GetAllGenres();

			// Assert
			results.Count().Should().Be(3);
			results.ElementAt(0).Should().Be("Action");
			results.ElementAt(1).Should().Be("Crime");
			results.ElementAt(2).Should().Be("Drama");
		}
	}
}
