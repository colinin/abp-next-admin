using LINGYUN.Platform.Localization;
using LINGYUN.Platform.Versions;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Platform.Settings
{
    public class PlatformSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(CreateAppVersionSettings());
        }

        protected SettingDefinition[] CreateAppVersionSettings()
        {
            return new SettingDefinition[]
            {
                new SettingDefinition(
                    name: PlatformSettingNames.AppVersion.VersionFileLimitLength, 
                    defaultValue: AppVersionConsts.DefaultVersionFileLimitLength.ToString(),
                    displayName: L("DisplayName:VersionFileLimitLength"), 
                    description: L("Description:VersionFileLimitLength"), 
                    isVisibleToClients: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: PlatformSettingNames.AppVersion.AllowVersionFileExtensions, 
                    defaultValue: AppVersionConsts.DefaultAllowVersionFileExtensions,
                    displayName: L("DisplayName:AllowVersionFileExtensions"), 
                    description: L("Description:AllowVersionFileExtensions"), 
                    isVisibleToClients: true)
                .WithProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
            };
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<PlatformResource>(name);
        }
    }
}
