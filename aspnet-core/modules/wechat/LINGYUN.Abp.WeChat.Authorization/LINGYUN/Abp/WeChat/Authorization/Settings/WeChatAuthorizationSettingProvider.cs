using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Authorization.Settings
{
    public class WeChatAuthorizationSettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    WeChatAuthorizationSettingNames.AppId, "",
                    L("DisplayName:WeChat.Auth.AppId"),
                    L("Description:WeChat.Auth.AppId"),
                    isVisibleToClients: true,
                    isEncrypted: true),
                new SettingDefinition(
                    WeChatAuthorizationSettingNames.AppSecret, "",
                    L("DisplayName:WeChat.Auth.AppSecret"),
                    L("Description:WeChat.Auth.AppSecret"),
                    isVisibleToClients: true,
                    isEncrypted: true)
            );
        }

        protected ILocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatResource>(name);
        }
    }
}
