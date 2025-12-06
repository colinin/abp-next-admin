using LINGYUN.Abp.Elsa.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql.Migrations;

public class PostgreSqlElsaDataBaseInstaller : IElsaDataBaseInstaller, ITransientDependency
{
    public ILogger<PostgreSqlElsaDataBaseInstaller> Logger { protected get; set; }

    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly IConnectionStringResolver _connectionStringResolver;

    private readonly AbpElsaDataBaseInstallerOptions _installerOptions;

    public PostgreSqlElsaDataBaseInstaller(
        IVirtualFileProvider virtualFileProvider,
        IConnectionStringResolver connectionStringResolver,
        IOptions<AbpElsaDataBaseInstallerOptions> installerOptions)
    {
        _installerOptions = installerOptions.Value;
        _virtualFileProvider = virtualFileProvider;
        _connectionStringResolver = connectionStringResolver;

        Logger = NullLogger<PostgreSqlElsaDataBaseInstaller>.Instance;
    }

    public async virtual Task InstallAsync()
    {
        var connectionString = await _connectionStringResolver.ResolveAsync("Workflow");
        if (connectionString.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("Please configure the `Workflow` database connection string Workflow!");
            throw new ArgumentNullException(nameof(connectionString));
        }

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        var dataBaseName = await CreateDataBaseIfNotExists(builder.Database, builder);

        builder.Database = dataBaseName;

        using var npgsqlConnection = new NpgsqlConnection(builder.ConnectionString);

        if (npgsqlConnection.State == ConnectionState.Closed)
        {
            await npgsqlConnection.OpenAsync();
        }

        var tableParams = _installerOptions.InstallTables.Select((_, index) => $"@Table_{index}").JoinAsString(",");
        // ElsaContext中默认使用Elsa作为默认Schema
        using (var npgsqlCommand = new NpgsqlCommand($"SELECT COUNT(1) FROM information_schema.tables WHERE table_catalog = @DataBaseName AND table_schema = 'Elsa' AND table_name IN ({tableParams});", npgsqlConnection))
        {
            npgsqlCommand.Parameters.Add("@DataBaseName", NpgsqlDbType.Text).Value = dataBaseName;
            for (var index = 0; index < _installerOptions.InstallTables.Count; index++)
            {
                npgsqlCommand.Parameters.Add($"@Table_{index}", NpgsqlDbType.Text).Value = _installerOptions.InstallTables[index];
            }

            var rowsAffects = await npgsqlCommand.ExecuteScalarAsync() as long?;
            if (rowsAffects > 0)
            {
                Logger.LogInformation($"The `{dataBaseName}` database has already exists.");
                return;
            }
        }

        var sqlScript = await GetInitSqlScript();

        // ${DataBase} 替换
        sqlScript = sqlScript.ReplaceFirst("${DataBase}", dataBaseName);

        using (var npgsqlCommand = new NpgsqlCommand(sqlScript, npgsqlConnection))
        {
            Logger.LogInformation("The database initialization script `Initial.sql` starts...");

            await npgsqlCommand.ExecuteNonQueryAsync();
        }

        Logger.LogInformation("Database initialization script `Initial.sql` complete!");
    }

    public async virtual Task<string> CreateDataBaseIfNotExists(string dataBase, NpgsqlConnectionStringBuilder connectionStringBuilder)
    {
        // 切换主数据库查询数据库是否存在
        connectionStringBuilder.Database = "postgres";
        using var npgsqlConnection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);
        if (npgsqlConnection.State == ConnectionState.Closed)
        {
            await npgsqlConnection.OpenAsync();
        }

        var checkDataBaseName = "";
        using (var npgsqlCommand = new NpgsqlCommand("SELECT datname FROM pg_database WHERE datname = @DataBaseName;", npgsqlConnection))
        {
            var dataBaseParamter = npgsqlCommand.Parameters.Add("DataBaseName", NpgsqlDbType.Text);
            dataBaseParamter.Value = dataBase;

            checkDataBaseName = await npgsqlCommand.ExecuteScalarAsync() as string;
        }

        if (checkDataBaseName.IsNullOrWhiteSpace())
        {
            using (var npgsqlCommand = new NpgsqlCommand($"CREATE DATABASE \"{dataBase}\" ENCODING = 'UTF8' LC_COLLATE = 'en_US.UTF-8' LC_CTYPE = 'en_US.UTF-8';", npgsqlConnection))
            {
                await npgsqlCommand.ExecuteNonQueryAsync();
            }
        }

        return dataBase;
    }

    public async virtual Task<string> GetInitSqlScript()
    {
        var sqlScriptFileInfo = _virtualFileProvider.GetFileInfo("/LINGYUN/Abp/Elsa/EntityFrameworkCore/PostgreSql/Migrations/Initial.sql");
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
