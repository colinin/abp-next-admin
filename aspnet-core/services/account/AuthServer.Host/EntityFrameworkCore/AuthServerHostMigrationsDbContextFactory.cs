using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AuthServer.EntityFrameworkCore
{
    public class AuthServerHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<AuthServerHostMigrationsDbContext>
    {
        public AuthServerHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AuthServerHostMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"));

            return new AuthServerHostMigrationsDbContext(builder.Options);
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
