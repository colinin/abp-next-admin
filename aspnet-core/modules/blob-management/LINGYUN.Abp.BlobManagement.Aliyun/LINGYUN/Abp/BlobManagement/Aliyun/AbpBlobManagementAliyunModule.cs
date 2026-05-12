using LINGYUN.Abp.BlobStoring.Aliyun;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement.Aliyun;

[DependsOn(
    typeof(AbpBlobManagementDomainModule),
    typeof(AbpBlobStoringAliyunModule))]
public class AbpBlobManagementAliyunModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var ossConfiguration = configuration.GetSection("BlobManagement");
        var ossProvider = ossConfiguration["Provider"];

        if (!ossProvider.IsNullOrWhiteSpace() &&
            string.Equals(AliyunBlobProvider.ProviderName, ossProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.Configure<BlobManagementContainer>((containerConfiguration) =>
                {
                    containerConfiguration.UseAliyun(oss =>
                    {
                        ossConfiguration.GetSection(AliyunBlobProvider.ProviderName).Bind(oss);
                    });
                });
            });
            context.Services.AddAliyunHttpClient();
            context.Services.AddTransient<IBlobProvider, AliyunBlobProvider>();
        }
    }
}
