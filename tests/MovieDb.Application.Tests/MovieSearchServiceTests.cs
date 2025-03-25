using FluentAssertions;
using Moq;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Models;
using MovieDb.Application.Services;
using MovieDb.Domain.DataModels;
using MovieDb.Tests.Common;

namespace MovieDb.Application.Tests
{
	public class MovieSearchServiceTests
	{				
		//[Fact]
		//public async Task A_distinct_list_of_genres_can_be_retrieved_alphabetically()
		//{
		//	// Arrange
		//	using var fakeDbContext = GetFakeDbContext();
		//	fakeDbContext.Movies.AddRange(TestData);
		//	fakeDbContext.SaveChanges();

		//	var sut = new MovieController(fakeDbContext, new MemoryCache(new MemoryCacheOptions()), new Mock<ILogger<MovieController>>().Object);

		//	// Act
		//	ActionResult<IEnumerable<string>> result = await sut.GetAllGenres();

		//	// Assert
		//	var genres = ((OkObjectResult?)result?.Result)?.Value as IEnumerable<string>;
		//	genres.Should().NotBeNull();
		//	genres.Count().Should().Be(11);
		//	genres.ElementAt(0).Should().Be("Action");			
		//	genres.ElementAt(1).Should().Be("Adventure");
		//	genres.ElementAt(2).Should().Be("Comedy");
		//	genres.ElementAt(3).Should().Be("Crime");
		//	genres.ElementAt(4).Should().Be("Drama");
		//	genres.ElementAt(5).Should().Be("Family");
		//	genres.ElementAt(6).Should().Be("Fantasy");
		//	genres.ElementAt(7).Should().Be("Mystery");
		//	genres.ElementAt(8).Should().Be("Science Fiction");
		//	genres.ElementAt(9).Should().Be("Thriller");
		//	genres.ElementAt(10).Should().Be("War");
		//}
	}
}