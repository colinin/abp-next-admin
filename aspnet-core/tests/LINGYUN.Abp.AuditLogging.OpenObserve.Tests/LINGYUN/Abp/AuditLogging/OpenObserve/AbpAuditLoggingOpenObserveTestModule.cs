using LINGYUN.Abp.OpenObserve;
using LINGYUN.Abp.OpenObserve.Models.Stream;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

[DependsOn(
    typeof(AbpAuditLoggingTestModule),
    typeof(AbpAuditLoggingOpenObserveModule))]
public class AbpAuditLoggingOpenObserveTestModule : AbpModule
{
    private const string UserSecretsId = "376A428E-4806-44BA-BF39-FA6F0E085982";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 集成测试按宿主实例使用独立数据流, 避免历史数据影响计数断言
        // (AbpIntegratedTest 每个测试方法会创建新的宿主实例, 仅用时间戳会因同秒碰撞而共用数据流)
        var suffix = $"{DateTime.Now:yyyyMMddHHmmss}{Guid.NewGuid():N}"[..22];

        Configure<AbpAuditLoggingOpenObserveOptions>(options =>
        {
            options.AuditLogStream = $"audit_log_test_{suffix}";
            options.SecurityLogStream = $"security_log_test_{suffix}";
            // 实体变更/操作数据流由 AuditLogStream 计算: {AuditLogStream}_entity_changes / _actions
        });

        // 收集日志以便断言(例如"数据流不存在"的警告)
        context.Services.AddLogging(builder => builder.AddProvider(new TestLoggerProvider()));
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        AsyncHelper.RunSync(async () => await OnApplicationShutdownAsync(context));
    }

    public async override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        await RemoveTestStreamsAsync(context.ServiceProvider);
    }

    private async static Task RemoveTestStreamsAsync(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<AbpAuditLoggingOpenObserveOptions>>().Value;
        var client = serviceProvider.GetRequiredService<OpenObserveClient>();

        var response = await client.GetStreamsAsync(
            options.Organization,
            fetchSchema: false,
            type: StreamType.Logs);

        // 数据流按租户隔离, 租户数据流以基础名称为前缀(如 audit_log_test_xxx_<tenantId>), 因此按前缀匹配清理
        var prefixes = new List<string>
        {
            options.AuditLogStream,
            options.SecurityLogStream,
        };
        if (options.WriteActions)
        {
            prefixes.Add(options.GetActionStream());
        }
        if (options.WriteEntityChanges)
        {
            prefixes.Add(options.GetEntityChangeStream());
        }

        var targets = response.List
            .Select(stream => stream.Name)
            .Where(name => prefixes.Any(prefix => name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var target in targets)
        {
            await client.DeleteStreamAsync(
                target,
                StreamType.Logs,
                true,
                options.Organization);
        }
    }
}
