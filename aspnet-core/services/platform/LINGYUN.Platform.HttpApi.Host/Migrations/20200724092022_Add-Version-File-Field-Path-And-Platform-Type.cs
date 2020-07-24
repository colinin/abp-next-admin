using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class AddVersionFileFieldPathAndPlatformType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppPlatformVersionFile_Name_Version",
                table: "AppPlatformVersionFile");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "AppPlatformVersionFile",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlatformType",
                table: "AppPlatformVersion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformVersionFile_Path_Name_Version",
                table: "AppPlatformVersionFile",
                columns: new[] { "Path", "Name", "Version" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppPlatformVersionFile_Path_Name_Version",
                table: "AppPlatformVersionFile");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "AppPlatformVersionFile");

            migrationBuilder.DropColumn(
                name: "PlatformType",
                table: "AppPlatformVersion");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformVersionFile_Name_Version",
                table: "AppPlatformVersionFile",
                columns: new[] { "Name", "Version" });
        }
    }
}
