using LINGYUN.Abp.BlobStoring.Tencent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Tencent;

[DependsOn(
    typeof(AbpBlobStoringTencentCloudModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementTencentModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("OssManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            "Tencent".Equals(ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseTencentCloud(oss =>
                    {
                        ossConfiguration.GetSection("Tencent").Bind(oss);
                    });
                });
            });
            context.Services.AddTencentContainer();
        }
    }
}
