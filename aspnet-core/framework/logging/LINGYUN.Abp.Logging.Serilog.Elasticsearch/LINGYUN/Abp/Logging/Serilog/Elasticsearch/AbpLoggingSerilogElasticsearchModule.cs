using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpLoggingModule),
    typeof(AbpElasticsearchModule),
    typeof(AbpMapperlyModule),
    typeof(AbpJsonModule))]
public class AbpLoggingSerilogElasticsearchModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpLoggingSerilogElasticsearchOptions>(configuration.GetSection("Logging:Serilog:Elasticsearch"));

        context.Services.AddMapperlyObjectMapper<AbpLoggingSerilogElasticsearchModule>();

        context.Services.AddKeyedSingleton<ILoggingProvider, SerilogElasticsearchLoggingProvider>("ElasticsearchLoggingProvider");
    }
}
