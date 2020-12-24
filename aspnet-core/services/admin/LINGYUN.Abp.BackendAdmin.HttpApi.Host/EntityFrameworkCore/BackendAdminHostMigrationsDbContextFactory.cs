using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LINGYUN.Abp.BackendAdmin.EntityFrameworkCore
{
    public class BackendAdminHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<BackendAdminHostMigrationsDbContext>
    {
        public BackendAdminHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<BackendAdminHostMigrationsDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new BackendAdminHostMigrationsDbContext(builder.Options);
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
