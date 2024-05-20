using LINGYUN.Abp.DataProtection.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.DataProtectionManagement.Permissions;

public class DataProtectionManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(
            DataProtectionManagementPermissionNames.GroupName, 
            L("Permission:DataProtectionManagement"));

        var entityTypeInfo = group.AddPermission(DataProtectionManagementPermissionNames.EntityTypeInfo.Default, L("Permission:EntityTypeInfo"));
        entityTypeInfo.AddChild(DataProtectionManagementPermissionNames.EntityTypeInfo.Create, L("Permission:Create"));
        entityTypeInfo.AddChild(DataProtectionManagementPermissionNames.EntityTypeInfo.Update, L("Permission:Update"));
        entityTypeInfo.AddChild(DataProtectionManagementPermissionNames.EntityTypeInfo.Delete, L("Permission:Delete"));

        var roleEntityRule = group.AddPermission(DataProtectionManagementPermissionNames.RoleEntityRule.Default, L("Permission:RoleEntityRule"));
        roleEntityRule.AddChild(DataProtectionManagementPermissionNames.RoleEntityRule.Create, L("Permission:Create"));
        roleEntityRule.AddChild(DataProtectionManagementPermissionNames.RoleEntityRule.Update, L("Permission:Update"));
        roleEntityRule.AddChild(DataProtectionManagementPermissionNames.RoleEntityRule.Delete, L("Permission:Delete"));

        var ouEntityRule = group.AddPermission(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Default, L("Permission:OrganizationUnitEntityRule"));
        ouEntityRule.AddChild(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Create, L("Permission:Create"));
        ouEntityRule.AddChild(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Update, L("Permission:Update"));
        ouEntityRule.AddChild(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DataProtectionResource>(name);
    }
}
