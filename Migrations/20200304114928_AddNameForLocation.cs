using Microsoft.EntityFrameworkCore.Migrations;

namespace MozzieAiSystems.Migrations
{
    public partial class AddNameForLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Location",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Location");
        }
    }
}
