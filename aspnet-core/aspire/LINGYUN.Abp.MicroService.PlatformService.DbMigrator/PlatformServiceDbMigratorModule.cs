using LINGYUN.Abp.BlobManagement.Aliyun;
using LINGYUN.Abp.BlobManagement.FileSystem;
using LINGYUN.Abp.BlobManagement.Minio;
using LINGYUN.Abp.BlobManagement.Tencent;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.PlatformService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpBlobManagementAliyunModule),
    typeof(AbpBlobManagementFileSystemModule),
    typeof(AbpBlobManagementMinioModule),
    typeof(AbpBlobManagementTencentModule),
    typeof(PlatformServiceMigrationsEntityFrameworkCoreModule)
    )]
public class PlatformServiceDbMigratorModule : AbpModule
{
}
