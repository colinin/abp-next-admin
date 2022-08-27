using LINGYUN.Abp.PushPlus.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.PushPlus.SettingManagement
{
    public class PushPlusSettingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var pushPlusGroup = context.AddGroup(
                PushPlusSettingPermissionNames.GroupName,
                L("Permission:PushPlus"));

            pushPlusGroup.AddPermission(
                PushPlusSettingPermissionNames.ManageSetting, L("Permission:ManageSetting"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<PushPlusResource>(name);
        }
    }
}
