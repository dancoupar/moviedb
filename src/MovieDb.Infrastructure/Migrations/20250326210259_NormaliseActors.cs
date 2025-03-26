using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormaliseActors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActorId",
                table: "MovieActor",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

			migrationBuilder.Sql(@"
                INSERT INTO Actors (Name)
                SELECT DISTINCT(ActorName)
                FROM MovieActor;
            ");

			migrationBuilder.Sql(@"
                UPDATE MovieActor
                SET ActorId = (
                    SELECT a.Id
                    FROM Actors a
                    WHERE a.Name = MovieActor.ActorName
                    LIMIT 1
                );
            ");

			migrationBuilder.CreateIndex(
                name: "IX_MovieActor_ActorId",
                table: "MovieActor",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActor_Actors_ActorId",
                table: "MovieActor",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActor_Actors_ActorId",
                table: "MovieActor");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropIndex(
                name: "IX_MovieActor_ActorId",
                table: "MovieActor");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "MovieActor");
        }
    }
}
