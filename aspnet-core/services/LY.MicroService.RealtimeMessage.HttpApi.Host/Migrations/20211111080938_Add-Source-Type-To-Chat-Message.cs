using Microsoft.EntityFrameworkCore.Migrations;

namespace LY.MicroService.RealtimeMessage.Migrations
{
    public partial class AddSourceTypeToChatMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "AppUserMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "AppGroupMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "AppUserMessages");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "AppGroupMessages");
        }
    }
}
