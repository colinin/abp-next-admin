using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

public class OpenObserveAuditLogManager_Tests : AuditLogManager_Tests<AbpAuditLoggingOpenObserveTestModule>
{
    private readonly IAuditLogManager _manager;

    public OpenObserveAuditLogManager_Tests()
    {
        _manager = GetRequiredService<IAuditLogManager>();
    }

    /// <summary>
    /// OpenObserve 不支持删除记录
    /// </summary>
    /// <remarks>
    /// 共享用例末尾会清理数据, 此处覆写并断言抛出的业务异常与错误码
    /// </remarks>
    protected override async Task DeleteAsync(Guid id)
    {
        var exception = await Should.ThrowAsync<BusinessException>(() => _manager.DeleteAsync(id));

        exception.Code.ShouldBe(AuditLoggingOpenObserveErrorCodes.DeleteNotSupported);
    }

    protected override async Task DeleteManyAsync(List<Guid> ids)
    {
        var exception = await Should.ThrowAsync<BusinessException>(() => _manager.DeleteManyAsync(ids));

        exception.Code.ShouldBe(AuditLoggingOpenObserveErrorCodes.DeleteNotSupported);
    }
}
