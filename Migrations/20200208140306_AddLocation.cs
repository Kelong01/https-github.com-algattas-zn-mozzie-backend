using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MozzieAiSystems.Migrations
{
    public partial class AddLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Uuid = table.Column<string>(maxLength: 200, nullable: true),
                    Lng = table.Column<double>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    ReportDateTime = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(maxLength: 1000, nullable: true),
                    AdditionalInfo = table.Column<string>(maxLength: 1000, nullable: true),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    ImagePath1 = table.Column<string>(maxLength: 200, nullable: true),
                    ImagePath2 = table.Column<string>(maxLength: 200, nullable: true),
                    ImagePath3 = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
