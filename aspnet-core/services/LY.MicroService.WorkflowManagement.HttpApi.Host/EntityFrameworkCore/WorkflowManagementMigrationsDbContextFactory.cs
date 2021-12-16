using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LY.MicroService.WorkflowManagement.EntityFrameworkCore;

public class WorkflowManagementMigrationsDbContextFactory : IDesignTimeDbContextFactory<WorkflowManagementMigrationsDbContext>
{
    public WorkflowManagementMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("WorkflowManagement");

        var builder = new DbContextOptionsBuilder<WorkflowManagementMigrationsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new WorkflowManagementMigrationsDbContext(builder.Options);
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
