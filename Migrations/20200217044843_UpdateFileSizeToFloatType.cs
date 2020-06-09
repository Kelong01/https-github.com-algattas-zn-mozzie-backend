using Microsoft.EntityFrameworkCore.Migrations;

namespace MozzieAiSystems.Migrations
{
    public partial class UpdateFileSizeToFloatType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "FileSize",
                table: "LocationFile",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "LocationFile",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
