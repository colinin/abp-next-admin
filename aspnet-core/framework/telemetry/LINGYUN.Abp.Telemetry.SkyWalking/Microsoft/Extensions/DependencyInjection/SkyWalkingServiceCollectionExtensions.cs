using LINGYUN.Abp.Telemetry.SkyWalking.Hosting;
using Microsoft.Extensions.Hosting;
using SkyApm;
using SkyApm.Config;
using SkyApm.Diagnostics;
using SkyApm.Diagnostics.EntityFrameworkCore;
using SkyApm.Diagnostics.Grpc;
using SkyApm.Diagnostics.Grpc.Net.Client;
using SkyApm.Diagnostics.HttpClient;
using SkyApm.Diagnostics.MSLogging;
using SkyApm.Diagnostics.SqlClient;
using SkyApm.Logging;
using SkyApm.PeerFormatters.MySqlConnector;
using SkyApm.PeerFormatters.SqlClient;
using SkyApm.Sampling;
using SkyApm.Service;
using SkyApm.Tracing;
using SkyApm.Transport;
using SkyApm.Transport.Grpc;
using SkyApm.Utilities.Configuration;
using SkyApm.Utilities.DependencyInjection;
using SkyApm.Utilities.Logging;
using System;
using Volo.Abp;

namespace Microsoft.Extensions.DependencyInjection;

internal static class SkyWalkingServiceCollectionExtensions
{
    public static IServiceCollection AddSkyWalking(this IServiceCollection services, Action<SkyApmExtensions> extensionsSetup = null)
    {
        Check.NotNull(extensionsSetup, nameof(extensionsSetup));

        services.AddSingleton<ISegmentDispatcher, AsyncQueueSegmentDispatcher>();
        services.AddSingleton<IExecutionService, RegisterService>();
        services.AddSingleton<IExecutionService, LogReportService>();
        services.AddSingleton<IExecutionService, PingService>();
        services.AddSingleton<IExecutionService, SegmentReportService>();
        services.AddSingleton<IExecutionService, CLRStatsService>();
        services.AddSingleton<IInstrumentStartup, InstrumentStartup>();
        services.AddSingleton(RuntimeEnvironment.Instance);
        services.AddSingleton<TracingDiagnosticProcessorObserver>();
        services.AddSingleton<IConfigAccessor, ConfigAccessor>();
        services.AddSingleton<IConfigurationFactory, ConfigurationFactory>();
        services.AddSingleton<IHostedService, InstrumentationHostedService>();
        services.AddSingleton<IEnvironmentProvider, HostingEnvironmentProvider>();
        services.AddSingleton<ISkyApmLogDispatcher, AsyncQueueSkyApmLogDispatcher>();
        services.AddSingleton<IPeerFormatter, PeerFormatter>();

        services.AddTracing()
            .AddSampling()
            .AddGrpcTransport()
            .AddSkyApmLogging();

        var skyWalking = services
            .AddSkyApmExtensions()
            .AddHttpClient()
            .AddGrpcClient()
            .AddSqlClient()
            .AddGrpc()
            .AddEntityFrameworkCore(delegate (DatabaseProviderBuilder c)
            {
                c.AddPomeloMysql().AddNpgsql().AddSqlite();
            })
            .AddMSLogging()
            .AddSqlClientPeerFormatter()
            .AddMySqlConnectorPeerFormatter();

        extensionsSetup?.Invoke(skyWalking);

        return services;
    }

    private static IServiceCollection AddTracing(this IServiceCollection services)
    {
        services.AddSingleton<ITracingContext, TracingContext>();
        services.AddSingleton<ICarrierPropagator, CarrierPropagator>();
        services.AddSingleton<ICarrierFormatter, Sw8CarrierFormatter>();
        services.AddSingleton<ISegmentContextFactory, SegmentContextFactory>();
        services.AddSingleton<IEntrySegmentContextAccessor, EntrySegmentContextAccessor>();
        services.AddSingleton<ILocalSegmentContextAccessor, LocalSegmentContextAccessor>();
        services.AddSingleton<IExitSegmentContextAccessor, ExitSegmentContextAccessor>();
        services.AddSingleton<ISegmentContextAccessor, SegmentContextAccessor>();
        services.AddSingleton<ISamplerChainBuilder, SamplerChainBuilder>();
        services.AddSingleton<IUniqueIdGenerator, UniqueIdGenerator>();
        services.AddSingleton<ISegmentContextMapper, SegmentContextMapper>();
        services.AddSingleton<IBase64Formatter, Base64Formatter>();
        return services;
    }

    private static IServiceCollection AddSampling(this IServiceCollection services)
    {
        services.AddSingleton<SimpleCountSamplingInterceptor>();
        services.AddSingleton((Func<IServiceProvider, ISamplingInterceptor>)((IServiceProvider p) => p.GetService<SimpleCountSamplingInterceptor>()));
        services.AddSingleton((Func<IServiceProvider, IExecutionService>)((IServiceProvider p) => p.GetService<SimpleCountSamplingInterceptor>()));
        services.AddSingleton<ISamplingInterceptor, RandomSamplingInterceptor>();
        services.AddSingleton<ISamplingInterceptor, IgnorePathSamplingInterceptor>();
        return services;
    }

    private static IServiceCollection AddGrpcTransport(this IServiceCollection services)
    {
        services.AddSingleton<ISegmentReporter, SegmentReporter>();
        services.AddSingleton<ILogReporter, LogReporter>();
        services.AddSingleton<ICLRStatsReporter, CLRStatsReporter>();
        services.AddSingleton<ConnectionManager>();
        services.AddSingleton<IPingCaller, PingCaller>();
        services.AddSingleton<IServiceRegister, ServiceRegister>();
        services.AddSingleton<IExecutionService, ConnectService>();
        return services;
    }

    private static IServiceCollection AddSkyApmLogging(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerFactory, DefaultLoggerFactory>();
        return services;
    }
}
