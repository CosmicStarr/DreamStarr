using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StarrAPI.Data.Migrations
{
    public partial class passwordstodb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "GetAppUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "GetAppUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "GetAppUsers");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "GetAppUsers");
        }
    }
}
