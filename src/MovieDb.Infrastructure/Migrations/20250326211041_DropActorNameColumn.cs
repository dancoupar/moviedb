using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropActorNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActorName",
                table: "MovieActor");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Actors",
                type: "TEXT",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActorName",
                table: "MovieActor",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Actors",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");
        }
    }
}
