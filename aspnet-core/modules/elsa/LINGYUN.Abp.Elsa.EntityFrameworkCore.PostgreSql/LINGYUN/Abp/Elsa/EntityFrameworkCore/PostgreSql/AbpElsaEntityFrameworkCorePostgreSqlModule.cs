using Elsa;
using Elsa.Options;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql;

[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule))]
public class AbpElsaEntityFrameworkCorePostgreSqlModule : AbpModule
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

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaEntityFrameworkCorePostgreSqlModule>();
        });
    }

    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        if (configuration.GetValue<bool>("Elsa:Features:DefaultPersistence:EntityFrameworkCore:PostgreSql:Enabled"))
        {
            await context.ServiceProvider
                .GetService<PostgreSqlElsaDataBaseInstaller>()
                ?.InstallAsync();
        }
    }
}
