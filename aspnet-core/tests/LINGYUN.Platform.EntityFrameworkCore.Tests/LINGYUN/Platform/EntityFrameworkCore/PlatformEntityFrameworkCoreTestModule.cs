using LINGYUN.Abp.EntityFrameworkCore.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    [DependsOn(
        typeof(PlatformDomainTestModule),
        typeof(PlatformEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreTestModule)
        )]
    public class PlatformEntityFrameworkCoreTestModule : AbpModule
    {
    }
}
