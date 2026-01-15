using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LINGYUN.Abp.MicroService.MessageService;
public class MessageServiceMigrationsDbContextFactory : IDesignTimeDbContextFactory<MessageServiceMigrationsDbContext>
{
    public MessageServiceMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");

        var builder = new DbContextOptionsBuilder<MessageServiceMigrationsDbContext>()
            .UseNpgsql(connectionString);

        return new MessageServiceMigrationsDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LINGYUN.Abp.MicroService.MessageService.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
