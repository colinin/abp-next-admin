using LINGYUN.Abp.Quartz.SqlInstaller;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Quartz.PostgresSqlInstaller;

[DependsOn(
    typeof(AbpQuartzSqlInstallerModule),
    typeof(AbpVirtualFileSystemModule))]
public class AbpQuartzPostgresSqlInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpQuartzPostgresSqlInstallerModule>();
        });

        context.Services.AddTransient<IQuartzSqlInstaller, PostgresSqlQuartzSqlInstaller>();
    }
}
