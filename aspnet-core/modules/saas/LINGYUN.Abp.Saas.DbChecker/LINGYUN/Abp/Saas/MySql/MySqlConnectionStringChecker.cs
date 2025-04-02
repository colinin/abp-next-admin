using LINGYUN.Abp.Saas.Tenants;
using MySqlConnector;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Saas.MySql;

public class MySqlConnectionStringChecker : IDataBaseConnectionStringChecker
{
    public virtual async Task<DataBaseConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new DataBaseConnectionStringCheckResult();
        var connString = new MySqlConnectionStringBuilder(connectionString)
        {
            ConnectionLifeTime = 1
        };

        var oldDatabaseName = connString.Database;
        connString.Database = "mysql";

        try
        {
            await using var conn = new MySqlConnection(connString.ConnectionString);
            await conn.OpenAsync();
            result.Connected = true;
            await conn.ChangeDatabaseAsync(oldDatabaseName);
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
