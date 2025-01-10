using Microsoft.EntityFrameworkCore;
using PackageName.CompanyName.ProjectName.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName;

public class SingleMigrationsDbContextFactory : IDesignTimeDbContextFactory<SingleMigrationsDbContext>
{
    public SingleMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");
        DbContextOptionsBuilder<SingleMigrationsDbContext> builder = null;
#if MySQL
        builder = new DbContextOptionsBuilder<SingleMigrationsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName"));
#elif SqlServer 
        builder = new DbContextOptionsBuilder<SingleMigrationsDbContext>()
            .UseSqlServer(connectionString, b => b.MigrationsAssembly("PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName"));
#elif Sqlite 
        builder = new DbContextOptionsBuilder<SingleMigrationsDbContext>()
            .UseSqlite(connectionString, b => b.MigrationsAssembly("PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName"));
#elif Oracle 
        builder = new DbContextOptionsBuilder<SingleMigrationsDbContext>()
            .UseOracle(connectionString, b => b.MigrationsAssembly("PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName"));
#elif OracleDevart 
        builder = (DbContextOptionsBuilder<SingleMigrationsDbContext>) new DbContextOptionsBuilder<SingleMigrationsDbContext>()
            .UseOracle(connectionString, b => b.MigrationsAssembly("PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName"));
#elif PostgreSql 
        builder = new DbContextOptionsBuilder<SingleMigrationsDbContext>()
            .UseNpgsql(connectionString, b => b.MigrationsAssembly("PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName"));
#endif
        return new SingleMigrationsDbContext(builder!.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),
                "../PackageName.CompanyName.ProjectName.AIO.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
