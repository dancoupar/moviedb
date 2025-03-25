using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FurtherNormaliseGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_Id",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenre_Genre",
                table: "MovieGenre");

            migrationBuilder.DropIndex(
                name: "IX_MovieActor_ActorName",
                table: "MovieActor");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "MovieGenre",
                newName: "GenreName");

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "MovieGenre",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

			migrationBuilder.Sql(@"
                INSERT INTO Genres (Name)
                SELECT DISTINCT(GenreName)
                FROM MovieGenre;
            ");

			migrationBuilder.Sql(@"
                UPDATE MovieGenre
                SET GenreId = (
                    SELECT g.Id
                    FROM Genres g
                    WHERE g.Name = MovieGenre.GenreName
                    LIMIT 1
                );
            ");

			migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_GenreId",
                table: "MovieGenre",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenre_Genres_GenreId",
                table: "MovieGenre",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenre_Genres_GenreId",
                table: "MovieGenre");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenre_GenreId",
                table: "MovieGenre");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "MovieGenre");

            migrationBuilder.RenameColumn(
                name: "GenreName",
                table: "MovieGenre",
                newName: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Id",
                table: "Movies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_Genre",
                table: "MovieGenre",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActor_ActorName",
                table: "MovieActor",
                column: "ActorName");
        }
    }
}
