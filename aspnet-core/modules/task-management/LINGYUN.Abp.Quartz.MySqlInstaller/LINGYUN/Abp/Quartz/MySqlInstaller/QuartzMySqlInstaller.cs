using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Quartz;
using Volo.Abp.VirtualFileSystem;
using static Quartz.SchedulerBuilder;

namespace LINGYUN.Abp.Quartz.MySqlInstaller;

public class QuartzMySqlInstaller : ITransientDependency
{
    public ILogger<QuartzMySqlInstaller> Logger { protected get; set; }

    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly AbpQuartzOptions _quartzOptions;

    public QuartzMySqlInstaller(
        IVirtualFileProvider virtualFileProvider,
        IOptions<AbpQuartzOptions> quartzOptions)
    {
        _quartzOptions = quartzOptions.Value;
        _virtualFileProvider = virtualFileProvider;

        Logger = NullLogger<QuartzMySqlInstaller>.Instance;
    }

    public async virtual Task InstallAsync()
    {
        var dataSource = _quartzOptions.Properties["quartz.jobStore.dataSource"] ?? AdoProviderOptions.DefaultDataSourceName;
        var connectionString = _quartzOptions.Properties[$"quartz.dataSource.{dataSource}.connectionString"];
        var tablePrefix = _quartzOptions.Properties["quartz.jobStore.tablePrefix"] ?? "QRTZ_";

        if (connectionString.IsNullOrWhiteSpace())
        {
            Logger.LogWarning($"Please configure the `{dataSource}` database connection string in `quartz.jobStore.dataSource`!");
            throw new ArgumentNullException(nameof(connectionString));
        }

        Logger.LogInformation("Install Quartz MySql...");

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
                Logger.LogInformation($"The `{dataBaseName}` tables has already exists.");
                return;
            }
        }

        var sqlScript = await GetInitSqlScript();

        // USE `${DataBase}`  ->  USE `Workflow`;
        sqlScript = sqlScript.ReplaceFirst("${DataBase}", dataBaseName);
        // CREATE TABLE $(TablePrefix)JOB_DETAILS`  ->  CREATE TABLE QRTZ_JOB_DETAILS;
        sqlScript = sqlScript.Replace("${TablePrefix}", tablePrefix);

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
        var sqlScriptFileInfo = _virtualFileProvider.GetFileInfo("/LINGYUN/Abp/Quartz/MySqlInstaller/Scripts/Initial.sql");
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
