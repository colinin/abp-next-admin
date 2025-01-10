using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PackageName.CompanyName.ProjectName.EntityFrameworkCore;
using Serilog;
using Volo.Abp;
using Volo.Abp.Data;

namespace PackageName.CompanyName.ProjectName.DbMigrator;

public class DbMigratorHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IConfiguration _configuration;

    public DbMigratorHostedService(
        IHostApplicationLifetime hostApplicationLifetime, 
        IConfiguration configuration)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var application = await AbpApplicationFactory
            .CreateAsync<PackageNameCompanyNameProjectNameDbMigratorModule>(options =>
        {
            options.Configuration.UserSecretsId = Environment.GetEnvironmentVariable("APPLICATION_USER_SECRETS_ID");
            options.Configuration.UserSecretsAssembly = typeof(DbMigratorHostedService).Assembly;
            options.Services.ReplaceConfiguration(_configuration);
            options.UseAutofac();
            options.Services.AddLogging(c => c.AddSerilog());
            options.AddDataMigrationEnvironment();
        });
        await application.InitializeAsync();

        await application
            .ServiceProvider
            .GetRequiredService<ProjectNameDbMigrationService>()
            .CheckAndApplyDatabaseMigrationsAsync();

        await application.ShutdownAsync();

        _hostApplicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

