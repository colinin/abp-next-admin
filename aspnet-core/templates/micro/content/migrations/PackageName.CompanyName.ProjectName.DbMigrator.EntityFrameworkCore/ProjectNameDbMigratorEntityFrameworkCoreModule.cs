using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
#if MySQL
// using Volo.Abp.EntityFrameworkCore.MySQL;
using LINGYUN.Abp.EntityFrameworkCore.MySQL;
using Microsoft.EntityFrameworkCore;
#elif SqlServer 
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure;
#elif Sqlite 
using Volo.Abp.EntityFrameworkCore.Sqlite;
#elif Oracle 
using Volo.Abp.EntityFrameworkCore.Oracle;
#elif OracleDevart 
using Volo.Abp.EntityFrameworkCore.Oracle.Devart;
#elif PostgreSql 
using System;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
#endif

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[DependsOn(
    typeof(AbpDataDbMigratorModule),
    typeof(ProjectNameEntityFrameworkCoreModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
#if MySQL
    // typeof(AbpEntityFrameworkCoreMySQLPomeloModule),
    typeof(AbpEntityFrameworkCoreMySQLMicrotingModule),
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
    typeof(AbpDataDbMigratorModule)
    )]
public class ProjectNameDbMigratorEntityFrameworkCoreModule : AbpModule
{
#if PostgreSql
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
#endif
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
#if MySQL
            options.UseMySQL(mysql =>
            {
                // see: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1960
                mysql.UseParameterizedCollectionMode(ParameterTranslationMode.Constant);
            });
            options.UseMySQL<ProjectNameDbContext>(mysql =>
            {
                // see: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1960
                mysql.UseParameterizedCollectionMode(ParameterTranslationMode.Constant);
            });
#elif SqlServer
            options.UseSqlServer();
            options.UseSqlServer<ProjectNameDbContext>(builder =>
            {
                // see https://learn.microsoft.com/en-us/sql/t-sql/statements/alter-database-transact-sql-compatibility-level?view=sql-server-ver16
                // builder.UseCompatibilityLevel(150);
            });
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
    }
}
