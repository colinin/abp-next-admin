using LINGYUN.Abp.Saas.Tenants;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Saas.Oracle;

public class OracleConnectionStringChecker : IDataBaseConnectionStringChecker
{
    public virtual async Task<DataBaseConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        var result = new DataBaseConnectionStringCheckResult();
        var connString = new OracleConnectionStringBuilder(connectionString)
        {
            ConnectionTimeout = 1
        };

        try
        {
            await using var conn = new OracleConnection(connString.ConnectionString);
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
