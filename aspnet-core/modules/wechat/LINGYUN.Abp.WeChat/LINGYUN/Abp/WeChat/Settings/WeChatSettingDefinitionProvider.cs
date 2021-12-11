using LINGYUN.Abp.WeChat.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Settings
{
    public class WeChatSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    WeChatSettingNames.EnabledQuickLogin, 
                    // 默认启用
                    true.ToString(),
                    L("DisplayName:WeChat.EnabledQuickLogin"),
                    L("Description:WeChat.EnabledQuickLogin"),
                    isVisibleToClients: true,
                    isEncrypted: false)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
            );
        }

        protected ILocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatResource>(name);
        }
    }
}
