using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.Settings
{
    public class WeChatWorkSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    WeChatWorkSettingNames.EnabledQuickLogin,
                    // 默认启用
                    true.ToString(),
                    L("DisplayName:WeChatWork.EnabledQuickLogin"),
                    L("Description:WeChatWork.EnabledQuickLogin"),
                    isVisibleToClients: true,
                    isEncrypted: false)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
            );
            context.Add(GetConnectionSettings());
        }

        protected virtual SettingDefinition[] GetConnectionSettings()
        {
            return new[]
            {
                new SettingDefinition(
                    WeChatWorkSettingNames.Connection.CorpId,
                    displayName: L("DisplayName:WeChatWork.Connection.CorpId"),
                    description: L("Description:WeChatWork.Connection.CorpId"),
                    isEncrypted: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
            };
        }

        protected ILocalizableString L(string name)
        {
            return LocalizableString.Create<WeChatWorkResource>(name);
        }
    }
}
