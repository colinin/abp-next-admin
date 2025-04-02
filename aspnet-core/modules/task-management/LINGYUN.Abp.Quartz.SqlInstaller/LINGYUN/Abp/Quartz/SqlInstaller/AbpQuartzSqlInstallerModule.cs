using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace LINGYUN.Abp.Quartz.SqlInstaller;

[DependsOn(typeof(AbpQuartzModule))]
public class AbpQuartzSqlInstallerModule : AbpModule
{
    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        if (configuration.GetValue("Quartz:UsePersistentStore", false))
        {
            // 初始化 Quartz 数据库
            await context.ServiceProvider
                .GetService<IQuartzSqlInstaller>()
                ?.InstallAsync();
        }
    }
}
