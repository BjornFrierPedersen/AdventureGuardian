using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdventureGuardian.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSexColumnToGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Encounters_EncounterId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_EncounterId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "EncounterId",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Sex",
                table: "Characters",
                newName: "Gender");

            migrationBuilder.AddColumn<List<int>>(
                name: "CharacterIds",
                table: "Encounters",
                type: "integer[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterIds",
                table: "Encounters");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Characters",
                newName: "Sex");

            migrationBuilder.AddColumn<int>(
                name: "EncounterId",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_EncounterId",
                table: "Characters",
                column: "EncounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Encounters_EncounterId",
                table: "Characters",
                column: "EncounterId",
                principalTable: "Encounters",
                principalColumn: "Id");
        }
    }
}
