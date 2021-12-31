using LINGYUN.Abp.Tencent;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobStoring.Tencent;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpTencentCloudModule))]
public class AbpBlobStoringTencentCloudModule : AbpModule
{
}