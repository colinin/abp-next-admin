using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
#if MySQL
using Volo.Abp.EntityFrameworkCore.MySQL;
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
using Volo.Abp.EntityFrameworkCore.PostgreSql;
#endif

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[DependsOn(
    typeof(AbpDataDbMigratorModule),
    typeof(ProjectNameEntityFrameworkCoreModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
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
    typeof(AbpDataDbMigratorModule)
    )]
public class ProjectNameDbMigratorEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 配置连接字符串
        Configure<AbpDbConnectionOptions>(options =>
        {
            // 业务数据库
            options.Databases.Configure("ProjectName", database =>
            {
                database.MapConnection(
                    "ProjectName"
                );
            });
            // abp框架数据库
            options.Databases.Configure("Framework", database =>
            {
                database.MapConnection(
                    "AbpSaas",
                    "AbpTextTemplating",
                    "AbpSettingManagement",
                    "AbpFeatureManagement",
                    "AbpPermissionManagement",
                    "AbpLocalizationManagement"
                );
            });
        });

        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
#if MySQL
            options.UseMySQL(mysql =>
            {
                // see: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1960
                mysql.TranslateParameterizedCollectionsToConstants();
            });
            options.UseMySQL<ProjectNameDbContext>(mysql =>
            {
                // see: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1960
                mysql.TranslateParameterizedCollectionsToConstants();
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
