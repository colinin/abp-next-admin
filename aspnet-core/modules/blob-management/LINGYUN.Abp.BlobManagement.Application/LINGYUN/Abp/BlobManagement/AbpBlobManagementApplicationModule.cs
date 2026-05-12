using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Caching;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpBlobManagementDomainModule),
    typeof(AbpBlobManagementApplicationContractsModule),
    typeof(AbpCachingModule),
    typeof(AbpMapperlyModule),
    typeof(AbpDddApplicationModule))]
public class AbpBlobManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpBlobManagementApplicationModule>();

        Configure<AbpBlobManagementOptions>(options =>
        {
            options.BlobPolicyCheckProviders.Add(PrivateBlobPolicyCheckProvider.ProviderName, new PrivateBlobPolicyCheckProvider());
        });
    }
}
