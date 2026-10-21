using LINGYUN.Abp.OpenObserve;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

[DependsOn(
    typeof(AbpSerilogEnrichersApplicationModule),
    typeof(AbpSerilogEnrichersUniqueIdModule),
    typeof(AbpLoggingModule),
    typeof(AbpOpenObserveModule))]
public class AbpLoggingSerilogOpenObserveModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpLoggingSerilogOpenObserveOptions>(configuration.GetSection("Logging:Serilog:OpenObserve"));

        context.Services.AddKeyedTransient<ILoggingProvider, SerilogOpenObserveLoggingProvider>("OpenObserveLoggingProvider");
    }
}
