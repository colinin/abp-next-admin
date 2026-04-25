using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LINGYUN.Abp.MicroService.AIService.Migrations
{
    /// <inheritdoc />
    public partial class Initial_AI_Service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAIConversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAIConversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpAITextChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Workspace = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReplyMessage = table.Column<string>(type: "text", nullable: true),
                    ReplyAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAITextChatMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpAITokenUsages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    InputTokenCount = table.Column<long>(type: "bigint", nullable: true),
                    OutputTokenCount = table.Column<long>(type: "bigint", nullable: true),
                    TotalTokenCount = table.Column<long>(type: "bigint", nullable: true),
                    CachedInputTokenCount = table.Column<long>(type: "bigint", nullable: true),
                    ReasoningTokenCount = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAITokenUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpAIWorkspaceDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Provider = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ModelName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ApiKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ApiBaseUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SystemPrompt = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Instructions = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Temperature = table.Column<float>(type: "real", nullable: true),
                    MaxOutputTokens = table.Column<int>(type: "integer", nullable: true),
                    FrequencyPenalty = table.Column<float>(type: "real", nullable: true),
                    PresencePenalty = table.Column<float>(type: "real", nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    StateCheckers = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAIWorkspaceDefinitions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAITextChatMessages_TenantId_ConversationId",
                table: "AbpAITextChatMessages",
                columns: new[] { "TenantId", "ConversationId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAITokenUsages_TenantId_ConversationId",
                table: "AbpAITokenUsages",
                columns: new[] { "TenantId", "ConversationId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAIWorkspaceDefinitions_Name",
                table: "AbpAIWorkspaceDefinitions",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAIConversations");

            migrationBuilder.DropTable(
                name: "AbpAITextChatMessages");

            migrationBuilder.DropTable(
                name: "AbpAITokenUsages");

            migrationBuilder.DropTable(
                name: "AbpAIWorkspaceDefinitions");
        }
    }
}
