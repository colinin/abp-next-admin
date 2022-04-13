using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.WebhooksManagement.Migrations
{
    public partial class AddFieldRequestAndResponseHeadersWithSendAttempts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestHeaders",
                table: "AbpWebhooksSendAttempts",
                type: "longtext",
                maxLength: 2147483647,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ResponseHeaders",
                table: "AbpWebhooksSendAttempts",
                type: "longtext",
                maxLength: 2147483647,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestHeaders",
                table: "AbpWebhooksSendAttempts");

            migrationBuilder.DropColumn(
                name: "ResponseHeaders",
                table: "AbpWebhooksSendAttempts");
        }
    }
}
