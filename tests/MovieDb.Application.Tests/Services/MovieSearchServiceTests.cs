using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Services;
using MovieDb.Domain.Models;

namespace MovieDb.Application.Tests.Services
{
	public class MovieSearchServiceTests
	{
		[Fact]
		public async Task Correct_search_results_are_returned()
		{
			// Arrange
			var searchModel = new MovieSearchModel()
			{
				TitleContains = "xxx",
				PageNumber = 1,
				PageSize = 10
			};

			var searchResults = new List<MovieSearchResult>()
			{
				new()
				{
					Id = 1,
					Title = "Batman",
					ReleaseDate = new DateOnly(1989, 6, 23),
					Genre = "Fantasy, Action"
				},
				new()
				{
					Id = 2,
					Title = "The Godfather",
					ReleaseDate = new DateOnly(1972, 3, 14),
					Genre = "Drama, Crime"
				},
				new()
				{
					Id = 3,
					Title = "Jurassic Park",
					ReleaseDate = new DateOnly(1993, 6, 11),
					Genre = "Adventure, Science Fiction"
				}
			};

			var mockSearchQuery = new Mock<IMovieSearchQuery>();
			mockSearchQuery.Setup(m => m.Search(searchModel)).ReturnsAsync(new SearchResults<MovieSearchResult>()
			{
				Results = searchResults
			});

			var mockValidator = new Mock<IValidator<MovieSearchModel>>();
			mockValidator.Setup(m => m.ValidateAsync(searchModel, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
		
			var sut = new MovieSearchService(mockSearchQuery.Object, new Mock<IGenresQuery>().Object, mockValidator.Object);

			// Act
			SearchResults<MovieSearchResult> results = await sut.Search(searchModel);

			// Assert
			results.Results.Should().BeEquivalentTo(searchResults);
		}

		[Fact]
		public async Task An_exception_is_thrown_if_the_search_model_is_invalid()
		{
			// Arrange
			var invalidSearchModel = new MovieSearchModel()
			{
				TitleContains = "x",
				PageNumber = 1,
				PageSize = 10
			};

			var mockValidator = new Mock<IValidator<MovieSearchModel>>();
			mockValidator.Setup(m => m.ValidateAsync(invalidSearchModel, It.IsAny<CancellationToken>())).ReturnsAsync(
				new ValidationResult() { Errors = [new ValidationFailure()] }
			);

			var sut = new MovieSearchService(new Mock<IMovieSearchQuery>().Object, new Mock<IGenresQuery>().Object, mockValidator.Object);

			// Act
			Func<Task> act = async () => await sut.Search(invalidSearchModel);

			// Assert
			await act.Should().ThrowAsync<ValidationException>();
		}

		[Fact]
		public async Task Correct_genres_are_returned()
		{
			// Arrange
			string[] genres = ["Fantasy", "Action", "Drama", "Crime"];
			
			var mockGenresQuery = new Mock<IGenresQuery>();
			mockGenresQuery.Setup(m => m.GetAllGenres()).ReturnsAsync(genres);

			var sut = new MovieSearchService(new Mock<IMovieSearchQuery>().Object, mockGenresQuery.Object, new Mock<IValidator<MovieSearchModel>>().Object);

			// Act
			IEnumerable<string> results = await sut.GetAllGenres();

			// Assert
			results.Should().BeEquivalentTo(genres);
		}
	}
}
