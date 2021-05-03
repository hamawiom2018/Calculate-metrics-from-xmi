using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace master_project.Migrations.XMIProject
{
    public partial class InitialCreateXMIProjectContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UploadName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagramName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileContent = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uploads", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Uploads");
        }
    }
}
