using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Util;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;
using static Quartz.SchedulerBuilder;

namespace LINGYUN.Abp.Quartz.SqlInstaller;

[DependsOn(
    typeof(AbpDataModule),
    typeof(AbpQuartzModule))]
public class AbpQuartzSqlInstallerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration;
        PreConfigure<AbpQuartzOptions>(options =>
        {
            // 如果使用持久化存储, 则配置quartz持久层
            if (configuration.GetValue("Quartz:UsePersistentStore", false))
            {
                var settings = configuration.GetSection("Quartz:Properties").Get<Dictionary<string, string>>();
                if (settings != null)
                {
                    foreach (var setting in settings)
                    {
                        options.Properties[setting.Key] = setting.Value;
                    }
                }

                options.Configurator += (config) =>
                {
                    config.UsePersistentStore(store =>
                    {
                        store.UseProperties = false;
                    });
                };
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var abpQuartzOptions = context.Services.ExecutePreConfiguredActions<AbpQuartzOptions>();
        Configure<QuartzOptions>(options =>
        {
            foreach (var settingKey in abpQuartzOptions.Properties.AllKeys)
            {
                if (!string.IsNullOrWhiteSpace(settingKey))
                {
                    options[settingKey] = abpQuartzOptions.Properties[settingKey];
                }
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
            if (!string.IsNullOrWhiteSpace(driverDelegateType))
            {
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
}
