using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.EntityFramework.Core;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class AbpElsaEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }
}
