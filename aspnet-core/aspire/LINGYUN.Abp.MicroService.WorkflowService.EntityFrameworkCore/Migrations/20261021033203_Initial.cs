using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LINGYUN.Abp.MicroService.WorkflowService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EX_ApprovalFlowCarbonCopies",
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
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalFlowCarbonCopies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalFlowDelegations",
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
                    table.PrimaryKey("PK_EX_ApprovalFlowDelegations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalFlowInstances",
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
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Outcome = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
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
                    table.PrimaryKey("PK_EX_ApprovalFlowInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalFlowNodeTasks",
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
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalFlowNodeTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalFlowRecords",
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
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EX_ApprovalFlowRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_ApprovalFlowTemplates",
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
                    table.PrimaryKey("PK_EX_ApprovalFlowTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_FormDefinitionData",
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
                    table.PrimaryKey("PK_EX_FormDefinitionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_FormDefinitionDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_EX_FormDefinitionDefinitions", x => x.Id);
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
                    table.PrimaryKey("PK_EX_OrganizationUnitManagerAssignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EX_FormDefinitionDefinitionFields",
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
                    table.PrimaryKey("PK_EX_FormDefinitionDefinitionFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EX_FormDefinitionDefinitionFields_EX_FormDefinitionDefiniti~",
                        column: x => x.FormDefinitionId,
                        principalTable: "EX_FormDefinitionDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowCarbonCopies_InstanceId",
                table: "EX_ApprovalFlowCarbonCopies",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowCarbonCopies_TenantId_RecipientId_IsRead",
                table: "EX_ApprovalFlowCarbonCopies",
                columns: new[] { "TenantId", "RecipientId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowDelegations_TenantId_DelegateId_IsEnabled",
                table: "EX_ApprovalFlowDelegations",
                columns: new[] { "TenantId", "DelegateId", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowDelegations_TenantId_DelegatorId_IsEnabled",
                table: "EX_ApprovalFlowDelegations",
                columns: new[] { "TenantId", "DelegatorId", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowInstances_ElsaWorkflowInstanceId",
                table: "EX_ApprovalFlowInstances",
                column: "ElsaWorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowInstances_TenantId_InitiatorId",
                table: "EX_ApprovalFlowInstances",
                columns: new[] { "TenantId", "InitiatorId" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowInstances_TenantId_Status",
                table: "EX_ApprovalFlowInstances",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowInstances_TenantId_TemplateId",
                table: "EX_ApprovalFlowInstances",
                columns: new[] { "TenantId", "TemplateId" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowNodeTasks_InstanceId",
                table: "EX_ApprovalFlowNodeTasks",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowNodeTasks_NodeInstanceId_Status",
                table: "EX_ApprovalFlowNodeTasks",
                columns: new[] { "NodeInstanceId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowNodeTasks_TaskId",
                table: "EX_ApprovalFlowNodeTasks",
                column: "TaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowRecords_InstanceId_OccurredAt",
                table: "EX_ApprovalFlowRecords",
                columns: new[] { "InstanceId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowTemplates_TenantId_IsPublished_IsEnabled",
                table: "EX_ApprovalFlowTemplates",
                columns: new[] { "TenantId", "IsPublished", "IsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_ApprovalFlowTemplates_TenantId_Name",
                table: "EX_ApprovalFlowTemplates",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDefinitionData_FormDefinitionId_InstanceId",
                table: "EX_FormDefinitionData",
                columns: new[] { "FormDefinitionId", "InstanceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDefinitionData_TenantId_FormCode",
                table: "EX_FormDefinitionData",
                columns: new[] { "TenantId", "FormCode" });

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDefinitionDefinitionFields_FormDefinitionId_Key",
                table: "EX_FormDefinitionDefinitionFields",
                columns: new[] { "FormDefinitionId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDefinitionDefinitions_TenantId_Code_Version",
                table: "EX_FormDefinitionDefinitions",
                columns: new[] { "TenantId", "Code", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EX_FormDefinitionDefinitions_TenantId_IsPublished",
                table: "EX_FormDefinitionDefinitions",
                columns: new[] { "TenantId", "IsPublished" });

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
                name: "EX_ApprovalFlowCarbonCopies");

            migrationBuilder.DropTable(
                name: "EX_ApprovalFlowDelegations");

            migrationBuilder.DropTable(
                name: "EX_ApprovalFlowInstances");

            migrationBuilder.DropTable(
                name: "EX_ApprovalFlowNodeTasks");

            migrationBuilder.DropTable(
                name: "EX_ApprovalFlowRecords");

            migrationBuilder.DropTable(
                name: "EX_ApprovalFlowTemplates");

            migrationBuilder.DropTable(
                name: "EX_FormDefinitionData");

            migrationBuilder.DropTable(
                name: "EX_FormDefinitionDefinitionFields");

            migrationBuilder.DropTable(
                name: "EX_OrganizationUnitManagerAssignments");

            migrationBuilder.DropTable(
                name: "EX_FormDefinitionDefinitions");
        }
    }
}
