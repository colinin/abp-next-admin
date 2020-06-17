using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Abp.MessageService.Migrations
{
    public partial class AddNotificationColumnCateGory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppNotifications_NotificationName",
                table: "AppNotifications");

            migrationBuilder.AddColumn<string>(
                name: "NotificationCateGory",
                table: "AppNotifications",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AppNotifications_TenantId_NotificationName",
                table: "AppNotifications",
                columns: new[] { "TenantId", "NotificationName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppNotifications_TenantId_NotificationName",
                table: "AppNotifications");

            migrationBuilder.DropColumn(
                name: "NotificationCateGory",
                table: "AppNotifications");

            migrationBuilder.CreateIndex(
                name: "IX_AppNotifications_NotificationName",
                table: "AppNotifications",
                column: "NotificationName");
        }
    }
}
