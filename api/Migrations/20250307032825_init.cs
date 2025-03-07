using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.id);
                    table.ForeignKey(
                        name: "FK_Sectors_Departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Departments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Tools = table.Column<string>(type: "varchar(100)", nullable: false),
                    Responsibles = table.Column<string>(type: "varchar(200)", nullable: false),
                    Documentation = table.Column<string>(type: "varchar(500)", nullable: false),
                    prioprity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    finished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    parentProcessId = table.Column<int>(type: "int", nullable: true),
                    Sectorid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processes_Processes_parentProcessId",
                        column: x => x.parentProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Processes_Sectors_Sectorid",
                        column: x => x.Sectorid,
                        principalTable: "Sectors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processes_parentProcessId",
                table: "Processes",
                column: "parentProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_Sectorid",
                table: "Processes",
                column: "Sectorid");

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_departmentId",
                table: "Sectors",
                column: "departmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
