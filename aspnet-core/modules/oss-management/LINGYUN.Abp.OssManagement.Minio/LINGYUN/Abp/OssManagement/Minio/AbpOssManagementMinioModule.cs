using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Minio;

[DependsOn(
    typeof(AbpBlobStoringMinioModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementMinioModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IOssContainerFactory, MinioOssContainerFactory>();

        context.Services.AddTransient<IOssObjectExpireor>(provider =>
            provider
                .GetRequiredService<IOssContainerFactory>()
                .Create()
                .As<MinioOssContainer>());
    }
}
