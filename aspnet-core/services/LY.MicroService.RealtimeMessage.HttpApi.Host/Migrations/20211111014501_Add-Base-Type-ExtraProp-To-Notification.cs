using Microsoft.EntityFrameworkCore.Migrations;

namespace LY.MicroService.RealtimeMessage.Migrations
{
    public partial class AddBaseTypeExtraPropToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationData",
                table: "AppNotifications");

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AppNotifications",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AppNotifications");

            migrationBuilder.AddColumn<string>(
                name: "NotificationData",
                table: "AppNotifications",
                type: "longtext",
                maxLength: 1048576,
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
