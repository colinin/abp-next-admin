using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.PlatformManagement.Migrations
{
    public partial class AddFieldStartupWithUserAndRoleMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Startup",
                table: "AppPlatformUserMenus",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Startup",
                table: "AppPlatformRoleMenus",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Startup",
                table: "AppPlatformUserMenus");

            migrationBuilder.DropColumn(
                name: "Startup",
                table: "AppPlatformRoleMenus");
        }
    }
}
