using LINGYUN.Abp.BlobManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.BlobManagement.Settings;

public class BlobManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(CreateBlobFileSettings());
    }

    protected SettingDefinition[] CreateBlobFileSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                name: BlobManagementSettingNames.FileLimitLength,
                defaultValue: BlobManagementSettingNames.DefaultFileLimitLength.ToString(),
                displayName: L("DisplayName:FileLimitLength"),
                description: L("Description:FileLimitLength"),
                isVisibleToClients: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName),
            new SettingDefinition(
                name: BlobManagementSettingNames.AllowFileExtensions,
                defaultValue: BlobManagementSettingNames.DefaultAllowFileExtensions,
                displayName: L("DisplayName:AllowFileExtensions"),
                description: L("Description:AllowFileExtensions"),
                isVisibleToClients: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName),
        };
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<BlobManagementResource>(name);
    }
}
