using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement;

[DependsOn(
    typeof(AbpOssManagementDomainModule),
    typeof(AbpTestsBaseModule))]
public class AbpOssManagementDomainTestsModule : AbpModule
{
}
