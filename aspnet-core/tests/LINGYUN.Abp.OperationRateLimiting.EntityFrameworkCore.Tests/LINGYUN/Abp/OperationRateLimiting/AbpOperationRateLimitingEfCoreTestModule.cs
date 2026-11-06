using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(LINGYUN.Abp.Tests.AbpTestsBaseModule),
    typeof(AbpOperationRateLimitingEntityFrameworkCoreModule))]
public class AbpOperationRateLimitingEfCoreTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<OperationRateLimitingDbContext>(dbContextOptions =>
            {
                dbContextOptions.DbContextOptions.UseInMemoryDatabase("OperationRateLimitingTests");
            });
        });
    }
}
