using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.FileSystem
{
    [DependsOn(
        typeof(AbpBlobStoringFileSystemModule),
        typeof(AbpOssManagementDomainModule))]
    public class AbpOssManagementFileSystemModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IOssContainerFactory, FileSystemOssContainerFactory>();
        }
    }
}
