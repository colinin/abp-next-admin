using LINGYUN.Abp.FeatureManagement.Client;
using LINGYUN.Abp.FeatureManagement.Client.Permissions;
using LINGYUN.Abp.Features.Client;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.FeatureManagement.Localization;

namespace LINGYUN.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpFeaturesClientModule),
        typeof(AbpFeatureManagementDomainModule)
        )]
    public class AbpFeatureManagementClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpFeatureManagementClientModule>();
            });

            Configure<FeatureManagementOptions>(options =>
            {
                options.Providers.Add<ClientFeatureManagementProvider>();

                options.ProviderPolicies[ClientFeatureValueProvider.ProviderName] = ClientFeaturePermissionNames.ManageClientFeatures;
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpFeatureManagementResource>()
                    .AddVirtualJson("/LINGYUN/Abp/FeatureManagement/Client/Localization/Resources");
            });
        }
    }
}
