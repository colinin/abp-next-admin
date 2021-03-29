using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
    public class LocalizationManagementHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<LocalizationManagementHttpApiHostMigrationsDbContext>
    {
        public LocalizationManagementHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<LocalizationManagementHttpApiHostMigrationsDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new LocalizationManagementHttpApiHostMigrationsDbContext(builder.Options);
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
