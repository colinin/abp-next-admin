using LINGYUN.Abp.OpenObserve;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

[DependsOn(
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
