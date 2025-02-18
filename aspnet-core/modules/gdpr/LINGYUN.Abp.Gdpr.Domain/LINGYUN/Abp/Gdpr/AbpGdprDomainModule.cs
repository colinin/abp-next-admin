using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Gdpr;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpDddDomainModule),
    typeof(AbpGdprDomainSharedModule)
    )]
public class AbpGdprDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpGdprOptions>(configuration.GetSection("Gdpr"));
        Configure<AbpCookieConsentOptions>(configuration.GetSection("CookieConsent"));

        context.Services.AddAutoMapperObjectMapper<AbpGdprDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpGdprDomainModule>(validate: true);
        });
    }
}
