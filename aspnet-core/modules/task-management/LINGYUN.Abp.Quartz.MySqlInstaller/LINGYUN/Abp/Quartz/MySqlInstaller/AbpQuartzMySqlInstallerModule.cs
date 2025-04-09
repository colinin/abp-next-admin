using LINGYUN.Abp.Quartz.SqlInstaller;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Quartz.MySqlInstaller;

[DependsOn(
    typeof(AbpQuartzSqlInstallerModule),
    typeof(AbpVirtualFileSystemModule))]
public class AbpQuartzMySqlInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpQuartzMySqlInstallerModule>();
        });
    }
}