using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeAbpFrameworkTo936 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceSecrets");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopeClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopeProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerClientClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerClientCorsOrigins");

            migrationBuilder.DropTable(
                name: "IdentityServerClientGrantTypes");

            migrationBuilder.DropTable(
                name: "IdentityServerClientIdPRestrictions");

            migrationBuilder.DropTable(
                name: "IdentityServerClientPostLogoutRedirectUris");

            migrationBuilder.DropTable(
                name: "IdentityServerClientProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerClientRedirectUris");

            migrationBuilder.DropTable(
                name: "IdentityServerClientScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerClientSecrets");

            migrationBuilder.DropTable(
                name: "IdentityServerDeviceFlowCodes");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResourceClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResourceProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerPersistedGrants");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResources");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerClients");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResources");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "ValueRange",
                table: "AbpAuthEntityProperties");

            migrationBuilder.RenameColumn(
                name: "AllowProperties",
                table: "AbpAuthRoleEntityRules",
                newName: "AccessedProperties");

            migrationBuilder.RenameColumn(
                name: "AllowProperties",
                table: "AbpAuthOrganizationUnitEntityRules",
                newName: "AccessedProperties");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddresses",
                table: "AbpSessions",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AbpSessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AbpRoles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AbpClaimTypes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "JavaScriptType",
                table: "AbpAuthEntityProperties",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AbpAuditLogExcelFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    FileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogExcelFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpAuthEntityEnums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PropertyInfoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuthEntityEnums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpAuthEntityEnums_AbpAuthEntityProperties_PropertyInfoId",
                        column: x => x.PropertyInfoId,
                        principalTable: "AbpAuthEntityProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpAuthSubjectStrategys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubjectName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SubjectId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Strategy = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuthSubjectStrategys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpGdprRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReadyTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpGdprRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformEmailMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Subject = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsBodyHtml = table.Column<bool>(type: "boolean", nullable: false),
                    CC = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Normalize = table.Column<bool>(type: "boolean", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: true),
                    BodyTransferEncoding = table.Column<int>(type: "integer", nullable: true),
                    DeliveryNotificationOptions = table.Column<int>(type: "integer", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Sender = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Receiver = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SendTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SendCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformEmailMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformFeedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformFeedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformSmsMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Sender = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Receiver = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SendTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SendCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformSmsMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Demo_BooksAuths",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    EntityId = table.Column<Guid>(type: "uuid", maxLength: 64, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Role = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    OrganizationUnit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demo_BooksAuths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demo_BooksAuths_Demo_Books_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Demo_Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpGdprInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    Provider = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpGdprInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpGdprInfos_AbpGdprRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "AbpGdprRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformEmailMessageAttachments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    BlobName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformEmailMessageAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlatformEmailMessageAttachments_AppPlatformEmailMessages~",
                        column: x => x.MessageId,
                        principalTable: "AppPlatformEmailMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformEmailMessageHeaders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformEmailMessageHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlatformEmailMessageHeaders_AppPlatformEmailMessages_Mes~",
                        column: x => x.MessageId,
                        principalTable: "AppPlatformEmailMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformFeedbackAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    FeedbackId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformFeedbackAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlatformFeedbackAttachments_AppPlatformFeedbacks_Feedbac~",
                        column: x => x.FeedbackId,
                        principalTable: "AppPlatformFeedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformFeedbackComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Capacity = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FeedbackId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformFeedbackComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlatformFeedbackComments_AppPlatformFeedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "AppPlatformFeedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuthEntityEnums_PropertyInfoId_Name",
                table: "AbpAuthEntityEnums",
                columns: new[] { "PropertyInfoId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpGdprInfos_RequestId",
                table: "AbpGdprInfos",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpGdprRequests_UserId",
                table: "AbpGdprRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformEmailMessageAttachments_MessageId",
                table: "AppPlatformEmailMessageAttachments",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformEmailMessageHeaders_MessageId",
                table: "AppPlatformEmailMessageHeaders",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformFeedbackAttachments_FeedbackId",
                table: "AppPlatformFeedbackAttachments",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformFeedbackComments_FeedbackId",
                table: "AppPlatformFeedbackComments",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Demo_BooksAuths_EntityId",
                table: "Demo_BooksAuths",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Demo_BooksAuths_OrganizationUnit",
                table: "Demo_BooksAuths",
                column: "OrganizationUnit");

            migrationBuilder.CreateIndex(
                name: "IX_Demo_BooksAuths_Role",
                table: "Demo_BooksAuths",
                column: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuditLogExcelFiles");

            migrationBuilder.DropTable(
                name: "AbpAuthEntityEnums");

            migrationBuilder.DropTable(
                name: "AbpAuthSubjectStrategys");

            migrationBuilder.DropTable(
                name: "AbpGdprInfos");

            migrationBuilder.DropTable(
                name: "AppPlatformEmailMessageAttachments");

            migrationBuilder.DropTable(
                name: "AppPlatformEmailMessageHeaders");

            migrationBuilder.DropTable(
                name: "AppPlatformFeedbackAttachments");

            migrationBuilder.DropTable(
                name: "AppPlatformFeedbackComments");

            migrationBuilder.DropTable(
                name: "AppPlatformSmsMessages");

            migrationBuilder.DropTable(
                name: "Demo_BooksAuths");

            migrationBuilder.DropTable(
                name: "AbpGdprRequests");

            migrationBuilder.DropTable(
                name: "AppPlatformEmailMessages");

            migrationBuilder.DropTable(
                name: "AppPlatformFeedbacks");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AbpSessions");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AbpRoles");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AbpClaimTypes");

            migrationBuilder.DropColumn(
                name: "JavaScriptType",
                table: "AbpAuthEntityProperties");

            migrationBuilder.RenameColumn(
                name: "AccessedProperties",
                table: "AbpAuthRoleEntityRules",
                newName: "AllowProperties");

            migrationBuilder.RenameColumn(
                name: "AccessedProperties",
                table: "AbpAuthOrganizationUnitEntityRules",
                newName: "AllowProperties");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "OpenIddictTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "OpenIddictTokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "OpenIddictTokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "OpenIddictTokens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OpenIddictTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "OpenIddictTokens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "OpenIddictTokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "OpenIddictAuthorizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "OpenIddictAuthorizations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "OpenIddictAuthorizations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "OpenIddictAuthorizations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OpenIddictAuthorizations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "OpenIddictAuthorizations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "OpenIddictAuthorizations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddresses",
                table: "AbpSessions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValueRange",
                table: "AbpAuthEntityProperties",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Emphasize = table.Column<bool>(type: "boolean", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                    AccessTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                    AccessTokenType = table.Column<int>(type: "integer", nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(type: "boolean", nullable: false),
                    AllowOfflineAccess = table.Column<bool>(type: "boolean", nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(type: "boolean", nullable: false),
                    AllowRememberConsent = table.Column<bool>(type: "boolean", nullable: false),
                    AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "boolean", nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(type: "boolean", nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(type: "integer", nullable: false),
                    BackChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false),
                    BackChannelLogoutUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ClientClaimsPrefix = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClientName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ClientUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ConsentLifetime = table.Column<int>(type: "integer", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DeviceCodeLifetime = table.Column<int>(type: "integer", nullable: false),
                    EnableLocalLogin = table.Column<bool>(type: "boolean", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false),
                    FrontChannelLogoutUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IdentityTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                    IncludeJwtId = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    LogoUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    PairWiseSubjectSalt = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProtocolType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RefreshTokenExpiration = table.Column<int>(type: "integer", nullable: false),
                    RefreshTokenUsage = table.Column<int>(type: "integer", nullable: false),
                    RequireClientSecret = table.Column<bool>(type: "boolean", nullable: false),
                    RequireConsent = table.Column<bool>(type: "boolean", nullable: false),
                    RequirePkce = table.Column<bool>(type: "boolean", nullable: false),
                    RequireRequestObject = table.Column<bool>(type: "boolean", nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(type: "integer", nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "boolean", nullable: false),
                    UserCodeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserSsoLifetime = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerDeviceFlowCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DeviceCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SubjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UserCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerDeviceFlowCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Emphasize = table.Column<bool>(type: "boolean", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerPersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ConsumedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SubjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerPersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceClaims",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceClaims", x => new { x.ApiResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceClaims_IdentityServerApiResources_~",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceProperties",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceProperties", x => new { x.ApiResourceId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceProperties_IdentityServerApiResour~",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceScopes",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceScopes", x => new { x.ApiResourceId, x.Scope });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceScopes_IdentityServerApiResources_~",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceSecrets",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceSecrets", x => new { x.ApiResourceId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceSecrets_IdentityServerApiResources~",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopeClaims",
                columns: table => new
                {
                    ApiScopeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopeClaims", x => new { x.ApiScopeId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiSco~",
                        column: x => x.ApiScopeId,
                        principalTable: "IdentityServerApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopeProperties",
                columns: table => new
                {
                    ApiScopeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopeProperties", x => new { x.ApiScopeId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiScopeProperties_IdentityServerApiScopes_Ap~",
                        column: x => x.ApiScopeId,
                        principalTable: "IdentityServerApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientClaims",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientClaims", x => new { x.ClientId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientClaims_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientCorsOrigins",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Origin = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientCorsOrigins", x => new { x.ClientId, x.Origin });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientCorsOrigins_IdentityServerClients_Clien~",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientGrantTypes",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrantType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientGrantTypes", x => new { x.ClientId, x.GrantType });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientGrantTypes_IdentityServerClients_Client~",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientIdPRestrictions",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientIdPRestrictions", x => new { x.ClientId, x.Provider });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientIdPRestrictions_IdentityServerClients_C~",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientPostLogoutRedirectUris",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostLogoutRedirectUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientPostLogoutRedirectUris", x => new { x.ClientId, x.PostLogoutRedirectUri });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientPostLogoutRedirectUris_IdentityServerCl~",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientProperties",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientProperties", x => new { x.ClientId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientProperties_IdentityServerClients_Client~",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientRedirectUris",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    RedirectUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientRedirectUris", x => new { x.ClientId, x.RedirectUri });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientRedirectUris_IdentityServerClients_Clie~",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientScopes",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientScopes", x => new { x.ClientId, x.Scope });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientScopes_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientSecrets",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientSecrets", x => new { x.ClientId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientSecrets_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResourceClaims",
                columns: table => new
                {
                    IdentityResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResourceClaims", x => new { x.IdentityResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerIdentityResourceClaims_IdentityServerIdentity~",
                        column: x => x.IdentityResourceId,
                        principalTable: "IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResourceProperties",
                columns: table => new
                {
                    IdentityResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResourceProperties", x => new { x.IdentityResourceId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerIdentityResourceProperties_IdentityServerIden~",
                        column: x => x.IdentityResourceId,
                        principalTable: "IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerClients_ClientId",
                table: "IdentityServerClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerDeviceFlowCodes_DeviceCode",
                table: "IdentityServerDeviceFlowCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerDeviceFlowCodes_Expiration",
                table: "IdentityServerDeviceFlowCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerDeviceFlowCodes_UserCode",
                table: "IdentityServerDeviceFlowCodes",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerPersistedGrants_Expiration",
                table: "IdentityServerPersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerPersistedGrants_SubjectId_ClientId_Type",
                table: "IdentityServerPersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerPersistedGrants_SubjectId_SessionId_Type",
                table: "IdentityServerPersistedGrants",
                columns: new[] { "SubjectId", "SessionId", "Type" });
        }
    }
}
