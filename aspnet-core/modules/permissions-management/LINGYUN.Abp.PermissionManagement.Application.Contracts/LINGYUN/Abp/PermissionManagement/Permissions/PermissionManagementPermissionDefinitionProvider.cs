using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Localization;

namespace LINGYUN.Abp.PermissionManagement.Permissions;

public class PermissionManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var permissionGroup = context.AddGroup(
            PermissionManagementPermissionNames.GroupName,
            L("Permission:PermissionManagement"));

        var groupDefinition = permissionGroup.AddPermission(
            PermissionManagementPermissionNames.GroupDefinition.Default,
            L("Permission:GroupDefinitions"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            PermissionManagementPermissionNames.GroupDefinition.Create, 
            L("Permission:Create"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            PermissionManagementPermissionNames.GroupDefinition.Update,
            L("Permission:Edit"),
            MultiTenancySides.Host);
        groupDefinition.AddChild(
            PermissionManagementPermissionNames.GroupDefinition.Delete, 
            L("Permission:Delete"),
            MultiTenancySides.Host);

        var permissionDefinition = permissionGroup.AddPermission(
            PermissionManagementPermissionNames.Definition.Default, 
            L("Permission:PermissionDefinitions"),
            MultiTenancySides.Host);
        permissionDefinition.AddChild(
            PermissionManagementPermissionNames.Definition.Create, 
            L("Permission:Create"),
            MultiTenancySides.Host);
        permissionDefinition.AddChild(
            PermissionManagementPermissionNames.Definition.Update,
            L("Permission:Edit"),
            MultiTenancySides.Host);
        permissionDefinition.AddChild(
            PermissionManagementPermissionNames.Definition.Delete, 
            L("Permission:Delete"),
            MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpPermissionManagementResource>(name);
    }
}
