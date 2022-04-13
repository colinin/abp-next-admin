using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.WebhooksManagement.Migrations
{
    public partial class AddFieldSendExactSameDataWithSendAttempts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendExactSameData",
                table: "AbpWebhooksSendAttempts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendExactSameData",
                table: "AbpWebhooksSendAttempts");
        }
    }
}
