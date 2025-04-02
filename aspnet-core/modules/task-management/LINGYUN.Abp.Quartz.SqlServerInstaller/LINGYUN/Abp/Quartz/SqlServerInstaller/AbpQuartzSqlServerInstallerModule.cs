using LINGYUN.Abp.Quartz.SqlInstaller;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Quartz.SqlServerInstaller;

[DependsOn(
    typeof(AbpQuartzSqlInstallerModule),
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
}