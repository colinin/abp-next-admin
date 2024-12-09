using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace LY.MicroService.Applications.Single.DbMigrator;

public class Program
{
    public async static Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
                .MinimumLevel.Override("LY.MicroService.Applications.Single.DbMigrator", LogEventLevel.Debug)
#else
                .MinimumLevel.Override("LY.MicroService.Applications.Single.DbMigrator", LogEventLevel.Information)
#endif
                .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/migrations.txt")
            .CreateLogger();
        await CreateHostBuilder(args).RunConsoleAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .AddAppSettingsSecretsJson()
            // .ConfigureAppConfiguration((context, builder) =>
            // {
            //     builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //         .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            // }  )
            .ConfigureLogging((context, logging) => logging.ClearProviders())
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<SingleDbMigratorHostedService>();
            });
    }
}
