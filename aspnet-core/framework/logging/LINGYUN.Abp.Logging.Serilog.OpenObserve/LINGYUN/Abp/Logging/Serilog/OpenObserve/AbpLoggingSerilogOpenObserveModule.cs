using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Volo.Abp.Json;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

[DependsOn(
    typeof(AbpLoggingModule),
    typeof(AbpMapperlyModule),
    typeof(AbpJsonModule))]
public class AbpLoggingSerilogOpenObserveModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpLoggingSerilogOpenObserveOptions>(configuration.GetSection("Logging:Serilog:OpenObserve"));

        context.Services.AddMapperlyObjectMapper<AbpLoggingSerilogOpenObserveModule>();

        context.Services.AddOpenObserveHttpClient();

        context.Services.AddKeyedTransient<ILoggingProvider, SerilogOpenObserveLoggingProvider>("OpenObserveLoggingProvider");
    }
}
