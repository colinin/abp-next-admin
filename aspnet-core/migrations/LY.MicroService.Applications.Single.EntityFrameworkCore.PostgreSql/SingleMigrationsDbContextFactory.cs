using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql;

public class SingleMigrationsDbContextFactory : IDesignTimeDbContextFactory<SingleMigrationsDbContext>
{
    public SingleMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");

        var builder = new DbContextOptionsBuilder<SingleMigrationsDbContext>()
            .UseNpgsql(connectionString,
                b => b.MigrationsAssembly("LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql"));

        return new SingleMigrationsDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),
                "../LY.MicroService.Applications.Single.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile(
                "appsettings.PostgreSql.json",
                optional: false);

        return builder.Build();
    }
}