using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Telemetry.OpenTelemetry;

public class AbpTelemetryOpenTelemetryModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<AbpTelemetryOpenTelemetryOptions>(options =>
        {
            var ignureRequestLocalUrlPrefixs = configuration.GetSection("OpenTelemetry:IgnoreUrls:Local").Get<List<string>>();
            if (ignureRequestLocalUrlPrefixs != null)
            {
                options.IgnoreLocalRequestUrls = ignureRequestLocalUrlPrefixs;
            }
            var ignureRequestRemoteUrlPrefixs = configuration.GetSection("OpenTelemetry:IgnoreUrls:Remote").Get<List<string>>();
            if (ignureRequestRemoteUrlPrefixs != null)
            {
                options.IgnoreRemoteRequestUrls = ignureRequestRemoteUrlPrefixs;
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        var openTelmetrySetup = context.Services.GetPreConfigureActions<OpenTelemetryBuilder>();

        var isOpenTelmetryEnabled = configuration["OpenTelemetry:IsEnabled"];
        if (isOpenTelmetryEnabled.IsNullOrWhiteSpace() || "false".Equals(isOpenTelmetryEnabled.ToLower()))
        {
            return;
        }

        var openTelemetyOptions = context.Services.ExecutePreConfiguredActions(new AbpTelemetryOpenTelemetryOptions());

        var applicationName = configuration["OpenTelemetry:ServiceName"];
        if (applicationName.IsNullOrWhiteSpace())
        {
            applicationName = context.Services.GetApplicationName();
        }

        if (applicationName.IsNullOrWhiteSpace())
        {
            return;
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
                ConfigureTracing(tracing, configuration, openTelemetyOptions);
            })
            .WithMetrics(metrics =>
            {
                ConfigureMetrics(metrics, configuration);
            })
            .WithLogging(logging =>
            {
                ConfigureLogging(logging, configuration);
            });

        openTelmetrySetup.Configure(openTelmetryBuilder);
    }

    private static void ConfigureTracing(TracerProviderBuilder tracing, IConfiguration configuration, AbpTelemetryOpenTelemetryOptions openTelemetyOptions)
    {
        tracing.AddHttpClientInstrumentation(options =>
        {
            options.FilterHttpRequestMessage += (request) =>
            {
                if (request.RequestUri != null &&
                    openTelemetyOptions.IsIgnureRemoteRequestUrl(request.RequestUri.AbsolutePath))
                {
                    return false;
                }

                return true;
            };
        });
        tracing.AddAspNetCoreInstrumentation(options =>
        {
            options.Filter += (ctx) =>
            {
                if (openTelemetyOptions.IsIgnureLocalRequestUrl(ctx.Request.Path))
                {
                    return false;
                }

                return true;
            };
        });
        tracing.AddCapInstrumentation();
        tracing.AddEntityFrameworkCoreInstrumentation();

        if (configuration.GetValue("OpenTelemetry:Console:IsEnabled", false))
        {
            tracing.AddConsoleExporter();
        }

        if (configuration.GetValue("OpenTelemetry:Otlp:IsEnabled", false))
        {
            tracing.AddOtlpExporter(otlpOptions =>
            {
                var otlpEndPoint = configuration["OpenTelemetry:Otlp:Endpoint"];
                Check.NotNullOrWhiteSpace(otlpEndPoint, nameof(otlpEndPoint));

                otlpOptions.Headers = configuration["OpenTelemetry:Otlp:Headers"];
                otlpOptions.Endpoint = new Uri(otlpEndPoint.EnsureEndsWith('/') + "v1/traces");
                otlpOptions.Protocol = configuration.GetValue("OpenTelemetry:Otlp:Protocol", otlpOptions.Protocol);
            });
        }

        if (configuration.GetValue("OpenTelemetry:ZipKin:IsEnabled", false))
        {
            tracing.AddZipkinExporter(zipKinOptions =>
            {
                var zipkinEndPoint = configuration["OpenTelemetry:ZipKin:Endpoint"];
                Check.NotNullOrWhiteSpace(zipkinEndPoint, nameof(zipkinEndPoint));

                zipKinOptions.Endpoint = new Uri(zipkinEndPoint);
            });
        }
    }

    private static void ConfigureMetrics(MeterProviderBuilder metrics, IConfiguration configuration)
    {
        metrics.AddRuntimeInstrumentation();
        metrics.AddHttpClientInstrumentation();
        metrics.AddAspNetCoreInstrumentation();

        if (configuration.GetValue("OpenTelemetry:Otlp:IsEnabled", false))
        {
            metrics.AddOtlpExporter(otlpOptions =>
            {
                var otlpEndPoint = configuration["OpenTelemetry:Otlp:Endpoint"];
                Check.NotNullOrWhiteSpace(otlpEndPoint, nameof(otlpEndPoint));

                otlpOptions.Headers = configuration["OpenTelemetry:Otlp:Headers"];
                otlpOptions.Endpoint = new Uri(otlpEndPoint.EnsureEndsWith('/') + "v1/metrics");
                otlpOptions.Protocol = configuration.GetValue("OpenTelemetry:Otlp:Protocol", otlpOptions.Protocol);
            });
        }
    }

    private static void ConfigureLogging(LoggerProviderBuilder logging, IConfiguration configuration)
    {
        if (configuration.GetValue("OpenTelemetry:Otlp:IsEnabled", false))
        {
            logging.AddOtlpExporter(otlpOptions =>
            {
                var otlpEndPoint = configuration["OpenTelemetry:Otlp:Endpoint"];
                Check.NotNullOrWhiteSpace(otlpEndPoint, nameof(otlpEndPoint));

                otlpOptions.Headers = configuration["OpenTelemetry:Otlp:Headers"];
                otlpOptions.Endpoint = new Uri(otlpEndPoint.EnsureEndsWith('/') + "v1/logs");
                otlpOptions.Protocol = configuration.GetValue("OpenTelemetry:Otlp:Protocol", otlpOptions.Protocol);
            });
        }
    }
}
