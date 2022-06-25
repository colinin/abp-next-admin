using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingDomainModule),
    typeof(AbpTextTemplatingApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDddApplicationModule))]
public class AbpTextTemplatingApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpTextTemplatingApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpTextTemplatingApplicationAutoMapperProfile>(validate: true);
        });
    }
}
