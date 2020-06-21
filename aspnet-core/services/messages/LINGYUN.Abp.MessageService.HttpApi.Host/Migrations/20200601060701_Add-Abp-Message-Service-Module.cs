using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Abp.MessageService.Migrations
{
    public partial class AddAbpMessageServiceModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppNotifications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(nullable: true),
                    Severity = table.Column<sbyte>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    NotificationId = table.Column<long>(nullable: false),
                    NotificationName = table.Column<string>(maxLength: 100, nullable: false),
                    NotificationData = table.Column<string>(maxLength: 1048576, nullable: false),
                    NotificationTypeName = table.Column<string>(maxLength: 512, nullable: false),
                    ExpirationTime = table.Column<DateTime>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserNotifications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    NotificationId = table.Column<long>(nullable: false),
                    ReadStatus = table.Column<sbyte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserSubscribes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    NotificationName = table.Column<string>(maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserSubscribes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppNotifications_NotificationName",
                table: "AppNotifications",
                column: "NotificationName");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_User_Notification_Id",
                table: "AppUserNotifications",
                columns: new[] { "TenantId", "UserId", "NotificationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_User_Notification_Name",
                table: "AppUserSubscribes",
                columns: new[] { "TenantId", "UserId", "NotificationName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppNotifications");

            migrationBuilder.DropTable(
                name: "AppUserNotifications");

            migrationBuilder.DropTable(
                name: "AppUserSubscribes");
        }
    }
}
