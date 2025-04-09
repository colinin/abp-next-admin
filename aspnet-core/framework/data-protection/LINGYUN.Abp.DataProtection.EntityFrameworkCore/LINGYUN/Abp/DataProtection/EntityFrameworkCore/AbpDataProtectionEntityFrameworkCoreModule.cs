using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;

[DependsOn(
    typeof(AbpDataProtectionModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class AbpDataProtectionEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient<IDataAccessStrategyFilterBuilder, EfCoreDataAccessStrategyFilterBuilder>();
    }
}
