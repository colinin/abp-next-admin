using LINGYUN.Abp.FeatureManagement.Client;
using LINGYUN.Abp.FeatureManagement.Client.Permissions;
using LINGYUN.Abp.Features.Client;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;

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
            Configure<FeatureManagementOptions>(options =>
            {
                options.Providers.Add<ClientFeatureManagementProvider>();

                options.ProviderPolicies[ClientFeatureValueProvider.ProviderName] = ClientFeaturePermissionNames.Clients.ManageFeatures;
            });
        }
    }
}
