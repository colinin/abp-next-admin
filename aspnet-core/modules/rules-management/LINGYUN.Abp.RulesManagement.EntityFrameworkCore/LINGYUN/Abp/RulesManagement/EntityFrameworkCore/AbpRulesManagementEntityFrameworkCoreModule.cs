using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RulesManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpRulesManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class AbpRulesManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<RulesManagementDbContext>(options =>
            {
                options.AddRepository<EntityRule, EfCoreEntityRuleRepository>();
                options.AddRepository<EntityRuleInGroup, EfCoreEntityRuleGroupRepository>();
            });
        }
    }
}
