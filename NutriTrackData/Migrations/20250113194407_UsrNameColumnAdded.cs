using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriTrackData.Migrations
{
    /// <inheritdoc />
    public partial class UsrNameColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalActivities_AspNetUsers_UserId",
                table: "PhysicalActivities");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PhysicalActivities",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_PhysicalActivities_UserId",
                table: "PhysicalActivities",
                newName: "IX_PhysicalActivities_UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalActivities_AspNetUsers_UserName",
                table: "PhysicalActivities",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalActivities_AspNetUsers_UserName",
                table: "PhysicalActivities");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "PhysicalActivities",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PhysicalActivities_UserName",
                table: "PhysicalActivities",
                newName: "IX_PhysicalActivities_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalActivities_AspNetUsers_UserId",
                table: "PhysicalActivities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
