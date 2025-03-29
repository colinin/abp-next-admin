using Microsoft.Extensions.DependencyInjection;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.CAP;
using SkyApm.Utilities.DependencyInjection;
using System;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Telemetry.SkyWalking;

public class AbpTelemetrySkyWalkingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var isSkywalkingEnabled = configuration["SkyWalking:Enable"];
        if (isSkywalkingEnabled.IsNullOrWhiteSpace() || "false".Equals(isSkywalkingEnabled.ToLower()))
        {
            return;
        }

        var applicationName = configuration["SkyWalking:ServiceName"];
        if (applicationName.IsNullOrWhiteSpace())
        {
            applicationName = context.Services.GetApplicationName();
        }

        if (applicationName.IsNullOrWhiteSpace())
        {
            return;
        }

        Environment.SetEnvironmentVariable("SKYWALKING__SERVICENAME", applicationName);

        var skywalkingSetup = context.Services.GetPreConfigureActions<SkyApmExtensions>();

        context.Services.AddSkyWalking(setup =>
        {
            setup.AddAspNetCoreHosting();
            setup.AddCap();

            skywalkingSetup.Configure(setup);
        });
    }
}
