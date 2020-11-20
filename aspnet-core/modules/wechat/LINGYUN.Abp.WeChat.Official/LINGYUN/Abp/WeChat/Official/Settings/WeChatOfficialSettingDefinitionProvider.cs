using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Official.Settings
{
    public class WeChatOfficialSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    WeChatOfficialSettingNames.AppId, "",
                    L("DisplayName:WeChat.Official.AppId"),
                    L("Description:WeChat.Official.AppId"),
                    isVisibleToClients: true,
                    isEncrypted: true),
                new SettingDefinition(
                    WeChatOfficialSettingNames.AppSecret, "",
                    L("DisplayName:WeChat.Official.AppSecret"),
                    L("Description:WeChat.Official.AppSecret"),
                    isVisibleToClients: true,
                    isEncrypted: true),
                new SettingDefinition(
                    WeChatOfficialSettingNames.Url, "",
                    L("DisplayName:WeChat.Official.Url"),
                    L("Description:WeChat.Official.Url"),
                    isVisibleToClients: true,
                    isEncrypted: false),
                new SettingDefinition(
                    WeChatOfficialSettingNames.Token, "",
                    L("DisplayName:WeChat.Official.Token"),
                    L("Description:WeChat.Official.Token"),
                    isVisibleToClients: true,
                    isEncrypted: true),
                new SettingDefinition(
                    WeChatOfficialSettingNames.EncodingAESKey, "",
                    L("DisplayName:WeChat.Official.EncodingAESKey"),
                    L("Description:WeChat.Official.EncodingAESKey"),
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
