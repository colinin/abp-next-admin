using Elsa;
using Elsa.Options;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;

[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLPomeloModule))]
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

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaEntityFrameworkCoreMySqlModule>();
        });
    }

    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        if (configuration.GetValue<bool>("Elsa:Features:DefaultPersistence:EntityFrameworkCore:MySql:Enabled"))
        {
            await context.ServiceProvider
                .GetService<MySqlElsaDataBaseInstaller>()
                ?.InstallAsync();
        }
    }
}
