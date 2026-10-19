using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 多租户隔离测试
/// </summary>
/// <remarks>
/// 参考 Elasticsearch 实现: 不同租户使用不同数据流, 写入按日志自身租户, 查询按当前租户
/// </remarks>
public class OpenObserveMultiTenancy_Tests : AbpAuditLoggingOpenObserveTestBase
{
    private readonly IAuditLogWriter _writer;
    private readonly IAuditLogManager _manager;
    private readonly ICurrentTenant _currentTenant;

    public OpenObserveMultiTenancy_Tests()
    {
        _writer = GetRequiredService<IAuditLogWriter>();
        _manager = GetRequiredService<IAuditLogManager>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task Should_Isolate_Audit_Logs_By_Tenant()
    {
        var tenantId = Guid.NewGuid();

        var auditLogInfo = new AuditLogInfo
        {
            TenantId = tenantId,
            ExecutionTime = DateTime.Now,
            UserName = "_tenant_user",
        };

        var id = Guid.Parse(await _writer.WriteAsync(auditLogInfo));

        await Task.Delay(10000);

        // 宿主(未指定租户)看不到租户的数据流
        (await _manager.GetAsync(id)).ShouldBeNull();
        (await _manager.GetCountAsync()).ShouldBe(0);

        // 切换到租户后可见
        using (_currentTenant.Change(tenantId))
        {
            (await _manager.GetAsync(id)).ShouldNotBeNull();
            (await _manager.GetCountAsync()).ShouldBe(1);
        }
    }
}
