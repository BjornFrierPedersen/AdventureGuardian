using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdventureGuardian.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRaceAndClassTypeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassType",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RaceType",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassType",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "RaceType",
                table: "Characters");
        }
    }
}
