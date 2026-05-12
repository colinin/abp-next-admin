using LINGYUN.Abp.Features.LimitValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpBlobManagementDomainSharedModule),
    typeof(AbpDddDomainModule),
    typeof(AbpMultiTenancyModule),
    typeof(AbpFeaturesLimitValidationModule)
    )]
public class AbpBlobManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpBlobManagementDomainModule>();

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<BlobContainer>();
            options.AutoEventSelectors.Add<Blob>();

            options.EtoMappings.Add<BlobContainer, BlobContainerEto>(typeof(AbpBlobManagementDomainModule));
            options.EtoMappings.Add<Blob, BlobEto>(typeof(AbpBlobManagementDomainModule));
        });

        if (context.Services.IsDataMigrationEnvironment())
        {
            context.Services.Replace(
                ServiceDescriptor.Singleton<IBlobDataSeeder, NoneBlobDataSeeder>());
        }
    }
}
