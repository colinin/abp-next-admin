using LINGYUN.Abp.MicroService.TaskService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

var defaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] [{ProcessId}] [{ThreadId}] - {Message:lj}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
        .MinimumLevel.Override("LINGYUN.Abp.MicroService.TaskService", LogEventLevel.Debug)
#else
        .MinimumLevel.Override("LINGYUN.Abp.MicroService.TaskService", LogEventLevel.Information)
#endif
        .Enrich.FromLogContext()
    .WriteTo.Async(x => x.Console(outputTemplate: defaultOutputTemplate))
    .WriteTo.Async(x => x.File("Logs/migrations.txt", outputTemplate: defaultOutputTemplate))
    .CreateLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args)
        .AddAppSettingsSecretsJson()
        .ConfigureLogging((context, logging) => logging.ClearProviders())
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<TaskServiceDbMigratorHostedService>();
        });
    await builder.RunConsoleAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
}
finally
{
    await Log.CloseAndFlushAsync();
}