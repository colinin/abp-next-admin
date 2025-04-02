using LINGYUN.Abp.Quartz.SqlInstaller;
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
using Volo.Abp.DependencyInjection;
using Volo.Abp.Quartz;
using Volo.Abp.VirtualFileSystem;
using static Quartz.SchedulerBuilder;

namespace LINGYUN.Abp.Quartz.SqlServerInstaller;

public class QuartzSqlServerInstaller : ITransientDependency
{
    public ILogger<QuartzSqlServerInstaller> Logger { protected get; set; }

    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly AbpQuartzSqlInstallerOptions _installerOptions;
    private readonly AbpQuartzOptions _quartzOptions;

    public QuartzSqlServerInstaller(
        IVirtualFileProvider virtualFileProvider,
        IOptions<AbpQuartzOptions> quartzOptions,
        IOptions<AbpQuartzSqlInstallerOptions> installerOptions)
    {
        _quartzOptions = quartzOptions.Value;
        _virtualFileProvider = virtualFileProvider;
        _installerOptions = installerOptions.Value;

        Logger = NullLogger<QuartzSqlServerInstaller>.Instance;
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

        Logger.LogInformation("Install Quartz SqlServer...");

        var builder = new SqlConnectionStringBuilder(connectionString);

        var dataBaseName = await CreateDataBaseIfNotExists(builder.InitialCatalog, builder);

        builder.InitialCatalog = dataBaseName;

        using var sqlConnection = new SqlConnection(builder.ConnectionString);

        if (sqlConnection.State == ConnectionState.Closed)
        {
            await sqlConnection.OpenAsync();
        }

        var tableParams = _installerOptions.InstallTables.Select((_, index) => $"@Table_{index}").JoinAsString(",");
        using (var sqlCommand = new SqlCommand($"SELECT COUNT(1) FROM [sys].[objects] WHERE type=N'U' AND name IN ({tableParams})", sqlConnection))
        {
            sqlCommand.Parameters.Add("@DataBaseName", SqlDbType.NVarChar).Value = dataBaseName;

            for (var index = 0; index < _installerOptions.InstallTables.Count; index++)
            {
                sqlCommand.Parameters.Add($"@Table_{index}", SqlDbType.NVarChar).Value = $"{tablePrefix}{_installerOptions.InstallTables[index]}";
            }

            var rowsAffects = await sqlCommand.ExecuteScalarAsync() as long?;
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
        using (var sqlCommand = new SqlCommand("SELECT [name] FROM [master].[dbo].[sysdatabases] WHERE [name] = @DataBaseName;", sqlConnection))
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
        var sqlScriptFileInfo = _virtualFileProvider.GetFileInfo("/LINGYUN/Abp/Quartz/SqlServerInstaller/Scripts/Initial.sql");
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
