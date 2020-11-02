using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Abp.MessageService.Migrations
{
    public partial class AddUserChatFriendColumnDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserChatBlacks");

            migrationBuilder.DropTable(
                name: "AppUserSpecialFocuss");

            migrationBuilder.DropColumn(
                name: "NotificationCateGory",
                table: "AppNotifications");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AppUserChatFriends",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppUserChatFriends",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AppUserChatFriends",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "AppUserChatFriends",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AppUserChatFriends");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppUserChatFriends");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AppUserChatFriends");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppUserChatFriends");

            migrationBuilder.AddColumn<string>(
                name: "NotificationCateGory",
                table: "AppNotifications",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppUserChatBlacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ShieldUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserChatBlacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserSpecialFocuss",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    FocusUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserSpecialFocuss", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserChatBlacks_TenantId_UserId",
                table: "AppUserChatBlacks",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserSpecialFocuss_TenantId_UserId",
                table: "AppUserSpecialFocuss",
                columns: new[] { "TenantId", "UserId" });
        }
    }
}
