using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Thornless.Data.GeneratorRepo.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterAncestry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Copyright = table.Column<string>(nullable: true),
                    FlavorHtml = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTimeOffset>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAncestry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAncestryNamePart",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RandomizationWeight = table.Column<int>(nullable: false),
                    CharacterAncestryId = table.Column<int>(nullable: false),
                    NameSegmentCode = table.Column<string>(nullable: true),
                    NamePartsJson = table.Column<string>(nullable: true),
                    NameMeaningsJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAncestryNamePart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterAncestryNamePart_CharacterAncestry_CharacterAncestryId",
                        column: x => x.CharacterAncestryId,
                        principalTable: "CharacterAncestry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAncestryOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CharacterAncestryId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NamePartSeperatorJson = table.Column<string>(nullable: true),
                    SeperatorChancePercentage = table.Column<int>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAncestryOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterAncestryOption_CharacterAncestry_CharacterAncestryId",
                        column: x => x.CharacterAncestryId,
                        principalTable: "CharacterAncestry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAncestrySegmentGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RandomizationWeight = table.Column<int>(nullable: false),
                    CharacterAncestryOptionId = table.Column<int>(nullable: false),
                    NameSegmentCodesJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAncestrySegmentGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterAncestrySegmentGroup_CharacterAncestryOption_CharacterAncestryOptionId",
                        column: x => x.CharacterAncestryOptionId,
                        principalTable: "CharacterAncestryOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAncestryNamePart_CharacterAncestryId",
                table: "CharacterAncestryNamePart",
                column: "CharacterAncestryId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAncestryOption_CharacterAncestryId",
                table: "CharacterAncestryOption",
                column: "CharacterAncestryId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAncestrySegmentGroup_CharacterAncestryOptionId",
                table: "CharacterAncestrySegmentGroup",
                column: "CharacterAncestryOptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterAncestryNamePart");

            migrationBuilder.DropTable(
                name: "CharacterAncestrySegmentGroup");

            migrationBuilder.DropTable(
                name: "CharacterAncestryOption");

            migrationBuilder.DropTable(
                name: "CharacterAncestry");
        }
    }
}
