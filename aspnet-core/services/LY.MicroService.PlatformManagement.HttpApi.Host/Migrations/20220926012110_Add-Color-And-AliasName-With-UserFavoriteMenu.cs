using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.PlatformManagement.Migrations
{
    public partial class AddColorAndAliasNameWithUserFavoriteMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppPlatformUserFavoriteMenus",
                keyColumn: "Path",
                keyValue: null,
                column: "Path",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "AppPlatformUserFavoriteMenus",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AliasName",
                table: "AppPlatformUserFavoriteMenus",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "AppPlatformUserFavoriteMenus",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AliasName",
                table: "AppPlatformUserFavoriteMenus");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "AppPlatformUserFavoriteMenus");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "AppPlatformUserFavoriteMenus",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
