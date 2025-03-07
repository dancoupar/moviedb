using CsvHelper.Configuration;
using MovieDb.Api.Entities;

namespace MovieDb.Api.DbContexts
{
	public class CsvMap : ClassMap<Movie>
	{
		public CsvMap()
		{
			Map(m => m.ReleaseDate).Name("Release_Date");
			Map(m => m.Title).Name("Title");
			Map(m => m.Overview).Name("Overview");
			Map(m => m.Popularity).Name("Popularity");
			Map(m => m.VoteCount).Name("Vote_Count");
			Map(m => m.VoteAverage).Name("Vote_Average");
			Map(m => m.OriginalLanguage).Name("Original_Language");
			Map(m => m.Genre).Name("Genre");
			Map(m => m.PosterUrl).Name("Poster_Url");
		}
	}
}
