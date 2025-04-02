using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Quartz.MySqlInstaller;

[DependsOn(
    typeof(AbpQuartzModule),
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

    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        // 初始化 Quartz Mysql 数据库
        await context.ServiceProvider
            .GetRequiredService<QuartzMySqlInstaller>()
            .InstallAsync();
    }
}