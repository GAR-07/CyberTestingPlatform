using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberTestingPlatform.DataAccess.Migrations
{
    public partial class updatingcourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Courses_CourseEntityId",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_CourseEntityId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "CourseEntityId",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Courses",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Lectures");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Courses",
                newName: "Title");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseEntityId",
                table: "Lectures",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "Courses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CourseEntityId",
                table: "Lectures",
                column: "CourseEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Courses_CourseEntityId",
                table: "Lectures",
                column: "CourseEntityId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
