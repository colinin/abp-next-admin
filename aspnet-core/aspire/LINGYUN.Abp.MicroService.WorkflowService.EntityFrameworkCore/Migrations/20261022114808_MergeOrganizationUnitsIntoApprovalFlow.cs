using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LINGYUN.Abp.MicroService.WorkflowService.Migrations
{
    /// <inheritdoc />
    /// <summary>
    /// 将独立的 OrganizationUnits 模块合并到审批流模块，并让本迁移项目的模型快照与审批流模块保持一致。
    /// </summary>
    /// <remarks>
    /// 本迁移项目此前未同步表单定义合并带来的表重命名（EX_FormDefinition* → EX_Form* / EX_Approval*），
    /// 因此这里包含这些重命名；OrganizationUnitManagerAssignment 实体合并后表名仍为
    /// EX_OrganizationUnitManagerAssignments，列与索引均未变化，不产生额外架构变更。
    /// </remarks>
    public partial class MergeOrganizationUnitsIntoApprovalFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EX_FormDefinitionDefinitionFields_EX_FormDefinitionDefiniti~",
                table: "EX_FormDefinitionDefinitionFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_FormDefinitionDefinitions",
                table: "EX_FormDefinitionDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_FormDefinitionDefinitionFields",
                table: "EX_FormDefinitionDefinitionFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_FormDefinitionData",
                table: "EX_FormDefinitionData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalFlowTemplates",
                table: "EX_ApprovalFlowTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalFlowRecords",
                table: "EX_ApprovalFlowRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalFlowNodeTasks",
                table: "EX_ApprovalFlowNodeTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalFlowInstances",
                table: "EX_ApprovalFlowInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalFlowDelegations",
                table: "EX_ApprovalFlowDelegations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalFlowCarbonCopies",
                table: "EX_ApprovalFlowCarbonCopies");

            migrationBuilder.RenameTable(
                name: "EX_FormDefinitionDefinitions",
                newName: "EX_FormDefinitions");

            migrationBuilder.RenameTable(
                name: "EX_FormDefinitionDefinitionFields",
                newName: "EX_FormFieldDefinitions");

            migrationBuilder.RenameTable(
                name: "EX_FormDefinitionData",
                newName: "EX_FormDatas");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalFlowTemplates",
                newName: "EX_ApprovalTemplates");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalFlowRecords",
                newName: "EX_ApprovalRecords");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalFlowNodeTasks",
                newName: "EX_ApprovalNodeTasks");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalFlowInstances",
                newName: "EX_ApprovalInstances");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalFlowDelegations",
                newName: "EX_ApprovalDelegations");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalFlowCarbonCopies",
                newName: "EX_ApprovalCcs");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDefinitionDefinitions_TenantId_IsPublished",
                table: "EX_FormDefinitions",
                newName: "IX_EX_FormDefinitions_TenantId_IsPublished");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDefinitionDefinitions_TenantId_Code_Version",
                table: "EX_FormDefinitions",
                newName: "IX_EX_FormDefinitions_TenantId_Code_Version");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDefinitionDefinitionFields_FormDefinitionId_Key",
                table: "EX_FormFieldDefinitions",
                newName: "IX_EX_FormFieldDefinitions_FormDefinitionId_Key");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDefinitionData_TenantId_FormCode",
                table: "EX_FormDatas",
                newName: "IX_EX_FormDatas_TenantId_FormCode");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDefinitionData_FormDefinitionId_InstanceId",
                table: "EX_FormDatas",
                newName: "IX_EX_FormDatas_FormDefinitionId_InstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowTemplates_TenantId_Name",
                table: "EX_ApprovalTemplates",
                newName: "IX_EX_ApprovalTemplates_TenantId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowTemplates_TenantId_IsPublished_IsEnabled",
                table: "EX_ApprovalTemplates",
                newName: "IX_EX_ApprovalTemplates_TenantId_IsPublished_IsEnabled");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowRecords_InstanceId_OccurredAt",
                table: "EX_ApprovalRecords",
                newName: "IX_EX_ApprovalRecords_InstanceId_OccurredAt");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowNodeTasks_TaskId",
                table: "EX_ApprovalNodeTasks",
                newName: "IX_EX_ApprovalNodeTasks_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowNodeTasks_NodeInstanceId_Status",
                table: "EX_ApprovalNodeTasks",
                newName: "IX_EX_ApprovalNodeTasks_NodeInstanceId_Status");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowNodeTasks_InstanceId",
                table: "EX_ApprovalNodeTasks",
                newName: "IX_EX_ApprovalNodeTasks_InstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowInstances_TenantId_TemplateId",
                table: "EX_ApprovalInstances",
                newName: "IX_EX_ApprovalInstances_TenantId_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowInstances_TenantId_Status",
                table: "EX_ApprovalInstances",
                newName: "IX_EX_ApprovalInstances_TenantId_Status");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowInstances_TenantId_InitiatorId",
                table: "EX_ApprovalInstances",
                newName: "IX_EX_ApprovalInstances_TenantId_InitiatorId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowInstances_ElsaWorkflowInstanceId",
                table: "EX_ApprovalInstances",
                newName: "IX_EX_ApprovalInstances_ElsaWorkflowInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowDelegations_TenantId_DelegatorId_IsEnabled",
                table: "EX_ApprovalDelegations",
                newName: "IX_EX_ApprovalDelegations_TenantId_DelegatorId_IsEnabled");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowDelegations_TenantId_DelegateId_IsEnabled",
                table: "EX_ApprovalDelegations",
                newName: "IX_EX_ApprovalDelegations_TenantId_DelegateId_IsEnabled");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowCarbonCopies_TenantId_RecipientId_IsRead",
                table: "EX_ApprovalCcs",
                newName: "IX_EX_ApprovalCcs_TenantId_RecipientId_IsRead");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalFlowCarbonCopies_InstanceId",
                table: "EX_ApprovalCcs",
                newName: "IX_EX_ApprovalCcs_InstanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_FormDefinitions",
                table: "EX_FormDefinitions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_FormFieldDefinitions",
                table: "EX_FormFieldDefinitions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_FormDatas",
                table: "EX_FormDatas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalTemplates",
                table: "EX_ApprovalTemplates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalRecords",
                table: "EX_ApprovalRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalNodeTasks",
                table: "EX_ApprovalNodeTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalInstances",
                table: "EX_ApprovalInstances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalDelegations",
                table: "EX_ApprovalDelegations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalCcs",
                table: "EX_ApprovalCcs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EX_FormFieldDefinitions_EX_FormDefinitions_FormDefinitionId",
                table: "EX_FormFieldDefinitions",
                column: "FormDefinitionId",
                principalTable: "EX_FormDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EX_FormFieldDefinitions_EX_FormDefinitions_FormDefinitionId",
                table: "EX_FormFieldDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_FormFieldDefinitions",
                table: "EX_FormFieldDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_FormDefinitions",
                table: "EX_FormDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_FormDatas",
                table: "EX_FormDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalTemplates",
                table: "EX_ApprovalTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalRecords",
                table: "EX_ApprovalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalNodeTasks",
                table: "EX_ApprovalNodeTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalInstances",
                table: "EX_ApprovalInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalDelegations",
                table: "EX_ApprovalDelegations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EX_ApprovalCcs",
                table: "EX_ApprovalCcs");

            migrationBuilder.RenameTable(
                name: "EX_FormFieldDefinitions",
                newName: "EX_FormDefinitionDefinitionFields");

            migrationBuilder.RenameTable(
                name: "EX_FormDefinitions",
                newName: "EX_FormDefinitionDefinitions");

            migrationBuilder.RenameTable(
                name: "EX_FormDatas",
                newName: "EX_FormDefinitionData");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalTemplates",
                newName: "EX_ApprovalFlowTemplates");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalRecords",
                newName: "EX_ApprovalFlowRecords");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalNodeTasks",
                newName: "EX_ApprovalFlowNodeTasks");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalInstances",
                newName: "EX_ApprovalFlowInstances");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalDelegations",
                newName: "EX_ApprovalFlowDelegations");

            migrationBuilder.RenameTable(
                name: "EX_ApprovalCcs",
                newName: "EX_ApprovalFlowCarbonCopies");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormFieldDefinitions_FormDefinitionId_Key",
                table: "EX_FormDefinitionDefinitionFields",
                newName: "IX_EX_FormDefinitionDefinitionFields_FormDefinitionId_Key");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDefinitions_TenantId_IsPublished",
                table: "EX_FormDefinitionDefinitions",
                newName: "IX_EX_FormDefinitionDefinitions_TenantId_IsPublished");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDefinitions_TenantId_Code_Version",
                table: "EX_FormDefinitionDefinitions",
                newName: "IX_EX_FormDefinitionDefinitions_TenantId_Code_Version");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDatas_TenantId_FormCode",
                table: "EX_FormDefinitionData",
                newName: "IX_EX_FormDefinitionData_TenantId_FormCode");

            migrationBuilder.RenameIndex(
                name: "IX_EX_FormDatas_FormDefinitionId_InstanceId",
                table: "EX_FormDefinitionData",
                newName: "IX_EX_FormDefinitionData_FormDefinitionId_InstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalTemplates_TenantId_Name",
                table: "EX_ApprovalFlowTemplates",
                newName: "IX_EX_ApprovalFlowTemplates_TenantId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalTemplates_TenantId_IsPublished_IsEnabled",
                table: "EX_ApprovalFlowTemplates",
                newName: "IX_EX_ApprovalFlowTemplates_TenantId_IsPublished_IsEnabled");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalRecords_InstanceId_OccurredAt",
                table: "EX_ApprovalFlowRecords",
                newName: "IX_EX_ApprovalFlowRecords_InstanceId_OccurredAt");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalNodeTasks_TaskId",
                table: "EX_ApprovalFlowNodeTasks",
                newName: "IX_EX_ApprovalFlowNodeTasks_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalNodeTasks_NodeInstanceId_Status",
                table: "EX_ApprovalFlowNodeTasks",
                newName: "IX_EX_ApprovalFlowNodeTasks_NodeInstanceId_Status");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalNodeTasks_InstanceId",
                table: "EX_ApprovalFlowNodeTasks",
                newName: "IX_EX_ApprovalFlowNodeTasks_InstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalInstances_TenantId_TemplateId",
                table: "EX_ApprovalFlowInstances",
                newName: "IX_EX_ApprovalFlowInstances_TenantId_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalInstances_TenantId_Status",
                table: "EX_ApprovalFlowInstances",
                newName: "IX_EX_ApprovalFlowInstances_TenantId_Status");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalInstances_TenantId_InitiatorId",
                table: "EX_ApprovalFlowInstances",
                newName: "IX_EX_ApprovalFlowInstances_TenantId_InitiatorId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalInstances_ElsaWorkflowInstanceId",
                table: "EX_ApprovalFlowInstances",
                newName: "IX_EX_ApprovalFlowInstances_ElsaWorkflowInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalDelegations_TenantId_DelegatorId_IsEnabled",
                table: "EX_ApprovalFlowDelegations",
                newName: "IX_EX_ApprovalFlowDelegations_TenantId_DelegatorId_IsEnabled");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalDelegations_TenantId_DelegateId_IsEnabled",
                table: "EX_ApprovalFlowDelegations",
                newName: "IX_EX_ApprovalFlowDelegations_TenantId_DelegateId_IsEnabled");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalCcs_TenantId_RecipientId_IsRead",
                table: "EX_ApprovalFlowCarbonCopies",
                newName: "IX_EX_ApprovalFlowCarbonCopies_TenantId_RecipientId_IsRead");

            migrationBuilder.RenameIndex(
                name: "IX_EX_ApprovalCcs_InstanceId",
                table: "EX_ApprovalFlowCarbonCopies",
                newName: "IX_EX_ApprovalFlowCarbonCopies_InstanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_FormDefinitionDefinitionFields",
                table: "EX_FormDefinitionDefinitionFields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_FormDefinitionDefinitions",
                table: "EX_FormDefinitionDefinitions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_FormDefinitionData",
                table: "EX_FormDefinitionData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalFlowTemplates",
                table: "EX_ApprovalFlowTemplates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalFlowRecords",
                table: "EX_ApprovalFlowRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalFlowNodeTasks",
                table: "EX_ApprovalFlowNodeTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalFlowInstances",
                table: "EX_ApprovalFlowInstances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalFlowDelegations",
                table: "EX_ApprovalFlowDelegations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EX_ApprovalFlowCarbonCopies",
                table: "EX_ApprovalFlowCarbonCopies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EX_FormDefinitionDefinitionFields_EX_FormDefinitionDefiniti~",
                table: "EX_FormDefinitionDefinitionFields",
                column: "FormDefinitionId",
                principalTable: "EX_FormDefinitionDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
