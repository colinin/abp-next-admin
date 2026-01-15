using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LINGYUN.Abp.MicroService.AdminService;
public class AdminServiceMigrationsDbContextFactory : IDesignTimeDbContextFactory<AdminServiceMigrationsDbContext>
{
    public AdminServiceMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");

        var builder = new DbContextOptionsBuilder<AdminServiceMigrationsDbContext>()
            .UseNpgsql(connectionString);

        return new AdminServiceMigrationsDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LINGYUN.Abp.MicroService.AdminService.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
