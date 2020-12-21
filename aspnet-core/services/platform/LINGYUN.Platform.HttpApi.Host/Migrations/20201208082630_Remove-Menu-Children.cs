using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class RemoveMenuChildren : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPlatformMenus_AppPlatformMenus_ParentId",
                table: "AppPlatformMenus");

            migrationBuilder.DropIndex(
                name: "IX_AppPlatformMenus_ParentId",
                table: "AppPlatformMenus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformMenus_ParentId",
                table: "AppPlatformMenus",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPlatformMenus_AppPlatformMenus_ParentId",
                table: "AppPlatformMenus",
                column: "ParentId",
                principalTable: "AppPlatformMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
