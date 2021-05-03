using master_project.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

namespace master_project.Migrations.XMIProject
{
    public partial class metricElementsSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
                        " VALUES(" + (int)AppEnums.Type.Class + ",'" + AppEnums.Type.Class.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NCM'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
                        " VALUES(" + (int)AppEnums.Type.Actor + ",'" + AppEnums.Type.Actor.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NAM'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
                                    " VALUES(" + (int)AppEnums.Type.UseCase + ",'" + AppEnums.Type.UseCase.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NUM'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
                                                " VALUES(" + (int)AppEnums.Type.Object + ",'" + AppEnums.Type.Object.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NOM'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
                                                            " VALUES(" + (int)AppEnums.Type.Message + ",'" + AppEnums.Type.Message.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NMM'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
                                                                        " VALUES(" + (int)AppEnums.Type.Association + ",'" + AppEnums.Type.Association.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NASM'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
            " VALUES(" + (int)AppEnums.Type.Association + ",'" + AppEnums.Type.Association.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NAGM'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
            " VALUES(" + (int)AppEnums.Type.Generalization + ",'" + AppEnums.Type.Generalization.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'NIM'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
                       " VALUES(" + (int)AppEnums.Type.Class + ",'" + AppEnums.Type.Class.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'MHF'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
            " VALUES(" + (int)AppEnums.Type.Class + ",'" + AppEnums.Type.Class.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'AHF'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
            " VALUES(" + (int)AppEnums.Type.Class + ",'" + AppEnums.Type.Class.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'MIF'))");
            migrationBuilder.Sql("INSERT INTO MetricElements (Element,ElementName,CreatedDate,UpdatedDate,MetricId)" +
            " VALUES(" + (int)AppEnums.Type.Class + ",'" + AppEnums.Type.Class.ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,(SELECT Id FROM Metrics WHERE MetricCode= 'AIF'))");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
