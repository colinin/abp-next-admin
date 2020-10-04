using LINGYUN.Abp.Rules;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RulesEngine
{
    [DependsOn(
        typeof(AbpRulesModule),
        typeof(AbpAutoMapperModule))]
    public class AbpRulesEngineModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpRulesEngineModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MsRulesEngineMapperProfile>(validate: true);
            });
        }
    }
}
