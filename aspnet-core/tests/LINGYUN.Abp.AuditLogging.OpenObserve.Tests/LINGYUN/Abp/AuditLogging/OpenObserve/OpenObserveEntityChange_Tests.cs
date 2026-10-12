#nullable enable
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Xunit;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 实体变更往返测试
/// </summary>
/// <remarks>
/// OpenObserve 会把嵌套数组保存为 JSON 字符串列, 因此实体变更明细需要在写入前序列化、读取后反序列化
/// </remarks>
public class OpenObserveEntityChange_Tests : AbpAuditLoggingOpenObserveTestBase
{
    private readonly IAuditLogWriter _writer;
    private readonly IAuditLogManager _manager;
    private readonly IEntityChangeStore _entityChangeStore;

    public OpenObserveEntityChange_Tests()
    {
        _writer = GetRequiredService<IAuditLogWriter>();
        _manager = GetRequiredService<IAuditLogManager>();
        _entityChangeStore = GetRequiredService<IEntityChangeStore>();
    }

    [Fact]
    public async Task Should_Round_Trip_Entity_Changes()
    {
        var auditLogInfo = new AuditLogInfo
        {
            ExecutionTime = DateTime.Now,
            UserName = "_entity_user_1",
        };

        var entityChangeInfo = new EntityChangeInfo
        {
            ChangeType = EntityChangeType.Created,
            EntityId = "_entity_1",
            EntityTypeFullName = "_entity_type_1",
            ChangeTime = DateTime.Now,
            PropertyChanges = new List<EntityPropertyChangeInfo>
            {
                new()
                {
                    PropertyName = "Name",
                    OriginalValue = null,
                    NewValue = "_new_value",
                    PropertyTypeFullName = "System.String",
                },
            },
        };
        auditLogInfo.EntityChanges.Add(entityChangeInfo);

        var id = Guid.Parse(await _writer.WriteAsync(auditLogInfo));

        // OpenObserve 写入到可检索存在延迟, 轮询等待实体变更可见
        var auditLog = await WaitUntilAsync(
            () => _manager.GetAsync(id, includeDetails: true),
            log => log?.EntityChanges.Count > 0);

        auditLog.ShouldNotBeNull();
        auditLog!.EntityChanges.Count.ShouldBe(1);

        var change = auditLog.EntityChanges[0];
        change.EntityId.ShouldBe("_entity_1");
        change.EntityTypeFullName.ShouldBe("_entity_type_1");
        change.ChangeType.ShouldBe(EntityChangeType.Created);
        change.PropertyChanges.Count.ShouldBe(1);
        change.PropertyChanges[0].PropertyName.ShouldBe("Name");
        change.PropertyChanges[0].NewValue.ShouldBe("_new_value");

        // IEntityChangeStore 走独立数据流
        var storedChanges = await _entityChangeStore.GetListAsync(auditLogId: id);
        storedChanges.Count.ShouldBe(1);
        storedChanges[0].PropertyChanges.Count.ShouldBe(1);
    }

    /// <summary>
    /// 轮询直到满足条件(OpenObserve 写入到可检索存在延迟)
    /// </summary>
    private async Task<AuditLog?> WaitUntilAsync(
        Func<Task<AuditLog?>> query,
        Func<AuditLog?, bool> predicate,
        int timeoutSeconds = 60)
    {
        var deadline = DateTime.Now.AddSeconds(timeoutSeconds);
        AuditLog? result = null;

        while (DateTime.Now < deadline)
        {
            result = await query();
            if (predicate(result))
            {
                return result;
            }

            await Task.Delay(2000);
        }

        return result;
    }
}
