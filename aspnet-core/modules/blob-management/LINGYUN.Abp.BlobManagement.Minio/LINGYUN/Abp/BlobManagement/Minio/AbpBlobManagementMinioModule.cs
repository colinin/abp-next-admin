using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement.Minio;

[DependsOn(
    typeof(AbpBlobManagementDomainModule),
    typeof(AbpBlobStoringMinioModule))]
public class AbpBlobManagementMinioModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("BlobManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            string.Equals(MinioBlobProvider.ProviderName, ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.Configure<BlobManagementContainer>((containerConfiguration) =>
                {
                    containerConfiguration.UseMinio(oss =>
                    {
                        ossConfiguration.GetSection(MinioBlobProvider.ProviderName).Bind(oss);
                    });
                });
            });
            context.Services.AddMinioHttpClient();
            context.Services.AddTransient<IBlobProvider, MinioBlobProvider>();
        }
    }
}
