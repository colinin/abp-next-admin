using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddElsaNextApprovalFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Args",
                table: "TK_BackgroundJobs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Paramters",
                table: "TK_BackgroundJobActions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AppPlatformPackageBlobs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AppNotifications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AppNotificationDefinitions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AppNotificationDefinitionGroups",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpWebhooksWebhooks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpWebhooksWebhookGroups",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpWebhooksSubscriptions",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResourceName",
                table: "AbpLocalizationTexts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubjectId",
                table: "AbpAuthSubjectStrategys",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityTypeFullName",
                table: "AbpAuthRoleEntityRules",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityTypeFullName",
                table: "AbpAuthOrganizationUnitEntityRules",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EX_ApprovalCcs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    InstanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    InstanceReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    NodeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    NodeInstanceId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RecipientProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RecipientType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    RecipientId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RecipientDisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    SenderProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SenderType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    SenderId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SenderDisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DataJson = table.Column<string>(type: "text", nullable: true),
                    NotifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalCcs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalDelegations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    DelegatorProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DelegatorType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    DelegatorId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DelegatorDisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DelegateProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DelegateType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    DelegateId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DelegateDisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Remark = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalDelegations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TemplateCategory = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    BusinessKey = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    InitiatorProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    InitiatorType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    InitiatorId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    InitiatorDisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ElsaWorkflowInstanceId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CurrentNodeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CurrentNodeName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FormDataJson = table.Column<string>(type: "text", nullable: true),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Outcome = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalNodeTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    InstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeInstanceId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    NodeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    NodeName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TaskId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ApproverProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ApproverType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    ApproverId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ApproverDisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    SignMode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Origin = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FirstViewedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalNodeTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    InstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    NodeId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    NodeName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NodeInstanceId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    TaskId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ActorProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ActorType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    ActorId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ActorDisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ActionKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ActionLabel = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Effect = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DataJson = table.Column<string>(type: "text", nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Category = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ElsaDefinitionId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ElsaDefinitionKey = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ElsaDefinitionVersion = table.Column<int>(type: "integer", nullable: true),
                    FormDefinitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    BusinessKeyHint = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FlowDefinitionJson = table.Column<string>(type: "text", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_FormDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    FormDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FormCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FormVersion = table.Column<int>(type: "integer", nullable: false),
                    InstanceId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DataJson = table.Column<string>(type: "text", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_FormDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_FormDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_FormDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_OrganizationUnitManagerAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationUnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Remark = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_OrganizationUnitManagerAssignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_FormFieldDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FormDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Label = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Permission = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Hint = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    OptionsJson = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_FormFieldDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EX_FormFieldDefinitions_EX_FormDefinitions_FormDefinitionId",
                        column: x => x.FormDefinitionId,
                        principalTable: "EX_FormDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalCcs_InstanceId",
                table: "EX_ApprovalCcs",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalCcs_TenantId_RecipientId_IsRead",
                table: "EX_ApprovalCcs",
                columns: new[] { "TenantId", "RecipientId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalDelegations_TenantId_DelegateId_IsEnabled",
                table: "EX_ApprovalDelegations",
                columns: new[] { "TenantId", "DelegateId", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalDelegations_TenantId_DelegatorId_IsEnabled",
                table: "EX_ApprovalDelegations",
                columns: new[] { "TenantId", "DelegatorId", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalInstances_ElsaWorkflowInstanceId",
                table: "EX_ApprovalInstances",
                column: "ElsaWorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalInstances_TenantId_InitiatorId",
                table: "EX_ApprovalInstances",
                columns: new[] { "TenantId", "InitiatorId" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalInstances_TenantId_Status",
                table: "EX_ApprovalInstances",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalInstances_TenantId_TemplateId",
                table: "EX_ApprovalInstances",
                columns: new[] { "TenantId", "TemplateId" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalNodeTasks_InstanceId",
                table: "EX_ApprovalNodeTasks",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalNodeTasks_NodeInstanceId_Status",
                table: "EX_ApprovalNodeTasks",
                columns: new[] { "NodeInstanceId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalNodeTasks_TaskId",
                table: "EX_ApprovalNodeTasks",
                column: "TaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalRecords_InstanceId_OccurredAt",
                table: "EX_ApprovalRecords",
                columns: new[] { "InstanceId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalTemplates_TenantId_IsPublished_IsEnabled",
                table: "EX_ApprovalTemplates",
                columns: new[] { "TenantId", "IsPublished", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalTemplates_TenantId_Name",
                table: "EX_ApprovalTemplates",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDatas_FormDefinitionId_InstanceId",
                table: "EX_FormDatas",
                columns: new[] { "FormDefinitionId", "InstanceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDatas_TenantId_FormCode",
                table: "EX_FormDatas",
                columns: new[] { "TenantId", "FormCode" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDefinitions_TenantId_Code_Version",
                table: "EX_FormDefinitions",
                columns: new[] { "TenantId", "Code", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDefinitions_TenantId_IsPublished",
                table: "EX_FormDefinitions",
                columns: new[] { "TenantId", "IsPublished" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormFieldDefinitions_FormDefinitionId_Key",
                table: "EX_FormFieldDefinitions",
                columns: new[] { "FormDefinitionId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_OrganizationUnitManagerAssignments_TenantId_Organizatio~1",
                table: "EX_OrganizationUnitManagerAssignments",
                columns: new[] { "TenantId", "OrganizationUnitId", "IsPrimary", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_OrganizationUnitManagerAssignments_TenantId_Organization~",
                table: "EX_OrganizationUnitManagerAssignments",
                columns: new[] { "TenantId", "OrganizationUnitId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_OrganizationUnitManagerAssignments_TenantId_UserId",
                table: "EX_OrganizationUnitManagerAssignments",
                columns: new[] { "TenantId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EX_ApprovalCcs");

            migrationBuilder.DropTable(
                name: "EX_ApprovalDelegations");

            migrationBuilder.DropTable(
                name: "EX_ApprovalInstances");

            migrationBuilder.DropTable(
                name: "EX_ApprovalNodeTasks");

            migrationBuilder.DropTable(
                name: "EX_ApprovalRecords");

            migrationBuilder.DropTable(
                name: "EX_ApprovalTemplates");

            migrationBuilder.DropTable(
                name: "EX_FormDatas");

            migrationBuilder.DropTable(
                name: "EX_FormFieldDefinitions");

            migrationBuilder.DropTable(
                name: "EX_OrganizationUnitManagerAssignments");

            migrationBuilder.DropTable(
                name: "EX_FormDefinitions");

            migrationBuilder.AlterColumn<string>(
                name: "Args",
                table: "TK_BackgroundJobs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Paramters",
                table: "TK_BackgroundJobActions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AppPlatformPackageBlobs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AppNotifications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AppNotificationDefinitions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AppNotificationDefinitionGroups",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpWebhooksWebhooks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpWebhooksWebhookGroups",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpWebhooksSubscriptions",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "ResourceName",
                table: "AbpLocalizationTexts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectId",
                table: "AbpAuthSubjectStrategys",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "EntityTypeFullName",
                table: "AbpAuthRoleEntityRules",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EntityTypeFullName",
                table: "AbpAuthOrganizationUnitEntityRules",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
