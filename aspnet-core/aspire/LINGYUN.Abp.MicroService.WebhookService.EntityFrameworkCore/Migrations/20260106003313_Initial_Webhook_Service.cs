using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LINGYUN.Abp.MicroService.WebhookService.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Webhook_Service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpWebhooksEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    WebhookName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Data = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhooksEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhooksSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    WebhookUri = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Secret = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Webhooks = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    Headers = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    TimeoutDuration = table.Column<int>(type: "integer", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhooksSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhooksWebhookGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhooksWebhookGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhooksWebhooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    RequiredFeatures = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhooksWebhooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhooksSendAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    WebhookEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    WebhookSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Response = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    ResponseStatusCode = table.Column<int>(type: "integer", nullable: true),
                    RequestHeaders = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    ResponseHeaders = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    SendExactSameData = table.Column<bool>(type: "boolean", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhooksSendAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpWebhooksSendAttempts_AbpWebhooksEvents_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "AbpWebhooksEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpWebhooksSendAttempts_WebhookEventId",
                table: "AbpWebhooksSendAttempts",
                column: "WebhookEventId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpWebhooksWebhookGroups_Name",
                table: "AbpWebhooksWebhookGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpWebhooksWebhooks_GroupName",
                table: "AbpWebhooksWebhooks",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpWebhooksWebhooks_Name",
                table: "AbpWebhooksWebhooks",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpWebhooksSendAttempts");

            migrationBuilder.DropTable(
                name: "AbpWebhooksSubscriptions");

            migrationBuilder.DropTable(
                name: "AbpWebhooksWebhookGroups");

            migrationBuilder.DropTable(
                name: "AbpWebhooksWebhooks");

            migrationBuilder.DropTable(
                name: "AbpWebhooksEvents");
        }
    }
}
