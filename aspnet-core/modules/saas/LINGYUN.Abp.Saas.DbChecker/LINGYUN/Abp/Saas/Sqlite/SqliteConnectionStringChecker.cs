using LINGYUN.Abp.Saas.Tenants;
using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Saas.Sqlite;

public class SqliteConnectionStringChecker : IDataBaseConnectionStringChecker, ITransientDependency
{
    public virtual async Task<DataBaseConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new DataBaseConnectionStringCheckResult();

        try
        {
            await using var conn = new SqliteConnection(connectionString);
            await conn.OpenAsync();
            result.Connected = true;
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

