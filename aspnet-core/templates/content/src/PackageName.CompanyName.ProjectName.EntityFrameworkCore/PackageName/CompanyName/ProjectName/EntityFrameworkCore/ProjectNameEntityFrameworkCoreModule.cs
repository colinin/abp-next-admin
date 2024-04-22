﻿using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
#if MySQL
using Volo.Abp.EntityFrameworkCore.MySQL;
#elif SqlServer 
using Volo.Abp.EntityFrameworkCore.SqlServer;
#elif Sqlite 
using Volo.Abp.EntityFrameworkCore.Sqlite;
#elif Oracle 
using Volo.Abp.EntityFrameworkCore.Oracle;
#elif OracleDevart 
using Volo.Abp.EntityFrameworkCore.Oracle.Devart;
#elif PostgreSql 
using Volo.Abp.EntityFrameworkCore.PostgreSql;
#endif

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[DependsOn(
    typeof(ProjectNameDomainModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpEntityFrameworkCoreModule),
#if MySQL
    typeof(AbpEntityFrameworkCoreMySQLModule),
#elif SqlServer
    typeof(AbpEntityFrameworkCoreSqlServerModule),
#elif Sqlite
    typeof(AbpEntityFrameworkCoreSqliteModule),
#elif Oracle
    typeof(AbpEntityFrameworkCoreOracleModule),
#elif OracleDevart
    typeof(AbpEntityFrameworkCoreOracleDevartModule),
#elif PostgreSql
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
#endif
    typeof(AbpSaasEntityFrameworkCoreModule))]
public class ProjectNameEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
#if MySQL
            options.UseMySQL();
            options.UseMySQL<ProjectNameDbContext>();
#elif SqlServer
            options.UseSqlServer();
            options.UseSqlServer<ProjectNameDbContext>();
#elif Sqlite
            options.UseSqlite();
            options.UseSqlite<ProjectNameDbContext>();
#elif Oracle || OracleDevart
            options.UseOracle();
            options.UseOracle<ProjectNameDbContext>();
#elif PostgreSql
            options.UseNpgsql();
            options.UseNpgsql<ProjectNameDbContext>();
#endif
        });

        context.Services.AddAbpDbContext<ProjectNameDbContext>(options =>
        {
            options.AddDefaultRepositories<IProjectNameDbContext>();
        });
    }
}
