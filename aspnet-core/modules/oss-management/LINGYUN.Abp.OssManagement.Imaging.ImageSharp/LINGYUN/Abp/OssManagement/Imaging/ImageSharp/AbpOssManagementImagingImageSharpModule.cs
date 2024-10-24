using Volo.Abp.Imaging;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Imaging.ImageSharp;

[DependsOn(
    typeof(AbpImagingImageSharpModule),
    typeof(AbpOssManagementImagingModule))]
public class AbpOssManagementImagingImageSharpModule : AbpModule
{
}
