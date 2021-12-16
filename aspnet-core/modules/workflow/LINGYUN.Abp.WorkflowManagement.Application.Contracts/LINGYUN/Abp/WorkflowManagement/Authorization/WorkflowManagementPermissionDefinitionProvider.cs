using LINGYUN.Abp.WorkflowManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.WorkflowManagement.Authorization
{
    public class WorkflowManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(WorkflowManagementPermissions.GroupName, L("Permission:WorkflowManagement"));

            var engine = group.AddPermission(
                WorkflowManagementPermissions.Engine.Default,
                L("Permission:Engine"));
            engine.AddChild(
                WorkflowManagementPermissions.Engine.Initialize,
                L("Permission:Initialize"));
            engine.AddChild(
                WorkflowManagementPermissions.Engine.Register,
                L("Permission:Register"));

            var workflow = group.AddPermission(
                WorkflowManagementPermissions.WorkflowDef.Default,
                L("Permission:WorkflowDef"));
            workflow.AddChild(
                WorkflowManagementPermissions.WorkflowDef.Create,
                L("Permission:Create"));
            workflow.AddChild(
                WorkflowManagementPermissions.WorkflowDef.Delete,
                L("Permission:Delete"));

            group.AddPermission(
                WorkflowManagementPermissions.ManageSettings,
                L("Permission:ManageSettings"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<WorkflowManagementResource>(name);
        }
    }
}
