using LINGYUN.Abp.WxPusher.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.WxPusher.SettingManagement
{
    public class WxPusherSettingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var wxPusherGroup = context.AddGroup(
                WxPusherSettingPermissionNames.GroupName,
                L("Permission:WxPusher"));

            wxPusherGroup.AddPermission(
                WxPusherSettingPermissionNames.ManageSetting, L("Permission:ManageSetting"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<WxPusherResource>(name);
        }
    }
}
