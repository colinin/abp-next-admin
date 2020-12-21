using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class RenameDataItemValueToDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "AppPlatformDataItems");

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "AppPlatformDataItems",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "AppPlatformDataItems");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "AppPlatformDataItems",
                type: "varchar(128) CHARACTER SET utf8mb4",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
