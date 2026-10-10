using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.SqlServer;

public class SingleMigrationsDbContextFactory : IDesignTimeDbContextFactory<SingleMigrationsDbContext>
{
    public SingleMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");

        var builder = new DbContextOptionsBuilder<SingleMigrationsDbContext>()
            .UseSqlServer(connectionString,
                b => b.MigrationsAssembly("LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.SqlServer"));

        return new SingleMigrationsDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),
                "../LINGYUN.Abp.MicroService.AllInOne.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile(
                "appsettings.SqlServer.json",
                optional: false);

        return builder.Build();
    }
}
