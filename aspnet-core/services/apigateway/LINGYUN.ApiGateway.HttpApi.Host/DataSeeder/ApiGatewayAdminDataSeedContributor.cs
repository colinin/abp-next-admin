using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace DataSeeder
{
    public class ApiGatewayAdminDataSeedContributor : IDataSeedContributor, IScopedDependency
    {
        protected IConfiguration Configuration { get; }
        public ApiGatewayAdminDataSeedContributor(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            // Search file: ApiGateway-init.sql
            var scriptCommandFilePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../database");
            var scriptCommandFile = Path.Combine(scriptCommandFilePath, "ApiGateway-Init.sql");
            if (File.Exists(scriptCommandFile))
            {
                // read all script
                var apigatewayInitScript = await FileHelper.ReadAllTextAsync(scriptCommandFile);
                if (!apigatewayInitScript.IsNullOrWhiteSpace())
                {
                    // open connection and excute the apigateway-init script
                    var connectionString = Configuration.GetConnectionString("ApiGateway");
                    using var connection = new MySqlConnection(connectionString);
                    await connection.OpenAsync();
                    var sqlTransaction = connection.BeginTransaction();
                    var sqlCommand = new MySqlCommand(apigatewayInitScript, connection, sqlTransaction);
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
