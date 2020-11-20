using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.WeChat.SettingManagement
{
    public class WeChatSettingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var wechatGroup = context.AddGroup(
                WeChatSettingPermissionNames.GroupName,
                L("Permission:WeChat"));

            wechatGroup.AddPermission(
                WeChatSettingPermissionNames.Official, L("Permission:WeChat.Official"));

            wechatGroup.AddPermission(
                WeChatSettingPermissionNames.MiniProgram, L("Permission:WeChat.MiniProgram"));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatResource>(name);
        }
    }
}
