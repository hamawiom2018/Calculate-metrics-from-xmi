// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using master_project.Data.XMI_Project;

namespace master_project.Migrations.XMIProject
{
    [DbContext(typeof(XMIProjectContext))]
    [Migration("20210416200516_addMetricsEntity")]
    partial class addMetricsEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("master_project.Data.XMI_Project.Metric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MetricCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetricDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetricName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TargetType")
                        .HasColumnType("int");

                    b.Property<string>("TargetTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("master_project.Data.XMI_Project.Upload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DiagramName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("FileContent")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UploadName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Uploads");
                });
#pragma warning restore 612, 618
        }
    }
}
