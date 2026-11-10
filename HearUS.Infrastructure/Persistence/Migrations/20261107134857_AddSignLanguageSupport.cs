using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearUS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSignLanguageSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SignVideoUrl",
                table: "Posts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignVideoUrl",
                table: "Jobs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignVideoUrl",
                table: "Insights",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SignTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignTerms_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignTerms_Category_Term",
                table: "SignTerms",
                columns: new[] { "Category", "Term" });

            migrationBuilder.CreateIndex(
                name: "IX_SignTerms_CreatedById",
                table: "SignTerms",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignTerms");

            migrationBuilder.DropColumn(
                name: "SignVideoUrl",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SignVideoUrl",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "SignVideoUrl",
                table: "Insights");
        }
    }
}
