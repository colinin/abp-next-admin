using LINGYUN.Abp.Rules;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RulesEngine
{
    [DependsOn(
        typeof(AbpRulesEngineModule),
        typeof(AbpAutoMapperModule))]
    public class AbpMsRulesEngineModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpMsRulesEngineModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MsRulesEngineMapperProfile>(validate: true);
            });
        }
    }
}
