using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Localization.Persistence;

[DependsOn(
    typeof(AbpLocalizationModule))]
public class AbpLocalizationPersistenceModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHostedService<StaticLocalizationSaverHostService>();

        Configure<AbpLocalizationOptions>(options =>
        {
            options.GlobalContributors.Add<LocalizationPersistenceContributor>();
        });
    }
}
