using LINGYUN.Abp.BlobStoring.Tencent;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Tencent;

[DependsOn(
    typeof(AbpBlobStoringTencentCloudModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementTencentModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IOssContainerFactory, TencentOssContainerFactory>();

        context.Services.AddTransient<IOssObjectExpireor>(provider =>
            provider
                .GetRequiredService<IOssContainerFactory>()
                .Create()
                .As<TencentOssContainer>());
    }
}
