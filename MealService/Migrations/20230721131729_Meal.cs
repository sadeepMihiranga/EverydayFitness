using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealService.Migrations
{
    public partial class Meal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheatMeals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealPortionSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealReasonId = table.Column<long>(type: "bigint", nullable: false),
                    CheatMealSatisfcation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaloriesTaken = table.Column<int>(type: "int", nullable: false),
                    DateTimeTaken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheatMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheatMeals_CheatMealReasons_MealReasonId",
                        column: x => x.MealReasonId,
                        principalTable: "CheatMealReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheatMeals_CheatMealTypes_MealTypeId",
                        column: x => x.MealTypeId,
                        principalTable: "CheatMealTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheatMeals_MealReasonId",
                table: "CheatMeals",
                column: "MealReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_CheatMeals_MealTypeId",
                table: "CheatMeals",
                column: "MealTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheatMeals");
        }
    }
}
