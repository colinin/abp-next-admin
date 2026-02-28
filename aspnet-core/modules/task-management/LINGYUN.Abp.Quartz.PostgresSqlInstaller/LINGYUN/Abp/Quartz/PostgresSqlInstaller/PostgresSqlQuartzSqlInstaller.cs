using LINGYUN.Abp.Quartz.SqlInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Quartz;
using Volo.Abp.VirtualFileSystem;
using static Quartz.SchedulerBuilder;

namespace LINGYUN.Abp.Quartz.PostgresSqlInstaller;

public class PostgresSqlQuartzSqlInstaller : IQuartzSqlInstaller
{
    public ILogger<PostgresSqlQuartzSqlInstaller> Logger { protected get; set; }

    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly AbpQuartzSqlInstallerOptions _installerOptions;
    private readonly AbpQuartzOptions _quartzOptions;
    private readonly IConfiguration _configuration;

    public PostgresSqlQuartzSqlInstaller(
        IConfiguration configuration,
        IVirtualFileProvider virtualFileProvider,
        IOptions<AbpQuartzOptions> quartzOptions,
        IOptions<AbpQuartzSqlInstallerOptions> installerOptions)
    {
        _configuration = configuration;
        _quartzOptions = quartzOptions.Value;
        _virtualFileProvider = virtualFileProvider;
        _installerOptions = installerOptions.Value;

        Logger = NullLogger<PostgresSqlQuartzSqlInstaller>.Instance;
    }

    public bool CanInstall(string driverDelegateType)
    {
        return "Quartz.Impl.AdoJobStore.PostgreSQLDelegate,Quartz".Equals(driverDelegateType);
    }

    public async virtual Task InstallAsync()
    {
        var dataSource = _quartzOptions.Properties["quartz.jobStore.dataSource"] ?? AdoProviderOptions.DefaultDataSourceName;

        var connectionString = _quartzOptions.Properties[$"quartz.dataSource.{dataSource}.connectionString"];
        var connectionStringName = _quartzOptions.Properties[$"quartz.dataSource.{dataSource}.connectionStringName"];
        if (connectionString.IsNullOrWhiteSpace() && !connectionStringName.IsNullOrWhiteSpace())
        {
            connectionString = _configuration.GetConnectionString(connectionStringName);
        }
        var tablePrefix = _quartzOptions.Properties["quartz.jobStore.tablePrefix"] ?? "QRTZ_";

        if (connectionString.IsNullOrWhiteSpace())
        {
            Logger.LogWarning($"Please configure the `{dataSource}` database connection string in `quartz.jobStore.dataSource`!");
            throw new ArgumentNullException(nameof(connectionString));
        }

        Logger.LogInformation("Install Quartz PostgreSQL...");

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        var dataBaseName = await CreateDataBaseIfNotExists(builder.Database, builder);

        builder.Database = dataBaseName;

        using var connection = new NpgsqlConnection(builder.ConnectionString);

        if (connection.State == ConnectionState.Closed)
        {
            await connection.OpenAsync();
        }

        // 检查表是否存在
        var tableParams = _installerOptions.InstallTables.Select((_, index) => $"@Table_{index}").JoinAsString(",");
        using (var command = new NpgsqlCommand($@"
            SELECT COUNT(*) FROM information_schema.tables 
            WHERE table_catalog = @DataBaseName 
            AND table_schema = 'public' 
            AND table_name IN ({tableParams});", connection))
        {
            command.Parameters.AddWithValue("@DataBaseName", dataBaseName);

            for (var index = 0; index < _installerOptions.InstallTables.Count; index++)
            {
                command.Parameters.AddWithValue($"@Table_{index}", $"{tablePrefix}{_installerOptions.InstallTables[index]}");
            }

            var rowsAffects = await command.ExecuteScalarAsync() as long?;
            if (rowsAffects > 0)
            {
                Logger.LogInformation($"The `{dataBaseName}` tables has already exists.");
                return;
            }
        }

        var sqlScript = await GetInitSqlScript();

        // 替换表前缀
        sqlScript = sqlScript.Replace("${TablePrefix}", tablePrefix);

        // 分割 SQL 脚本，PostgreSQL 需要逐条执行
        var sqlCommands = sqlScript.Split(';', StringSplitOptions.RemoveEmptyEntries);

        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            foreach (var sqlCommandText in sqlCommands.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                using var command = new NpgsqlCommand(sqlCommandText.Trim(), connection, transaction);
                Logger.LogDebug($"Executing SQL: {sqlCommandText.Trim().Substring(0, Math.Min(100, sqlCommandText.Trim().Length))}...");
                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            Logger.LogInformation("Database initialization script `Initial.sql` complete!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Logger.LogError(ex, "Failed to execute database initialization script!");
            throw;
        }
    }

    public async virtual Task<string> CreateDataBaseIfNotExists(string dataBase, NpgsqlConnectionStringBuilder connectionStringBuilder)
    {
        // PostgreSQL 切换到 template1 数据库来创建新数据库
        var originalDatabase = connectionStringBuilder.Database;
        // 使用 postgres 系统数据库
        connectionStringBuilder.Database = "postgres"; 

        using var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);
        if (connection.State == ConnectionState.Closed)
        {
            await connection.OpenAsync();
        }

        // 检查数据库是否存在
        using (var command = new NpgsqlCommand(
            "SELECT 1 FROM pg_database WHERE datname = @DataBaseName;", connection))
        {
            command.Parameters.AddWithValue("@DataBaseName", dataBase);
            var exists = await command.ExecuteScalarAsync() != null;

            if (!exists)
            {
                // 创建数据库
                using var createCommand = new NpgsqlCommand(
                    $"CREATE DATABASE \"{dataBase}\" ENCODING = 'UTF8' LC_COLLATE = 'en_US.UTF-8' LC_CTYPE = 'en_US.UTF-8' TEMPLATE = template0;",
                    connection);

                await createCommand.ExecuteNonQueryAsync();
                Logger.LogInformation($"Database `{dataBase}` created successfully.");
            }
        }

        // 恢复原始数据库连接
        connectionStringBuilder.Database = originalDatabase;

        return dataBase;
    }

    public async virtual Task<string> GetInitSqlScript()
    {
        // 获取数据库初始化脚本
        var sqlScriptFileInfo = _virtualFileProvider.GetFileInfo("/LINGYUN/Abp/Quartz/PostgresSqlInstaller/Scripts/Initial.sql");
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
