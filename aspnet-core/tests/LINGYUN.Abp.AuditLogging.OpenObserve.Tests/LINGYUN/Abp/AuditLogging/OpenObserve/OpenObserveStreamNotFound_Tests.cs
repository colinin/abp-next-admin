using Microsoft.Extensions.Logging;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 数据流不存在时的处理
/// </summary>
/// <remarks>
/// OpenObserve 未写入过数据时数据流尚未创建, 查询按空结果处理并记录警告日志
/// </remarks>
public class OpenObserveStreamNotFound_Tests : AbpAuditLoggingOpenObserveTestBase
{
    private readonly IAuditLogManager _manager;
    private readonly ICurrentTenant _currentTenant;

    public OpenObserveStreamNotFound_Tests()
    {
        _manager = GetRequiredService<IAuditLogManager>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task Should_Log_Warning_When_Stream_Not_Found()
    {
        TestLoggerProvider.Clear();

        var tenantId = Guid.NewGuid();

        // 该租户从未写入过数据, 其数据流尚未创建
        using (_currentTenant.Change(tenantId))
        {
            (await _manager.GetCountAsync()).ShouldBe(0);
        }

        var warning = TestLoggerProvider.Entries
            .FirstOrDefault(entry => entry.Level == LogLevel.Warning &&
                                     entry.Message.Contains("does not exist yet", StringComparison.OrdinalIgnoreCase));

        warning.ShouldNotBeNull();
        // 日志类别为管理器类型(由子类注入的 ILogger<T> 决定)
        warning!.Category.ShouldContain("OpenObserveAuditLogManager");
    }
}
