using LINGYUN.Abp.WeChat.Localization;
using LINGYUN.Abp.WeChat.MiniProgram.Features;
using LINGYUN.Abp.WeChat.Official.Features;
using LINGYUN.Abp.WeChat.Work.Features;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
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
                WeChatSettingPermissionNames.Official, L("Permission:WeChat.Official"))
                .RequireFeatures(WeChatOfficialFeatures.Enable);

            wechatGroup.AddPermission(
                WeChatSettingPermissionNames.MiniProgram, L("Permission:WeChat.MiniProgram"))
                .RequireFeatures(WeChatMiniProgramFeatures.Enable);

            wechatGroup.AddPermission(
                WeChatSettingPermissionNames.Work, L("Permission:WeChat.Work"))
                .RequireFeatures(WeChatWorkFeatureNames.Enable);
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatResource>(name);
        }
    }
}
