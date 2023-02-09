using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LY.MicroService.IdentityServer.EntityFrameworkCore;

public class IdentityServerMigrationsDbContextFactory : IDesignTimeDbContextFactory<IdentityServerMigrationsDbContext>
{
    public IdentityServerMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");

        var builder = new DbContextOptionsBuilder<IdentityServerMigrationsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new IdentityServerMigrationsDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LY.MicroService.IdentityServer.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
