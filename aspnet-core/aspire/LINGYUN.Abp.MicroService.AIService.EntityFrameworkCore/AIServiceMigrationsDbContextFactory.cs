using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LINGYUN.Abp.MicroService.AIService;
public class AIServiceMigrationsDbContextFactory : IDesignTimeDbContextFactory<AIServiceMigrationsDbContext>
{
    public AIServiceMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");

        var builder = new DbContextOptionsBuilder<AIServiceMigrationsDbContext>()
            .UseNpgsql(connectionString);

        return new AIServiceMigrationsDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LINGYUN.Abp.MicroService.AIService.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
