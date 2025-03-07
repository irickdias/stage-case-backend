using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateProcessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Sectors_Sectorid",
                table: "Processes");

            migrationBuilder.RenameColumn(
                name: "Tools",
                table: "Processes",
                newName: "tools");

            migrationBuilder.RenameColumn(
                name: "Sectorid",
                table: "Processes",
                newName: "sectorId");

            migrationBuilder.RenameColumn(
                name: "Responsibles",
                table: "Processes",
                newName: "responsibles");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Processes",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Documentation",
                table: "Processes",
                newName: "documentation");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Processes",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Processes_Sectorid",
                table: "Processes",
                newName: "IX_Processes_sectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Sectors_sectorId",
                table: "Processes",
                column: "sectorId",
                principalTable: "Sectors",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Sectors_sectorId",
                table: "Processes");

            migrationBuilder.RenameColumn(
                name: "tools",
                table: "Processes",
                newName: "Tools");

            migrationBuilder.RenameColumn(
                name: "sectorId",
                table: "Processes",
                newName: "Sectorid");

            migrationBuilder.RenameColumn(
                name: "responsibles",
                table: "Processes",
                newName: "Responsibles");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Processes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "documentation",
                table: "Processes",
                newName: "Documentation");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Processes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Processes_sectorId",
                table: "Processes",
                newName: "IX_Processes_Sectorid");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Sectors_Sectorid",
                table: "Processes",
                column: "Sectorid",
                principalTable: "Sectors",
                principalColumn: "id");
        }
    }
}
