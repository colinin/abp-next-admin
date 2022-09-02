using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.RealtimeMessage.Migrations
{
    public partial class AddGroupNameWithNotificationDefinitionRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "AppNotificationDefinitions",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "AppNotificationDefinitions");
        }
    }
}
