using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RulesManagement
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpRulesManagementDomainSharedModule))]
    public class AbpRulesManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpRulesManagementDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<RulesManagementMapperProfile>(validate: true);
            });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<EntityRule, EntityRuleEto>(typeof(AbpRulesManagementDomainModule));
                options.EtoMappings.Add<EntityRuleInGroup, EntityRuleGroupEto>(typeof(AbpRulesManagementDomainModule));
            });
        }
    }
}
