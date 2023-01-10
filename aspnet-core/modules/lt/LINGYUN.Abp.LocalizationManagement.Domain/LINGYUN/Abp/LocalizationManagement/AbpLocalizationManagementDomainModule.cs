using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.LocalizationManagement
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpDddDomainModule),
        typeof(AbpLocalizationManagementDomainSharedModule))]
    public class AbpLocalizationManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpLocalizationManagementDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<LocalizationManagementDomainMapperProfile>(validate: true);
            });

            // 分布式事件
            //Configure<AbpDistributedEntityEventOptions>(options =>
            //{
            //    options.AutoEventSelectors.Add<Text>();
            //    options.EtoMappings.Add<Text, TextEto>();
            //});
        }
    }
}
