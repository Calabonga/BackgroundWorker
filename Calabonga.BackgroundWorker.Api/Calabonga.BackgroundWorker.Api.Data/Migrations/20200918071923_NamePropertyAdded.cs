using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Calabonga.BackgroundWorker.Api.Data.Migrations
{
    public partial class NamePropertyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Works",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Works",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Works");
        }
    }
}
