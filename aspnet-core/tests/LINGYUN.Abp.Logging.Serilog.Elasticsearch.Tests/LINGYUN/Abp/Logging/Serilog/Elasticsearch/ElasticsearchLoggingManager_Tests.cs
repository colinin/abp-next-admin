using Microsoft.Extensions.DependencyInjection;
using NSubstitute.Extensions;
using Serilog;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch;

public class ElasticsearchLoggingManager_Tests : LoggingManager_Tests<AbpLoggingSerilogElasticsearchTestModule>
{
    protected override void BeforeAddApplication(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithUniqueId()
            .WriteTo.Elasticsearch(
                nodeUris: "http://localhost:9200",
                indexFormat: "abp-test-logging")
            .CreateLogger();

        services.AddLogging(logging =>
        {
            logging.AddSerilog();
        });
    }
}
