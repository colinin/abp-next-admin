using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class AddLayoutIdColumnToMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LayoutId",
                table: "AppPlatformMenus",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LayoutId",
                table: "AppPlatformMenus");
        }
    }
}
