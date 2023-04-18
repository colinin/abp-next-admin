using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore;

[DependsOn(
    typeof(RulesEngineManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class RulesEngineManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<RulesEngineManagementDbContext>(options =>
        {
            options.AddRepository<RuleRecord, EfCoreRuleRecordRepository>();
            options.AddRepository<WorkflowRecord, EfCoreWorkflowRecordRepository>();

            options.AddDefaultRepositories<IRulesEngineManagementDbContext>();
        });
    }
}
