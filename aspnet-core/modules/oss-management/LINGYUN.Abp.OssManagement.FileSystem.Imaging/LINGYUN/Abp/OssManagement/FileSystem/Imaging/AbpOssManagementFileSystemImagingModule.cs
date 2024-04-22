using Volo.Abp.Imaging;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.FileSystem.Imaging;

[DependsOn(
    typeof(AbpImagingAbstractionsModule),
    typeof(AbpOssManagementFileSystemModule))]
public class AbpOssManagementFileSystemImagingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FileSystemOssOptions>(options =>
        {
            options.AddProcesser(new AbpImagingProcesserContributor());
        });
    }
}
