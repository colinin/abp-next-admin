using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;
public class ProjectNameDbContextFactory : IDesignTimeDbContextFactory<ProjectNameDbContext>
{
    public ProjectNameDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("ProjectName");

        DbContextOptionsBuilder<ProjectNameDbContext>? builder = null;

        try
        {
#if MySQL
            builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => 
                        b.MigrationsAssembly(GetType().Assembly));
#elif SqlServer
            builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
                .UseSqlServer(connectionString, b => b.MigrationsAssembly(GetType().Assembly));
#elif Sqlite
            builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
                .UseSqlite(connectionString, b => b.MigrationsAssembly(GetType().Assembly));
#elif Oracle
            builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
                .UseOracle(connectionString, b => b.MigrationsAssembly(GetType().Assembly));
#elif OracleDevart
            builder = (DbContextOptionsBuilder<ProjectNameDbContext>) new DbContextOptionsBuilder<ProjectNameDbContext>()
                .UseOracle(connectionString, b => b.MigrationsAssembly(GetType().Assembly));
#elif PostgreSql
            builder = new DbContextOptionsBuilder<ProjectNameDbContext>()
                .UseNpgsql(connectionString, b => b.MigrationsAssembly(GetType().Assembly));
#endif
            return new ProjectNameDbContext(builder!.Options);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("[ProjectName] - Database migrations failed: {message}", ex.Message);

            Console.ResetColor();

            throw;
        }
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PackageName.CompanyName.ProjectName.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}

