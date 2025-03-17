using Microsoft.Extensions.DependencyInjection;
using PackageName.CompanyName.ProjectName.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
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
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName;

[DependsOn(
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
    typeof(SingleMigrationsEntityFrameworkCoreModule)
    )]
public class SingleMigrationsEntityFrameworkCoreDatabaseManagementNameModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SingleMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
#if MySQL
            options.UseMySQL();
            options.UseMySQL<ProjectNameDbContext>();
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
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//解决PostgreSql设置为utc时间后无法写入local时区的问题
            options.UseNpgsql();
            options.UseNpgsql<ProjectNameDbContext>();
#endif
        });
    }
}
