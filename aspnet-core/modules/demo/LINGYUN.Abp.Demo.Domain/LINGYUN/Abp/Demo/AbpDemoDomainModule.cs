using LINGYUN.Abp.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Demo;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpDddDomainModule),
    typeof(AbpDataProtectionModule),
    typeof(AbpDemoDomainSharedModule))]
public class AbpDemoDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpDemoDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<DemoDomainMapperProfile>(validate: true);
        });

        //Configure<AbpLocalizationPersistenceOptions>(options =>
        //{
        //    options.AddPersistenceResource<DemoResource>();
        //});

        // 分布式事件
        //Configure<AbpDistributedEntityEventOptions>(options =>
        //{
        //    options.AutoEventSelectors.Add<Text>();
        //    options.EtoMappings.Add<Text, TextEto>();
        //});
    }
}
