using Microsoft.EntityFrameworkCore.Migrations;

namespace LY.MicroService.RealtimeMessage.Migrations
{
    public partial class AddFieldStaticToChatFriend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "AppUserChatFriends",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "AppUserChatFriends");
        }
    }
}
