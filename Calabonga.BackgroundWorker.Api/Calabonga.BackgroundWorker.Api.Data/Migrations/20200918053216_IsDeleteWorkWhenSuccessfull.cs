using Microsoft.EntityFrameworkCore.Migrations;

namespace Calabonga.BackgroundWorker.Api.Data.Migrations
{
    public partial class IsDeleteWorkWhenSuccessfull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleteAfterSuccessfulCompleted",
                table: "Works",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleteAfterSuccessfulCompleted",
                table: "Works");
        }
    }
}
