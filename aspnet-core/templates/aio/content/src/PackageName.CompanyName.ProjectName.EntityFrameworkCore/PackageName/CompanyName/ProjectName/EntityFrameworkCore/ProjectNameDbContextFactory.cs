using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;
public class ProjectNameDbContextFactory : IDesignTimeDbContextFactory<ProjectNameDbContext>
{
    public ProjectNameDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("ProjectName");

        DbContextOptionsBuilder<ProjectNameDbContext> builder = null;

#if MySQL
        builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
#elif SqlServer 
        builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
            .UseSqlServer(connectionString);
#elif Sqlite 
        builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
            .UseSqlite(connectionString);
#elif Oracle 
        builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
            .UseOracle(connectionString);
#elif OracleDevart 
        builder = (DbContextOptionsBuilder<ProjectNameDbContext>) new DbContextOptionsBuilder<ProjectNameDbContext>()
            .UseOracle(connectionString);
#elif PostgreSql 
        builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
            .UseNpgsql(connectionString);
#endif

        return new ProjectNameDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PackageName.CompanyName.ProjectName.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true);

        return builder.Build();
    }
}

