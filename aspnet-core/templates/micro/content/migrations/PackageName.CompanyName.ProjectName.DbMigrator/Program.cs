using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PackageName.CompanyName.ProjectName.DbMigrator;
using Serilog;
using Serilog.Events;

try
{
    Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
                .MinimumLevel.Override("PackageName.CompanyName.ProjectName", LogEventLevel.Debug)
#else
                .MinimumLevel.Override("PackageName.CompanyName.ProjectName", LogEventLevel.Information)
#endif
                .Enrich.FromLogContext()
            .WriteTo.File("Logs/logs.txt")
            .WriteTo.Console()
            .CreateLogger();

    var builder = Host.CreateDefaultBuilder(args)
        .AddAppSettingsSecretsJson()
        .ConfigureLogging((context, logger) => logger.ClearProviders())
        .ConfigureServices((hostContext, services) => services.AddHostedService<DbMigratorHostedService>());

    await builder.RunConsoleAsync();

    return 0;
}
catch (Exception ex)
{
    if (ex is HostAbortedException)
    {
        throw;
    }

    Log.Fatal(ex, "Host terminated unexpectedly!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}