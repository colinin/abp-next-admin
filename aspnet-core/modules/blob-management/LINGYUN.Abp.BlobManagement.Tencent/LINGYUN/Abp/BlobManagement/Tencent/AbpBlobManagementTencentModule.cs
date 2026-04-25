using LINGYUN.Abp.BlobStoring.Tencent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement.Tencent;

[DependsOn(
    typeof(AbpBlobManagementDomainModule),
    typeof(AbpBlobStoringTencentCloudModule))]
public class AbpBlobManagementTencentModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("BlobManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            string.Equals(TencentBlobProvider.ProviderName, ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.Configure<BlobManagementContainer>((containerConfiguration) =>
                {
                    containerConfiguration.UseTencentCloud(oss =>
                    {
                        ossConfiguration.GetSection(TencentBlobProvider.ProviderName).Bind(oss);
                    });
                });
            });
            context.Services.AddTencentHttpClient();
            context.Services.AddTransient<IBlobProvider, TencentBlobProvider>();
        }
    }
}
