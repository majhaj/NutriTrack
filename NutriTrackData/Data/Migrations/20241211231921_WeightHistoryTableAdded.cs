using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class WeightHistoryTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealProduct_Meals_MealId",
                table: "MealProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_MealProduct_Product_ProductId",
                table: "MealProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalActivity_AspNetUsers_UserId",
                table: "PhysicalActivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhysicalActivity",
                table: "PhysicalActivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealProduct",
                table: "MealProduct");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "PhysicalActivity",
                newName: "PhysicalActivities");

            migrationBuilder.RenameTable(
                name: "MealProduct",
                newName: "MealProducts");

            migrationBuilder.RenameIndex(
                name: "IX_PhysicalActivity_UserId",
                table: "PhysicalActivities",
                newName: "IX_PhysicalActivities_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MealProduct_ProductId",
                table: "MealProducts",
                newName: "IX_MealProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhysicalActivities",
                table: "PhysicalActivities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealProducts",
                table: "MealProducts",
                columns: new[] { "MealId", "ProductId" });

            migrationBuilder.CreateTable(
                name: "WeightHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightHistories_UserId",
                table: "WeightHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Meals_MealId",
                table: "MealProducts",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Products_ProductId",
                table: "MealProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalActivities_AspNetUsers_UserId",
                table: "PhysicalActivities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Meals_MealId",
                table: "MealProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Products_ProductId",
                table: "MealProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalActivities_AspNetUsers_UserId",
                table: "PhysicalActivities");

            migrationBuilder.DropTable(
                name: "WeightHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhysicalActivities",
                table: "PhysicalActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealProducts",
                table: "MealProducts");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "PhysicalActivities",
                newName: "PhysicalActivity");

            migrationBuilder.RenameTable(
                name: "MealProducts",
                newName: "MealProduct");

            migrationBuilder.RenameIndex(
                name: "IX_PhysicalActivities_UserId",
                table: "PhysicalActivity",
                newName: "IX_PhysicalActivity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MealProducts_ProductId",
                table: "MealProduct",
                newName: "IX_MealProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhysicalActivity",
                table: "PhysicalActivity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealProduct",
                table: "MealProduct",
                columns: new[] { "MealId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MealProduct_Meals_MealId",
                table: "MealProduct",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealProduct_Product_ProductId",
                table: "MealProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalActivity_AspNetUsers_UserId",
                table: "PhysicalActivity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
