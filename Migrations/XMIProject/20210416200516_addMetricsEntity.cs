using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace master_project.Migrations.XMIProject
{
    public partial class addMetricsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetricCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetricName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetricDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetType = table.Column<int>(type: "int", nullable: false),
                    TargetTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}
