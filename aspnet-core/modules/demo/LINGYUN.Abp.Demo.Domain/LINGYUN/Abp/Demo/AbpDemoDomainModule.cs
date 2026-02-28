using LINGYUN.Abp.DataProtection;
using LINGYUN.Abp.DataProtection.Localization;
using LINGYUN.Abp.Demo.Books;
using LINGYUN.Abp.Demo.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Demo;

[DependsOn(
    typeof(AbpMapperlyModule),
    typeof(AbpDddDomainModule),
    typeof(AbpDataProtectionModule),
    typeof(AbpDemoDomainSharedModule))]
public class AbpDemoDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpDemoDomainModule>();

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Get<DemoResource>()
                .AddBaseTypes(typeof(DataProtectionResource));
        });

        // 分布式事件
        //Configure<AbpDistributedEntityEventOptions>(options =>
        //{
        //    options.AutoEventSelectors.Add<Text>();
        //    options.EtoMappings.Add<Text, TextEto>();
        //});

        Configure<AbpDataProtectionOptions>(options =>
        {
            // 外键属性不可设定规则
            options.EntityIgnoreProperties.Add(typeof(Book), new []{ nameof(Book.AuthorId) } );
        });
    }
}
