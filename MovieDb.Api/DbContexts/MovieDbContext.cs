using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MovieDb.Api.Entities;
using System.Globalization;

namespace MovieDb.Api.DbContexts
{
	public class MovieDbContext(DbContextOptions options) : DbContext(options)
	{
		public DbSet<Movie> Movies { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (!options.IsConfigured)
			{
				options
					.UseSqlite("Data Source=Data/movies.db")
					.UseSeeding((context, _) =>
					{
						DbSet<Movie> moviesDbSet = context.Set<Movie>();

						if (moviesDbSet.Any())
						{
							return;
						}

						using var reader = new StreamReader("Assets/mymoviedb.csv");
						using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
						csv.Context.RegisterClassMap<CsvMap>();
						moviesDbSet.AddRange(csv.GetRecords<Movie>());
						context.SaveChanges();
					}
				);
			}
		}
	}
}
