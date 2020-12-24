using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LINGYUN.ApiGateway.EntityFrameworkCore
{
    public class HttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<HttpApiHostMigrationsDbContext>
    {
        public HttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<HttpApiHostMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(connectionString));

            return new HttpApiHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false);

            return builder.Build();
        }
    }
}
