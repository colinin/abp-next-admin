using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Telemetry.APM;

public class AbpTelemetryAPMModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var isAPMEnabled = configuration["APM:IsEnabled"];
        if (isAPMEnabled.IsNullOrWhiteSpace() || "false".Equals(isAPMEnabled.ToLower()))
        {
            return;
        }

        context.Services.AddAllElasticApm();
    }
}
