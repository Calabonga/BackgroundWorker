using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Calabonga.BackgroundWorker.Api.Data.Migrations
{
    public partial class EntityWorkAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Parameters = table.Column<string>(nullable: true),
                    Dependency = table.Column<string>(nullable: true),
                    StartAfterMinutes = table.Column<int>(nullable: false),
                    WorkType = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    ProcessedAt = table.Column<DateTime>(nullable: false),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    CanceledAt = table.Column<DateTime>(nullable: true),
                    ProcessingResult = table.Column<string>(nullable: true),
                    ProcessedCount = table.Column<int>(nullable: false),
                    CancelAfterProcessingCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_Works_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_ParentId",
                table: "Works",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Works");
        }
    }
}
