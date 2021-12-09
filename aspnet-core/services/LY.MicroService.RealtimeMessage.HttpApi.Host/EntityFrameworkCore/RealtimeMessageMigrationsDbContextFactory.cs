using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LY.MicroService.RealtimeMessage.EntityFrameworkCore
{
    public class RealtimeMessageMigrationsDbContextFactory : IDesignTimeDbContextFactory<RealtimeMessageMigrationsDbContext>
    {
        public RealtimeMessageMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<RealtimeMessageMigrationsDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new RealtimeMessageMigrationsDbContext(builder.Options);
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
