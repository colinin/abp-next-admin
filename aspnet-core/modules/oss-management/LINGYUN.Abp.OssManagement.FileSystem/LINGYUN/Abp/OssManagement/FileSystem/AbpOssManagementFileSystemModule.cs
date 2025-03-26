using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.FileSystem;

[DependsOn(
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementFileSystemModule : AbpModule
{
}
