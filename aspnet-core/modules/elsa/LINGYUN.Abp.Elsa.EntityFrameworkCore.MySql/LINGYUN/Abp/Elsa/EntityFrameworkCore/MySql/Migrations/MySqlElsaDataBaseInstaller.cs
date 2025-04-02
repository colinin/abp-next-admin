using LINGYUN.Abp.Elsa.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql.Migrations;

[Dependency(ReplaceServices = true)]
public class MySqlElsaDataBaseInstaller : IElsaDataBaseInstaller, ITransientDependency
{
    public ILogger<MySqlElsaDataBaseInstaller> Logger { protected get; set; }

    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly IConnectionStringResolver _connectionStringResolver;

    public MySqlElsaDataBaseInstaller(
        IVirtualFileProvider virtualFileProvider,
        IConnectionStringResolver connectionStringResolver)
    {
        _virtualFileProvider = virtualFileProvider;
        _connectionStringResolver = connectionStringResolver;

        Logger = NullLogger<MySqlElsaDataBaseInstaller>.Instance;
    }

    public async virtual Task InstallAsync()
    {
        var connectionString = await _connectionStringResolver.ResolveAsync("Workflow");
        if (connectionString.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("Please configure the `Workflow` database connection string Workflow!");
            throw new ArgumentNullException(nameof(connectionString));
        }

        var builder = new MySqlConnectionStringBuilder(connectionString);

        var dataBaseName = await CreateDataBaseIfNotExists(builder.Database, builder);

        builder.Database = dataBaseName;

        using var mySqlConnection = new MySqlConnection(builder.ConnectionString);

        if (mySqlConnection.State == ConnectionState.Closed)
        {
            await mySqlConnection.OpenAsync();
        }

        using (var mySqlCommand = new MySqlCommand("SELECT COUNT(1) FROM `information_schema`.`TABLES` WHERE `TABLE_SCHEMA` = @DataBaseName;", mySqlConnection))
        {
            mySqlCommand.Parameters.Add("@DataBaseName", MySqlDbType.String).Value = dataBaseName;

            var rowsAffects = await mySqlCommand.ExecuteScalarAsync() as long?;
            if (rowsAffects > 0)
            {
                Logger.LogInformation($"The `{dataBaseName}` database has already exists.");
                return;
            }
        }

        var sqlScript = await GetInitSqlScript();

        // USE `${DataBase}`  ->  USE `Workflow`;;
        sqlScript = sqlScript.ReplaceFirst("${DataBase}", dataBaseName);

        using (var mySqlCommand = new MySqlCommand(sqlScript, mySqlConnection))
        {
            Logger.LogInformation("The database initialization script `Initial.sql` starts...");

            await mySqlCommand.ExecuteNonQueryAsync();
        }

        Logger.LogInformation("Database initialization script `Initial.sql` complete!");
    }

    public async virtual Task<string> CreateDataBaseIfNotExists(string dataBase, MySqlConnectionStringBuilder connectionStringBuilder)
    {
        // 切换主数据库查询数据库是否存在
        connectionStringBuilder.Database = "mysql";
        using var mySqlConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);
        if (mySqlConnection.State == ConnectionState.Closed)
        {
            await mySqlConnection.OpenAsync();
        }

        var checkDataBaseName = "";
        using (var mySqlCommand = new MySqlCommand("SELECT `SCHEMA_NAME` FROM `information_schema`.`SCHEMATA` WHERE `SCHEMA_NAME` = @DataBaseName;", mySqlConnection))
        {
            var dataBaseParamter = mySqlCommand.Parameters.Add("DataBaseName", DbType.String);
            dataBaseParamter.Value = dataBase;

            checkDataBaseName = await mySqlCommand.ExecuteScalarAsync() as string;
        }

        if (checkDataBaseName.IsNullOrWhiteSpace())
        {
            using (var mySqlCommand = new MySqlCommand($"CREATE DATABASE `{dataBase}` CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_general_ci';", mySqlConnection))
            {
                await mySqlCommand.ExecuteNonQueryAsync();
            }
        }

        return dataBase;
    }

    public async virtual Task<string> GetInitSqlScript()
    {
        var sqlScriptFileInfo = _virtualFileProvider.GetFileInfo("/LINGYUN/Abp/Elsa/EntityFrameworkCore/MySql/Migrations/Initial.sql");
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
