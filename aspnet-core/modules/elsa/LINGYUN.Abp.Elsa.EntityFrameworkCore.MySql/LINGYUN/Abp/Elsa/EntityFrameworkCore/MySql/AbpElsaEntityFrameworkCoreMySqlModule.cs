using Elsa;
using Elsa.Options;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;

[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule))]
public class AbpElsaEntityFrameworkCoreMySqlModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var startups = new[]
        {
            typeof(PersistenceStartup),
            typeof(WebhooksStartup),
            typeof(WorkflowSettingsStartup),
        };

        PreConfigure<ElsaOptionsBuilder>(elsa =>
        {
            elsa.AddFeatures(startups, configuration);
        });
    }
}
