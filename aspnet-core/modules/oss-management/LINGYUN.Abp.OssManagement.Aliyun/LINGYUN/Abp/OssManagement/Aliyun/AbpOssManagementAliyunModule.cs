using LINGYUN.Abp.BlobStoring.Aliyun;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Aliyun;

[DependsOn(
    typeof(AbpBlobStoringAliyunModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementAliyunModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("OssManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            "Aliyun".Equals(ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseAliyun(oss =>
                    {
                        ossConfiguration.GetSection("Aliyun").Bind(oss);
                    });
                });
            });
            context.Services.AddAliyunContainer();
        }
    }
}
