using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LY.MicroService.LocalizationManagement.EntityFrameworkCore
{
    public class LocalizationManagementMigrationsDbContextFactory : IDesignTimeDbContextFactory<LocalizationManagementMigrationsDbContext>
    {
        public LocalizationManagementMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<LocalizationManagementMigrationsDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new LocalizationManagementMigrationsDbContext(builder.Options);
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
