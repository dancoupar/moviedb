using FluentAssertions;
using FluentValidation.Results;
using Moq;
using MovieDb.Application.Interfaces;
using MovieDb.Application.Validators;
using MovieDb.Domain.Models;

namespace MovieDb.Application.Tests.Validators
{
	public class MovieSearchModelValidatorTests
	{
		[Fact]
		public async Task Title_contains_cannot_be_empty()
		{
			// Arrange
			var sut = new MovieSearchModelValidator(new Mock<IGenresQuery>().Object);

			// Act
			ValidationResult result = await sut.ValidateAsync(new MovieSearchModel()
			{
				TitleContains = string.Empty,
				PageSize = 10,
				PageNumber = 1
			});

			// Assert
			result.IsValid.Should().Be(false);
		}

		[Theory]
		[InlineData(2, false)]
		[InlineData(3, true)]
		[InlineData(100, true)]
		[InlineData(101, false)]
		public async Task Title_contains_must_be_between_3_and_100_characters(int numberOfCharacters, bool expectedResult)
		{
			// Arrange
			var sut = new MovieSearchModelValidator(new Mock<IGenresQuery>().Object);

			// Act
			ValidationResult result = await sut.ValidateAsync(new MovieSearchModel()
			{
				TitleContains = new string('x', numberOfCharacters),
				PageSize = 10,
				PageNumber = 1
			});

			// Assert
			result.IsValid.Should().Be(expectedResult);
		}

		[Theory]
		[InlineData(2, false)]
		[InlineData(3, true)]
		[InlineData(100, true)]
		[InlineData(101, false)]
		public async Task Actor_contains_must_be_between_3_and_100_characters_if_specified(int numberOfCharacters, bool expectedResult)
		{
			// Arrange
			var sut = new MovieSearchModelValidator(new Mock<IGenresQuery>().Object);

			// Act
			ValidationResult result = await sut.ValidateAsync(new MovieSearchModel()
			{
				TitleContains = "xxx",
				ActorContains = new string('x', numberOfCharacters),
				PageSize = 10,
				PageNumber = 1
			});

			// Assert
			result.IsValid.Should().Be(expectedResult);
		}

		[Theory]
		[InlineData(9, false)]
		[InlineData(10, true)]
		[InlineData(100, true)]
		[InlineData(101, false)]
		public async Task Page_size_must_be_between_10_and_100(int pageSize, bool expectedResult)
		{
			// Arrange
			var sut = new MovieSearchModelValidator(new Mock<IGenresQuery>().Object);

			// Act
			ValidationResult result = await sut.ValidateAsync(new MovieSearchModel()
			{
				TitleContains = "xxx",
				PageSize = pageSize,
				PageNumber = 1
			});

			// Assert
			result.IsValid.Should().Be(expectedResult);
		}

		[Theory]
		[InlineData(0, false)]
		[InlineData(1, true)]
		[InlineData(1000, true)]
		[InlineData(1001, false)]
		public async Task Page_number_must_be_between_1_and_1000(int pageNumber, bool expectedResult)
		{
			// Arrange
			var sut = new MovieSearchModelValidator(new Mock<IGenresQuery>().Object);

			// Act
			ValidationResult result = await sut.ValidateAsync(new MovieSearchModel()
			{
				TitleContains = "xxx",
				PageSize = 10,
				PageNumber = pageNumber
			});

			// Assert
			result.IsValid.Should().Be(expectedResult);
		}

		[Theory]
		[InlineData("Title", true)]
		[InlineData("ReleaseDate", true)]
		[InlineData("xxx", false)]
		public async Task Sort_by_must_be_by_title_or_release_date(string sortBy, bool expectedResult)
		{
			// Arrange
			var sut = new MovieSearchModelValidator(new Mock<IGenresQuery>().Object);

			// Act
			ValidationResult result = await sut.ValidateAsync(new MovieSearchModel()
			{
				TitleContains = "xxx",
				PageSize = 10,
				PageNumber = 1,
				SortBy = sortBy
			});

			// Assert
			result.IsValid.Should().Be(expectedResult);
		}

		[Theory]
		[InlineData(true, "Action")]
		[InlineData(true, "Action", "Thriller")]
		[InlineData(true, "Action", "Thriller", "Comedy")]
		[InlineData(false, "Action", "xxx")]
		[InlineData(false, "xxx")]
		public async Task Genres_must_be_valid_if_specified(bool expectedResult, params string[] genres)
		{
			// Arrange
			var genresQueryMock = new Mock<IGenresQuery>();
			genresQueryMock.Setup(m => m.GetAllGenres()).ReturnsAsync(["Action", "Thriller", "Comedy"]);

			var sut = new MovieSearchModelValidator(genresQueryMock.Object);

			// Act
			ValidationResult result = await sut.ValidateAsync(new MovieSearchModel()
			{
				TitleContains = "xxx",
				PageSize = 10,
				PageNumber = 1,
				Genres = genres
			});

			// Assert
			result.IsValid.Should().Be(expectedResult);
		}
	}
}
