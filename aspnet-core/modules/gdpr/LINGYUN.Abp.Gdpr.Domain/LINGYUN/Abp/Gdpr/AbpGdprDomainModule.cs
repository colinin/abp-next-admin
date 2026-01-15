using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Gdpr;

[DependsOn(
    typeof(AbpMapperlyModule),
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

        context.Services.AddMapperlyObjectMapper<AbpGdprDomainModule>();
    }
}
