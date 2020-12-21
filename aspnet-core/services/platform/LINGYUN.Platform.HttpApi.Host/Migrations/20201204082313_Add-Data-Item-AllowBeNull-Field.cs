using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class AddDataItemAllowBeNullField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Component",
                table: "AppPlatformMenus",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowBeNull",
                table: "AppPlatformDataItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowBeNull",
                table: "AppPlatformDataItems");

            migrationBuilder.AlterColumn<string>(
                name: "Component",
                table: "AppPlatformMenus",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);
        }
    }
}
