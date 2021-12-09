using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.IdentityServer.Migrations
{
    public partial class UpgradeAbpTo500RC1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AbpUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AbpUsers");
        }
    }
}
