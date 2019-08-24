using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Windows_Backend.Migrations
{
    public partial class AddCreationDateToEventAndPromotionAndLastLoginToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Creation",
                table: "Promotion",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Creation",
                table: "Events",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creation",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "Creation",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");
        }
    }
}
