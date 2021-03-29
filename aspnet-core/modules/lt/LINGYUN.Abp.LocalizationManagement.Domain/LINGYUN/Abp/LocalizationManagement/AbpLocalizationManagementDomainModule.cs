using LINGYUN.Abp.Localization.Dynamic;
using LINGYUN.Abp.LocalizationManagement.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.LocalizationManagement
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpDddDomainModule),
        typeof(AbpLocalizationDynamicModule),
        typeof(AbpLocalizationManagementDomainSharedModule))]
    public class AbpLocalizationManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpLocalizationManagementDomainModule>();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<LocalizationManagementResource>()
                    .AddDynamic();
            });

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
