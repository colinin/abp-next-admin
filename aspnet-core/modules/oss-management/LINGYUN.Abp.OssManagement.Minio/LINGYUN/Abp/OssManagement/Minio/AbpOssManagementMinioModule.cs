using Microsoft.Extensions.DependencyInjection;
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
        context.Services.AddMinioHttpClient();
    }
}
