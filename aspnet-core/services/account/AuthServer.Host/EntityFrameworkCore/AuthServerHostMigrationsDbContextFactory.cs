using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace AuthServer.EntityFrameworkCore
{
    public class AuthServerHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<AuthServerHostMigrationsDbContext>
    {
        public AuthServerHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<AuthServerHostMigrationsDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new AuthServerHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var envFile = Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.{env}.json");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            if (File.Exists(envFile))
            {
                builder.AddJsonFile($"appsettings.{env}.json", optional: false);
            }

            return builder.Build();
        }
    }
}
