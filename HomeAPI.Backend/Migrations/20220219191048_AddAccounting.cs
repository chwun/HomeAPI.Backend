using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeAPI.Backend.Migrations
{
    public partial class AddAccounting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accountingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountingCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "accountingSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountingSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accountingSubCategories_accountingCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "accountingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accountingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SubCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accountingItems_accountingSubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "accountingSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accountingEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimePeriod = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accountingEntries_accountingItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "accountingItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accountingSubEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<decimal>(type: "TEXT", nullable: false),
                    EntryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountingSubEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accountingSubEntries_accountingEntries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "accountingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accountingEntries_ItemId",
                table: "accountingEntries",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_accountingItems_SubCategoryId",
                table: "accountingItems",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_accountingSubCategories_CategoryId",
                table: "accountingSubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_accountingSubEntries_EntryId",
                table: "accountingSubEntries",
                column: "EntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountingSubEntries");

            migrationBuilder.DropTable(
                name: "accountingEntries");

            migrationBuilder.DropTable(
                name: "accountingItems");

            migrationBuilder.DropTable(
                name: "accountingSubCategories");

            migrationBuilder.DropTable(
                name: "accountingCategories");
        }
    }
}
