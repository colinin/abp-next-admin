using Microsoft.EntityFrameworkCore.Migrations;

namespace LY.MicroService.RealtimeMessage.Migrations
{
    public partial class RenameFieldSendStateToState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SendState",
                table: "AppUserMessages",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "SendState",
                table: "AppGroupMessages",
                newName: "State");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "AppUserMessages",
                newName: "SendState");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "AppGroupMessages",
                newName: "SendState");
        }
    }
}
