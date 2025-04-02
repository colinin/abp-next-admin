using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Quartz.SqlServerInstaller;

[DependsOn(
    typeof(AbpQuartzModule),
    typeof(AbpVirtualFileSystemModule))]
public class AbpQuartzSqlServerInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpQuartzSqlServerInstallerModule>();
        });
    }

    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        if (configuration.GetValue("Quartz:UsePersistentStore", false))
        {
            // 初始化 Quartz SqlServer 数据库
            await context.ServiceProvider
                .GetRequiredService<QuartzSqlServerInstaller>()
                .InstallAsync();
        }
    }
}