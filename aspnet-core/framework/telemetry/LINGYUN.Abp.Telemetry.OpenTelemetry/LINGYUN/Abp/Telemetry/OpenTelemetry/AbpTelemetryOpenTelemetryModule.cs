using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Telemetry.OpenTelemetry;

public class AbpTelemetryOpenTelemetryModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        var openTelmetrySetup = context.Services.GetPreConfigureActions<OpenTelemetryBuilder>();

        var openTelemetryEnabled = configuration["OpenTelemetry:IsEnabled"];
        if (openTelemetryEnabled.IsNullOrWhiteSpace() || "false".Equals(openTelemetryEnabled.ToLower()))
        {
            return;
        }

        var applicationName = configuration["OpenTelemetry:ServiceName"];
        if (applicationName.IsNullOrWhiteSpace())
        {
            applicationName = context.Services.GetApplicationName();
        }
        var openTelmetryBuilder = context.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(applicationName);
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(applicationName);
                ConfigureTracing(tracing, configuration);
            })
            .WithMetrics(metrics =>
            {
                ConfigureMetrics(metrics, configuration);
            });

        openTelmetrySetup.Configure(openTelmetryBuilder);
    }

    private static void ConfigureTracing(TracerProviderBuilder tracing, IConfiguration configuration)
    {
        tracing.AddHttpClientInstrumentation();
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddCapInstrumentation();
        tracing.AddEntityFrameworkCoreInstrumentation(efcore =>
        {
            efcore.SetDbStatementForText = configuration.GetValue(
                "OpenTelemetry:EntityFrameworkCore:SetDbStatementForText", 
                efcore.SetDbStatementForText);

            efcore.SetDbStatementForStoredProcedure = configuration.GetValue(
                "OpenTelemetry:EntityFrameworkCore:SetDbStatementForStoredProcedure",
                efcore.SetDbStatementForStoredProcedure);
        });

        if (configuration.GetValue("OpenTelemetry:Console:IsEnabled", false))
        {
            tracing.AddConsoleExporter();
        }

        var tracingOtlpEndpoint = configuration["OpenTelemetry:Otlp:Endpoint"];
        if (!tracingOtlpEndpoint.IsNullOrWhiteSpace())
        {
            tracing.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
            });
            return;
        }

        var zipkinEndpoint = configuration["OpenTelemetry:ZipKin:Endpoint"];
        if (!zipkinEndpoint.IsNullOrWhiteSpace())
        {
            tracing.AddZipkinExporter(zipKinOptions =>
            {
                zipKinOptions.Endpoint = new Uri(zipkinEndpoint);
            });
            return;
        }
    }

    private static void ConfigureMetrics(MeterProviderBuilder metrics, IConfiguration configuration)
    {
        metrics.AddRuntimeInstrumentation();
        metrics.AddHttpClientInstrumentation();
        metrics.AddAspNetCoreInstrumentation();

        if (configuration.GetValue("OpenTelemetry:Console:IsEnabled", false))
        {
            metrics.AddConsoleExporter();
        }

        var tracingOtlpEndpoint = configuration["OpenTelemetry:Otlp:Endpoint"];
        if (!tracingOtlpEndpoint.IsNullOrWhiteSpace())
        {
            metrics.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
            });
            return;
        }
    }
}
