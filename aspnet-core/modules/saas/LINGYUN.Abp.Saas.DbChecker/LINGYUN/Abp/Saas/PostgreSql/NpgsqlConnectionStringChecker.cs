using LINGYUN.Abp.Saas.Tenants;
using Npgsql;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Saas.PostgreSql;

public class NpgsqlConnectionStringChecker : IDataBaseConnectionStringChecker, ITransientDependency
{
    public virtual async Task<DataBaseConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new DataBaseConnectionStringCheckResult();
        var connString = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Timeout = 1
        };

        var oldDatabaseName = connString.Database;
        connString.Database = "postgres";

        try
        {
            await using var conn = new NpgsqlConnection(connString.ConnectionString);
            await conn.OpenAsync();
            result.Connected = true;
            await conn.ChangeDatabaseAsync(oldDatabaseName!);
            result.DatabaseExists = true;

            await conn.CloseAsync();

            return result;
        }
        catch (Exception e)
        {
            result.Error = e;
            return result;
        }
    }
}
