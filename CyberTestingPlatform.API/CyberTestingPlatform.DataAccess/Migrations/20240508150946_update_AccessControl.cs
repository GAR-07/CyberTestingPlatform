using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberTestingPlatform.DataAccess.Migrations
{
    public partial class update_AccessControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessType",
                table: "AccessControl",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "AccessControl",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "AccessControl",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessType",
                table: "AccessControl");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "AccessControl");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "AccessControl");
        }
    }
}
