using Microsoft.EntityFrameworkCore.Migrations;

namespace Windows_Backend.Migrations
{
    public partial class AddSeperatedStartAndEndDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartAndEndDate",
                table: "Promotion",
                newName: "StartDate");

            migrationBuilder.AddColumn<string>(
                name: "EndDate",
                table: "Promotion",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Promotion");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Promotion",
                newName: "StartAndEndDate");
        }
    }
}
