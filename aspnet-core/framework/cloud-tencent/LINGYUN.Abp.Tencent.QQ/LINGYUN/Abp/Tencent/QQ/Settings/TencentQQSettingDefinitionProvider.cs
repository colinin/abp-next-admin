using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent.QQ.Settings
{
    public class TencentQQSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(GetQQConnectSettings());
        }

        private SettingDefinition[] GetQQConnectSettings()
        {
            return new SettingDefinition[]
            {
                new SettingDefinition(
                    TencentQQSettingNames.QQConnect.AppId, "",
                    L("DisplayName:QQConnect.AppId"),
                    L("Description:QQConnect.AppId"),
                    isVisibleToClients: false,
                    isEncrypted: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    TencentQQSettingNames.QQConnect.AppKey, "",
                    L("DisplayName:QQConnect.AppKey"),
                    L("Description:QQConnect.AppKey"),
                    isVisibleToClients: false,
                    isEncrypted: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    TencentQQSettingNames.QQConnect.IsMobile,
                    "false",
                    L("DisplayName:QQConnect.IsMobile"),
                    L("Description:QQConnect.IsMobile"),
                    isVisibleToClients: false,
                    isEncrypted: false)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
            };
        }

        protected ILocalizableString L(string name)
        {
            return LocalizableString.Create<TencentCloudResource>(name);
        }
    }
}
