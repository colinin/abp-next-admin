using Volo.Abp.Modularity;

namespace LINGYUN.Abp.FileManagement.FileSystem.ImageSharp
{
    [DependsOn(typeof(AbpFileManagementFileSystemModule))]
    public class AbpFileManagementFileSystemImageSharpModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<FileSystemOssOptions>(options =>
            {
                options.AddProcesser(new ImageSharpProcesserContributor());
            });
        }
    }
}
