using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Tests
{
    [DependsOn(
        typeof(AbpTestsBaseModule)
        )]
    public class AbpElsaTestsModule : AbpModule
    {
    }
}
