using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class RenameRouteFieldPlantTypeToFramework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlatformType",
                table: "AppPlatformMenus");

            migrationBuilder.DropColumn(
                name: "PlatformType",
                table: "AppPlatformLayouts");

            migrationBuilder.AddColumn<string>(
                name: "Framework",
                table: "AppPlatformMenus",
                type: "varchar(64) CHARACTER SET utf8mb4",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Framework",
                table: "AppPlatformLayouts",
                type: "varchar(64) CHARACTER SET utf8mb4",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "AppPlatformDatas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "AppPlatformDataItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Framework",
                table: "AppPlatformMenus");

            migrationBuilder.DropColumn(
                name: "Framework",
                table: "AppPlatformLayouts");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "AppPlatformDatas");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "AppPlatformDataItems");

            migrationBuilder.AddColumn<int>(
                name: "PlatformType",
                table: "AppPlatformMenus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlatformType",
                table: "AppPlatformLayouts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
