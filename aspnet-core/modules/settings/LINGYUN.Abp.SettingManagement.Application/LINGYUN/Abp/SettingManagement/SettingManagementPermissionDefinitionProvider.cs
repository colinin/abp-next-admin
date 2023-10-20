using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement.Localization;
using VoloSettingManagementPermissions = Volo.Abp.SettingManagement.SettingManagementPermissions;

namespace LINGYUN.Abp.SettingManagement;
public class SettingManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var settingGroup = context.GetGroup(VoloSettingManagementPermissions.GroupName);

        var definitionPermission = settingGroup.AddPermission(
            SettingManagementPermissions.Definition.Default,
            displayName: L("Permission:Definition"),
            multiTenancySide: MultiTenancySides.Host);
        definitionPermission.AddChild(
            SettingManagementPermissions.Definition.Create,
            displayName: L("Permission:Create"),
            multiTenancySide: MultiTenancySides.Host);
        definitionPermission.AddChild(
            SettingManagementPermissions.Definition.Update,
            displayName: L("Permission:Update"),
            multiTenancySide: MultiTenancySides.Host);
        definitionPermission.AddChild(
            SettingManagementPermissions.Definition.DeleteOrRestore,
            displayName: L("Permission:DeleteOrRestore"),
            multiTenancySide: MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSettingManagementResource>(name);
    }
}
