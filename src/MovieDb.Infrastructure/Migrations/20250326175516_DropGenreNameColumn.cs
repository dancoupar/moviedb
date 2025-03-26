using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropGenreNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenreName",
                table: "MovieGenre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GenreName",
                table: "MovieGenre",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
