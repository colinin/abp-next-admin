using LINGYUN.Abp.BlobStoring.Tencent;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Tencent;

[DependsOn(
    typeof(AbpBlobStoringTencentCloudModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementTencentModule : AbpModule
{
}
