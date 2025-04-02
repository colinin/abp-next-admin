using LINGYUN.Abp.Saas.MySql;
using LINGYUN.Abp.Saas.Oracle;
using LINGYUN.Abp.Saas.PostgreSql;
using LINGYUN.Abp.Saas.Sqlite;
using LINGYUN.Abp.Saas.SqlServer;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Saas;

[DependsOn(typeof(AbpSaasDomainModule))]
public class AbpSaasDbCheckerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpSaasConnectionStringCheckOptions>(options =>
        {
            options.ConnectionStringCheckers["mysql"] = new MySqlConnectionStringChecker();
            options.ConnectionStringCheckers["oracle"] = new OracleConnectionStringChecker();
            options.ConnectionStringCheckers["postgres"] = new NpgsqlConnectionStringChecker();
            options.ConnectionStringCheckers["sqlite"] = new SqliteConnectionStringChecker();
            options.ConnectionStringCheckers["sqlserver"] = new SqlServerConnectionStringChecker();
        });
    }
}
