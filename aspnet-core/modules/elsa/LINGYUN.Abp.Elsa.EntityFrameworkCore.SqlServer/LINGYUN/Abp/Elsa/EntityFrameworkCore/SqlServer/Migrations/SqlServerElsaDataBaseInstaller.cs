using LINGYUN.Abp.Elsa.EntityFrameworkCore.Migrations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer.Migrations;

public class SqlServerElsaDataBaseInstaller : IElsaDataBaseInstaller, ITransientDependency
{
    public ILogger<SqlServerElsaDataBaseInstaller> Logger { protected get; set; }

    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly IConnectionStringResolver _connectionStringResolver;

    private readonly AbpElsaDataBaseInstallerOptions _installerOptions;

    public SqlServerElsaDataBaseInstaller(
        IVirtualFileProvider virtualFileProvider,
        IConnectionStringResolver connectionStringResolver,
        IOptions<AbpElsaDataBaseInstallerOptions> installerOptions)
    {
        _installerOptions = installerOptions.Value;
        _virtualFileProvider = virtualFileProvider;
        _connectionStringResolver = connectionStringResolver;

        Logger = NullLogger<SqlServerElsaDataBaseInstaller>.Instance;
    }

    public async virtual Task InstallAsync()
    {
        var connectionString = await _connectionStringResolver.ResolveAsync("Workflow");
        if (connectionString.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("Please configure the `Workflow` database connection string Workflow!");
            throw new ArgumentNullException(nameof(connectionString));
        }

        var builder = new SqlConnectionStringBuilder(connectionString);

        var dataBaseName = await CreateDataBaseIfNotExists(builder.InitialCatalog, builder);

        builder.InitialCatalog = dataBaseName;

        using var sqlConnection = new SqlConnection(builder.ConnectionString);

        if (sqlConnection.State == ConnectionState.Closed)
        {
            await sqlConnection.OpenAsync();
        }

        // 检查 Elsa schema
        using (var createSchemaCommand = new SqlCommand(@"
            IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'elsa')
            BEGIN
                EXEC('CREATE SCHEMA elsa');
            END", sqlConnection))
        {
            await createSchemaCommand.ExecuteNonQueryAsync();
        }

        // 检查表
        var tableParams = _installerOptions.InstallTables
            .Select((_, index) => $"@Table_{index}")
            .JoinAsString(",");

        using (var sqlCommand = new SqlCommand($@"
            SELECT COUNT(1) 
            FROM sys.tables t 
            INNER JOIN sys.schemas s ON t.schema_id = s.schema_id 
            WHERE s.name = 'elsa' 
            AND t.name IN ({tableParams});",
                sqlConnection))
        {
            for (var index = 0; index < _installerOptions.InstallTables.Count; index++)
            {
                sqlCommand.Parameters.Add($"@Table_{index}", SqlDbType.NVarChar, 128)
                    .Value = _installerOptions.InstallTables[index];
            }

            var rowsAffects = await sqlCommand.ExecuteScalarAsync() as int?;
            if (rowsAffects > 0)
            {
                Logger.LogInformation($"The `{dataBaseName}` tables in elsa schema already exist.");
                return;
            }
        }

        var sqlScript = await GetInitSqlScript();

        // USE `${DataBase}`  ->  USE `Workflow`;;
        sqlScript = sqlScript.ReplaceFirst("${DataBase}", dataBaseName);

        using (var sqlCommand = new SqlCommand(sqlScript, sqlConnection))
        {
            Logger.LogInformation("The database initialization script `Initial.sql` starts...");
            await sqlCommand.ExecuteNonQueryAsync();
        }

        Logger.LogInformation("Database initialization script `Initial.sql` complete!");
    }

    public async virtual Task<string> CreateDataBaseIfNotExists(string dataBase, SqlConnectionStringBuilder connectionStringBuilder)
    {
        // 切换主数据库查询数据库是否存在
        connectionStringBuilder.InitialCatalog = "master";
        using var sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
        if (sqlConnection.State == ConnectionState.Closed)
        {
            await sqlConnection.OpenAsync();
        }

        var checkDataBaseName = "";
        using (var sqlCommand = new SqlCommand("SELECT [name] FROM [sys].[databases] WHERE [name] = @DataBaseName;", sqlConnection))
        {
            var dataBaseParamter = sqlCommand.Parameters.Add("DataBaseName", SqlDbType.NVarChar);
            dataBaseParamter.Value = dataBase;

            checkDataBaseName = await sqlCommand.ExecuteScalarAsync() as string;
        }

        if (checkDataBaseName.IsNullOrWhiteSpace())
        {
            using (var sqlCommand = new SqlCommand($"CREATE DATABASE {dataBase};", sqlConnection))
            {
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        return dataBase;
    }

    public async virtual Task<string> GetInitSqlScript()
    {
        var sqlScriptFileInfo = _virtualFileProvider.GetFileInfo("/LINGYUN/Abp/Elsa/EntityFrameworkCore/SqlServer/Migrations/Initial.sql");
        if (!sqlScriptFileInfo.Exists || sqlScriptFileInfo.IsDirectory)
        {
            Logger.LogWarning("Please Check that the `Initial.sql` file exists!");
            throw new InvalidOperationException("The `Initial.sql` database initialization script file does not exist or is not valid!");
        }

        var sqlScript = await sqlScriptFileInfo.ReadAsStringAsync();
        if (sqlScript.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("The contents of the `Initial.sql` file are empty or invalid!");
            throw new InvalidOperationException("The contents of the `Initial.sql` file are empty or invalid!");
        }

        return sqlScript;
    }
}
