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

            var builder = new DbContextOptionsBuilder<AuthServerHostMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"));

            return new AuthServerHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var envFile = Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.{env}.json");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            if (File.Exists(envFile))
            {
                builder.AddJsonFile($"appsettings.{env}.json", optional: false);
            }

            return builder.Build();
        }
    }
}
