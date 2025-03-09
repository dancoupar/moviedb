using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDb.Api.Migrations
{
	/// <inheritdoc />
	public partial class AddActors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieActor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActorName = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieActor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieActor_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieActor_MovieId",
                table: "MovieActor",
                column: "MovieId");

            // Populate actors firstly by just replicating rows from the MovieGenres table...
			migrationBuilder.Sql(@"
                INSERT INTO MovieActor (MovieId, ActorName)
                SELECT MovieGenre.MovieId, ''
                FROM MovieGenre
            ");

			// Then update each row with a random name
			migrationBuilder.Sql(@"
                UPDATE MovieActor
                SET ActorName = 
                CASE 
                    WHEN ABS(RANDOM()) % 8 = 0 THEN 'John Doe'
                    WHEN ABS(RANDOM()) % 8 = 1 THEN 'Emma Stone'
                    WHEN ABS(RANDOM()) % 8 = 2 THEN 'Chris Pratt'
                    WHEN ABS(RANDOM()) % 8 = 3 THEN 'Scarlett Johansson'
                    WHEN ABS(RANDOM()) % 8 = 4 THEN 'Morgan Freeman'
                    WHEN ABS(RANDOM()) % 8 = 5 THEN 'Tom Hanks'
                    WHEN ABS(RANDOM()) % 8 = 6 THEN 'Robert Downey Jr.'
                    ELSE 'Natalie Portman'
                END;
            ");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieActor");
        }
    }
}
