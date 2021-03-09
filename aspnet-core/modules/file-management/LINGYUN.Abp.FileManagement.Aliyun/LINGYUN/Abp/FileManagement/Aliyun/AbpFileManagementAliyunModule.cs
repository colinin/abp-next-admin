using LINGYUN.Abp.BlobStoring.Aliyun;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.FileManagement.Aliyun
{
    [DependsOn(
        typeof(AbpBlobStoringAliyunModule),
        typeof(AbpFileManagementDomainModule))]
    public class AbpFileManagementAliyunModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IOssContainerFactory, AliyunOssContainerFactory>();
        }
    }
}
