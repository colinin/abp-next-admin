using LINGYUN.Abp.BlobStoring.Aliyun;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Aliyun;

[DependsOn(
    typeof(AbpBlobStoringAliyunModule),
    typeof(AbpOssManagementDomainModule))]
public class AbpOssManagementAliyunModule : AbpModule
{
}
