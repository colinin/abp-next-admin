using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.WebhooksManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpWebhooksSubscriptions",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AbpWebhooksSubscriptions",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AbpWebhooksSubscriptions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AbpWebhooksSubscriptions");
        }
    }
}
