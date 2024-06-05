using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.WebhooksManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeoutDurationToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeoutDuration",
                table: "AbpWebhooksSubscriptions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeoutDuration",
                table: "AbpWebhooksSubscriptions");
        }
    }
}
