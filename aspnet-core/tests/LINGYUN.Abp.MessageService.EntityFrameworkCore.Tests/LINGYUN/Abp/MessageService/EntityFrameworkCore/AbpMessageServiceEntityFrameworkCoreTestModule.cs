using LINGYUN.Abp.EntityFrameworkCore.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MessageService.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpMessageServiceEntityFrameworkCoreModule),
        typeof(AbpMessageServiceDomainTestModule),
        typeof(AbpEntityFrameworkCoreTestModule)
        )]
    public class AbpMessageServiceEntityFrameworkCoreTestModule : AbpModule
    {

    }
}
