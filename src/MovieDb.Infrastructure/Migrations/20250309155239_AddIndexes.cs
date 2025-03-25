using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_Genre",
                table: "MovieGenre",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActor_ActorName",
                table: "MovieActor",
                column: "ActorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MovieGenre_Genre",
                table: "MovieGenre");

            migrationBuilder.DropIndex(
                name: "IX_MovieActor_ActorName",
                table: "MovieActor");
        }
    }
}
