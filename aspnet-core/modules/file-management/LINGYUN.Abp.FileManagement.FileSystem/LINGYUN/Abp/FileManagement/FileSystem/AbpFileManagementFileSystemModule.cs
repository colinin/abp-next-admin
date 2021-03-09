using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.FileManagement.FileSystem
{
    [DependsOn(
        typeof(AbpBlobStoringFileSystemModule),
        typeof(AbpFileManagementDomainModule))]
    public class AbpFileManagementFileSystemModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IOssContainerFactory, FileSystemOssContainerFactory>();
        }
    }
}
