using LINGYUN.Abp.BlobStoring.Nexus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Nexus;

[DependsOn(
    typeof(AbpBlobStoringNexusModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementNexusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("OssManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            "Nexus".Equals(ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseNexus(oss =>
                    {
                        ossConfiguration.GetSection("Nexus").Bind(oss);
                    });
                });
            });

            context.Services.AddNexusContainer();
        }
    }
}
