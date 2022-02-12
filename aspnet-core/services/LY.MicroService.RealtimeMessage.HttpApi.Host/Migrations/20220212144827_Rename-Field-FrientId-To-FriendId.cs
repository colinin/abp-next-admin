using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.RealtimeMessage.Migrations
{
    public partial class RenameFieldFrientIdToFriendId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequireAddFriendValition",
                table: "AppUserChatSettings",
                newName: "RequireAddFriendValidation");

            migrationBuilder.RenameColumn(
                name: "FrientId",
                table: "AppUserChatFriends",
                newName: "FriendId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserChatFriends_TenantId_UserId_FrientId",
                table: "AppUserChatFriends",
                newName: "IX_AppUserChatFriends_TenantId_UserId_FriendId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequireAddFriendValidation",
                table: "AppUserChatSettings",
                newName: "RequireAddFriendValition");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "AppUserChatFriends",
                newName: "FrientId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserChatFriends_TenantId_UserId_FriendId",
                table: "AppUserChatFriends",
                newName: "IX_AppUserChatFriends_TenantId_UserId_FrientId");
        }
    }
}
