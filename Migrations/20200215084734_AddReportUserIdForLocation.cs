using Microsoft.EntityFrameworkCore.Migrations;

namespace MozzieAiSystems.Migrations
{
    public partial class AddReportUserIdForLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath1",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ImagePath2",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ImagePath3",
                table: "Location");

            migrationBuilder.AddColumn<int>(
                name: "ReportUserId",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportUserId",
                table: "Location");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath1",
                table: "Location",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath2",
                table: "Location",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath3",
                table: "Location",
                maxLength: 200,
                nullable: true);
        }
    }
}
