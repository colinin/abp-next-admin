using LINGYUN.Abp.BlobStoring.Aliyun;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Aliyun
{
    [DependsOn(
        typeof(AbpBlobStoringAliyunModule),
        typeof(AbpOssManagementDomainModule))]
    public class AbpOssManagementAliyunModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IOssContainerFactory, AliyunOssContainerFactory>();
        }
    }
}
