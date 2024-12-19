using Elsa;
using Elsa.Options;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer;

[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule))]
public class AbpElsaEntityFrameworkCoreSqlServerModule : AbpModule
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
