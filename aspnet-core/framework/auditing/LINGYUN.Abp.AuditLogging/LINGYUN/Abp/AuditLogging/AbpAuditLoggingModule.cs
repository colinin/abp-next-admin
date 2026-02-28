using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.AuditLogging;

[DependsOn(
    typeof(AbpAuditingModule),
    typeof(AbpGuidsModule),
    typeof(AbpExceptionHandlingModule))]
public class AbpAuditLoggingModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpAuditLoggingOptions>(configuration.GetSection("AuditLogging"));

        Configure<AbpAuditingOptions>(options =>
        {
            options.IgnoredTypes.AddIfNotContains(typeof(CancellationToken));
            options.IgnoredTypes.AddIfNotContains(typeof(CancellationTokenSource));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAuditLoggingOptions>>();

        if (options.Value.UseAuditLogQueue)
        {
            var auditLogQueue = rootServiceProvider.GetRequiredService<IAuditLogQueue>();
            if (auditLogQueue is AuditLogQueue queue1)
            {
                await queue1.StartAsync(_cancellationTokenSource.Token);
            }
        }

        if (options.Value.UseSecurityLogQueue)
        {
            var securityLogQueue = rootServiceProvider.GetRequiredService<ISecurityLogQueue>();
            if (securityLogQueue is SecurityLogQueue queue2)
            {
                await queue2.StartAsync(_cancellationTokenSource.Token);
            }
        }
    }

    public async override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAuditLoggingOptions>>();

        if (options.Value.UseAuditLogQueue)
        {
            var auditLogQueue = rootServiceProvider.GetRequiredService<IAuditLogQueue>();
            if (auditLogQueue is AuditLogQueue queue1)
            {
                await queue1.StopAsync(_cancellationTokenSource.Token);
            }
        }

        if (options.Value.UseSecurityLogQueue)
        {
            var securityLogQueue = rootServiceProvider.GetRequiredService<ISecurityLogQueue>();
            if (securityLogQueue is SecurityLogQueue queue2)
            {
                await queue2.StopAsync(_cancellationTokenSource.Token);
            }
        }

        _cancellationTokenSource.Cancel();
    }
}
