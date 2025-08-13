﻿using DotNetCore.CAP;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;
using LINGYUN.Abp.Quartz.MySqlInstaller;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
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
        var configuration = context.Services.GetConfiguration();

        PreConfigure<CapOptions>(options =>
        {
            if (configuration.GetValue<bool>("CAP:IsEnabled"))
            {
                options.UseMySql(
                    sqlOptions =>
                    {
                        configuration.GetSection("CAP:MySql").Bind(sqlOptions);
                    });
            }
        });
    }
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SingleMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL(
                mysql =>
                {
                    // see: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1960
                    mysql.TranslateParameterizedCollectionsToConstants();
                });
        });
    }
}
