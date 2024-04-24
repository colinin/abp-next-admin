using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Aliyun
{
    [DependsOn(
        typeof(AbpAliyunModule),
        typeof(AbpTestsBaseModule))]
    public class AbpAliyunTestModule : AbpModule
    {
    }
}
