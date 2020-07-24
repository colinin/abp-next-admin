using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class ModifyVersionFileSHA256Length : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SHA256",
                table: "AppPlatformVersionFile",
                maxLength: 65,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32) CHARACTER SET utf8mb4",
                oldMaxLength: 32);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SHA256",
                table: "AppPlatformVersionFile",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 65);
        }
    }
}
