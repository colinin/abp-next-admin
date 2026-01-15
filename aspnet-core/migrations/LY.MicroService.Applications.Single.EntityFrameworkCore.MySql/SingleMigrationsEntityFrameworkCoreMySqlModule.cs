using DotNetCore.CAP;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;
using LINGYUN.Abp.Quartz.MySqlInstaller;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.MySql;

[DependsOn(
    typeof(AbpEntityFrameworkCoreMySQLPomeloModule),
    // Quartz MySql数据库初始化模块
    typeof(AbpQuartzMySqlInstallerModule),
    // Elsa工作流模块 MySql集成
    typeof(AbpElsaEntityFrameworkCoreMySqlModule),
    typeof(SingleMigrationsEntityFrameworkCoreModule)
    )]
public class SingleMigrationsEntityFrameworkCoreMySqlModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var dbProvider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        if ("MySql".Equals(dbProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            // TODO: MySQL EF 10提供商不可用,待完成后移除
            // See: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/2007
            throw new AbpInitializationException("MySQL EF 10 provider is unavailable. Please switch to another database infrastructure!")
            {
                HelpLink = "https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/2007",
            };

            //var configuration = context.Services.GetConfiguration();

            //PreConfigure<CapOptions>(options =>
            //{
            //    if (configuration.GetValue<bool>("CAP:IsEnabled"))
            //    {
            //        options.UseMySql(
            //            sqlOptions =>
            //            {
            //                configuration.GetSection("CAP:MySql").Bind(sqlOptions);
            //            });
            //    }
            //});
        } 
    }
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var dbProvider = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_PROVIDER");
        if ("MySql".Equals(dbProvider, StringComparison.InvariantCultureIgnoreCase))
        {
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL(
                    mysql =>
                    {
                        // see: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1960
#pragma warning disable CS0618 
                        mysql.TranslateParameterizedCollectionsToConstants();
#pragma warning restore CS0618
                        mysql.MigrationsAssembly(GetType().Assembly);
                    });
            });

            Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                if (options.DefaultSequentialGuidType == null)
                {
                    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
                }
            });
        }
    }
}
