using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring;
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
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("OssManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            "Minio".Equals(ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            context.Services.AddMinioHttpClient();
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseMinio(oss =>
                    {
                        ossConfiguration.GetSection("Minio").Bind(oss);
                    });
                });
            });
            context.Services.AddMinioContainer();
        }
    }
}
