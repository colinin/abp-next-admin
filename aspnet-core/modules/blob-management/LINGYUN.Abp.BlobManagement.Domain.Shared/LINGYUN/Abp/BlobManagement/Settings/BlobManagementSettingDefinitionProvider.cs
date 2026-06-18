using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.BlobManagement.Settings;

public class BlobManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "BlobManagement";
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
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName, 
                L("Settings:BlobManagement"),
                requiredFeatures: [BlobManagementFeatureNames.Blob.Enable])
            .WithParent("Blobs", L("Settings:BlobManagement.Blobs"))
            .WithValueType(ValueType.Number),
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
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("Settings:BlobManagement"),
                requiredFeatures: [BlobManagementFeatureNames.Blob.Enable])
            .WithParent("Blobs", L("Settings:BlobManagement.Blobs")),
            new SettingDefinition(
                name: BlobManagementSettingNames.GenerateDownloadUrlExpirySeconds,
                defaultValue: BlobManagementSettingNames.DefaultGenerateDownloadUrlExpirySeconds.ToString(),
                displayName: L("DisplayName:GenerateDownloadUrlExpirySeconds"),
                description: L("Description:GenerateDownloadUrlExpirySeconds"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("Settings:BlobManagement"),
                requiredFeatures: [BlobManagementFeatureNames.Blob.Enable])
            .WithParent("Blobs", L("Settings:BlobManagement.Blobs"))
            .WithValueType(ValueType.Number),
        };
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<BlobManagementResource>(name);
    }
}
