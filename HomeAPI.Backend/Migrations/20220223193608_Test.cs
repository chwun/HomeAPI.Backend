using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAPI.Backend.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accountingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ParentCategoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountingCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accountingCategories_accountingCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "accountingCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "accountingEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimePeriod = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accountingEntries_accountingCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "accountingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accountingCategories_ParentCategoryId",
                table: "accountingCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_accountingEntries_CategoryId",
                table: "accountingEntries",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountingEntries");

            migrationBuilder.DropTable(
                name: "accountingCategories");
        }
    }
}
