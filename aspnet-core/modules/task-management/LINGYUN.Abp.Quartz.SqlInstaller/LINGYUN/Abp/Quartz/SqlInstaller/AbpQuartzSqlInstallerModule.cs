using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Util;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace LINGYUN.Abp.Quartz.SqlInstaller;

[DependsOn(
    typeof(AbpDataModule),
    typeof(AbpQuartzModule))]
public class AbpQuartzSqlInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var abpQuartzOptions = context.Services.ExecutePreConfiguredActions<AbpQuartzOptions>();
        Configure<QuartzOptions>(options =>
        {
            foreach (var settingKey in abpQuartzOptions.Properties.AllKeys)
            {
                options[settingKey] = abpQuartzOptions.Properties[settingKey];
            }

            if (abpQuartzOptions.Properties[StdSchedulerFactory.PropertyJobStoreType] == null)
            {
                var defaultJobStoreType = typeof(RAMJobStore).AssemblyQualifiedNameWithoutVersion();

                options[StdSchedulerFactory.PropertyJobStoreType] = defaultJobStoreType;
                abpQuartzOptions.Properties[StdSchedulerFactory.PropertyJobStoreType] = defaultJobStoreType;
            }
        });
    }

    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        if (configuration.GetValue("Quartz:UsePersistentStore", false))
        {
            var driverDelegateType = configuration[$"Quartz:Properties:quartz.jobStore.driverDelegateType"];
            // 初始化 Quartz 数据库
            var installs = context.ServiceProvider.GetServices<IQuartzSqlInstaller>();

            foreach (var install in installs)
            {
                if (install.CanInstall(driverDelegateType))
                {
                    await install.InstallAsync();
                }
            }
        }
    }
}
