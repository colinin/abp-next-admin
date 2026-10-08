using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

public class OpenObserveLoggingManager_Tests : LoggingManager_Tests<AbpLoggingSerilogOpenObserveTestModule>
{
    protected override void BeforeAddApplication(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithUniqueId()
            .WriteTo.OpenTelemetry(
                endpoint: "http://localhost:4318",
                protocol: OtlpProtocol.HttpProtobuf)
            .CreateLogger();

        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddSerilog();
        });
    }
}
