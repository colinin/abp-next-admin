using LINGYUN.Abp.OperationRateLimiting.Management.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.OperationRateLimiting;

public class AbpOperationRateLimitingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(
            AbpOperationRateLimitingPermissions.GroupName,
            L("Permission:OperationRateLimiting"));

        var policies = group.AddPermission(
            AbpOperationRateLimitingPermissions.Policies.Default,
            L("Permission:Policies"));

        policies.AddChild(
            AbpOperationRateLimitingPermissions.Policies.Create,
            L("Permission:Create"));
        policies.AddChild(
            AbpOperationRateLimitingPermissions.Policies.Update,
            L("Permission:Update"));
        policies.AddChild(
            AbpOperationRateLimitingPermissions.Policies.Delete,
            L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OperationRateLimitingManagementResource>(name);
    }
}
