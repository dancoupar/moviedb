using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MovieDb.Domain.DataModels;
using System.Globalization;

namespace MovieDb.Infrastructure.DbContexts
{
	public class MovieDbContext(DbContextOptions options) : DbContext(options)
	{
		public DbSet<Movie> Movies { get; set; }

		public DbSet<MovieGenre> MovieGenre { get; set; }

		public DbSet<MovieActor> MovieActor { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (options.IsConfigured)
			{
				return;
			}
			
			options
				.UseSqlite("Data Source=../../data/movies.db")
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Movie>()
				.Property(m => m.Title)
				.UseCollation("NOCASE");

			modelBuilder.Entity<MovieActor>()
				.Property(a => a.ActorName)
				.UseCollation("NOCASE");

			modelBuilder.Entity<Movie>()
				.HasMany(e => e.Genres)
				.WithOne(e => e.Movie)
				.HasForeignKey(e => e.MovieId)
				.HasPrincipalKey(e => e.Id);

			modelBuilder.Entity<Movie>()
				.HasMany(e => e.Actors)
				.WithOne(e => e.Movie)
				.HasForeignKey(e => e.MovieId)
				.HasPrincipalKey(e => e.Id);			
		}
	}
}
