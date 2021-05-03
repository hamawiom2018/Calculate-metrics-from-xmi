using System;
using master_project.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

namespace master_project.Migrations.XMIProject
{
    public partial class addMetricElementsWithData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetricElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Element = table.Column<int>(type: "int", nullable: false),
                    ElementName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MetricId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetricElements_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetricElements_MetricId",
                table: "MetricElements",
                column: "MetricId");

            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
            " VALUES(" + (int)AppEnums.Type.Package + ",'" + AppEnums.Type.Package.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NPM'))");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetricElements");
        }
    }
}
