using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement.Localization;

namespace LINGYUN.Abp.SettingManagement
{
    public class AbpSettingManagementPermissionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var settingPermissionGroup = context.AddGroup(AbpSettingManagementPermissions.GroupName, L("Permission:SettingManagement"));
            
            var settingPermissions = settingPermissionGroup.AddPermission(AbpSettingManagementPermissions.Settings.Default, L("Permission:Settings"));
            settingPermissions.AddChild(AbpSettingManagementPermissions.Settings.Manager, L("Permission:Manager"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpSettingManagementResource>(name);
        }
    }
}
