using LINGYUN.Abp.FileManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.FileManagement.Settings
{
    public class AbpFileManagementSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(CreateFileSystemSettings());
        }

        protected SettingDefinition[] CreateFileSystemSettings()
        {
            return new SettingDefinition[]
            {
                new SettingDefinition(
                    name: AbpFileManagementSettingNames.FileLimitLength, 
                    defaultValue: AbpFileManagementSettingNames.DefaultFileLimitLength.ToString(),
                    displayName: L("DisplayName:FileLimitLength"), 
                    description: L("Description:FileLimitLength"), 
                    isVisibleToClients: true)
                .WithProviders(
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
                new SettingDefinition(
                    name: AbpFileManagementSettingNames.AllowFileExtensions, 
                    defaultValue: AbpFileManagementSettingNames.DefaultAllowFileExtensions,
                    displayName: L("DisplayName:AllowFileExtensions"), 
                    description: L("Description:AllowFileExtensions"), 
                    isVisibleToClients: true)
                .WithProviders(
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName),
            };
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpFileManagementResource>(name);
        }
    }
}
