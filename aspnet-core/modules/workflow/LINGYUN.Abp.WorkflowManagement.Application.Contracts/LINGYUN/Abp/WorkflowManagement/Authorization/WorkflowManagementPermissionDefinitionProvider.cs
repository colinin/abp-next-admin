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
