using Microsoft.EntityFrameworkCore.Migrations;

namespace Windows_Backend.Migrations
{
    public partial class AddImageUrlToBusiness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Business",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Business");
        }
    }
}
