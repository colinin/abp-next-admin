using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpOperationRateLimitingDomainModule))]
public class AbpOperationRateLimitingEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<OperationRateLimitingDbContext>(options =>
        {
            options.AddRepository<OperationRateLimitingPolicyRecord, EfCoreOperationRateLimitingPolicyRecordRepository>();
        });

        context.Services.AddTransient<
            IOperationRateLimitingPolicyRecordRepository,
            EfCoreOperationRateLimitingPolicyRecordRepository>();

        context.Services.AddTransient<
            IDynamicOperationRateLimitingPolicyStore,
            DynamicOperationRateLimitingPolicyStore>();
    }
}
