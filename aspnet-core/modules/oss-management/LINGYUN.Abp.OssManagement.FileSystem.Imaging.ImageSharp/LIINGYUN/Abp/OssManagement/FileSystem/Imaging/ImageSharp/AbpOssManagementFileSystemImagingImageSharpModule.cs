using LINGYUN.Abp.OssManagement.FileSystem.Imaging;
using Volo.Abp.Imaging;
using Volo.Abp.Modularity;

namespace LIINGYUN.Abp.OssManagement.FileSystem.Imaging.ImageSharp;

[DependsOn(
    typeof(AbpImagingImageSharpModule),
    typeof(AbpOssManagementFileSystemImagingModule))]
public class AbpOssManagementFileSystemImagingImageSharpModule : AbpModule
{
}
