using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LY.MicroService.WebhooksManagement.EntityFrameworkCore;

public class WebhooksManagementMigrationsDbContextFactory : IDesignTimeDbContextFactory<WebhooksManagementMigrationsDbContext>
{
    public WebhooksManagementMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("WebhooksManagement");

        DbContextOptionsBuilder<WebhooksManagementMigrationsDbContext> builder = null;

        builder = new DbContextOptionsBuilder<WebhooksManagementMigrationsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new WebhooksManagementMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true);

        return builder.Build();
    }
}
