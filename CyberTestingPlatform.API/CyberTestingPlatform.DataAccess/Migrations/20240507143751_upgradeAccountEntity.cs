using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberTestingPlatform.DataAccess.Migrations
{
    public partial class upgradeAccountEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Accounts");
        }
    }
}
