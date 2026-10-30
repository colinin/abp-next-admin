using LINGYUN.Abp.Features.LimitValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Encryption;

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

        var stringEncryptionOptions = context.Services.ExecutePreConfiguredActions<AbpStringEncryptionOptions>();

        var configuration = context.Services.GetConfiguration();

        Configure<AbpBlobManagementOptions>(options =>
        {
            // 绑定 BlobManagement 配置节(下载密钥、分桶窗口等)
            configuration.GetSection("BlobManagement").Bind(options);
            if (options.DownloadKeySecret.IsNullOrWhiteSpace())
            {
                // 下载密钥的签名密钥未显式配置时, 使用abp加密密钥
                options.DownloadKeySecret = stringEncryptionOptions.DefaultPassPhrase;
            }
        });

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
