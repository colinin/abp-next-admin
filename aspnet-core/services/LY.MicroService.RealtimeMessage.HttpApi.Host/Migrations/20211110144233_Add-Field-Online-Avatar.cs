using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LY.MicroService.RealtimeMessage.Migrations
{
    public partial class AddFieldOnlineAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastOnlineTime",
                table: "AppUserChatCards",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "AppUserChatCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "AppChatGroups",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastOnlineTime",
                table: "AppUserChatCards");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AppUserChatCards");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "AppChatGroups");
        }
    }
}
