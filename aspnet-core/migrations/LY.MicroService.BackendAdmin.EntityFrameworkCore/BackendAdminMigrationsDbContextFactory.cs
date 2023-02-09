using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore;

public class BackendAdminMigrationsDbContextFactory : IDesignTimeDbContextFactory<BackendAdminMigrationsDbContext>
{
    public BackendAdminMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");

        var builder = new DbContextOptionsBuilder<BackendAdminMigrationsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new BackendAdminMigrationsDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LY.MicroService.BackendAdmin.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
