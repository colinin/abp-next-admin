using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

[Dependency(ReplaceServices = true)]
public class EntityChangeStore : IEntityChangeStore, ITransientDependency
{
    protected IObjectMapper ObjectMapper { get; }
    protected IAuditLogRepository AuditLogRepository { get; }

    public ILogger<EntityChangeStore> Logger { protected get; set; }

    public EntityChangeStore(
        IObjectMapper objectMapper,
        IAuditLogRepository auditLogRepository)
    {
        ObjectMapper = objectMapper;
        AuditLogRepository = auditLogRepository;

        Logger = NullLogger<EntityChangeStore>.Instance;
    }


    public async virtual Task<EntityChange> GetAsync(
        Guid entityChangeId, 
        CancellationToken cancellationToken = default)
    {
        var entityChange = await AuditLogRepository.GetEntityChange(
            entityChangeId,
            cancellationToken);

        return ObjectMapper.Map<Volo.Abp.AuditLogging.EntityChange, EntityChange>(entityChange);
    }

    public async virtual Task<long> GetCountAsync(
        Guid? auditLogId = null, 
        DateTime? startTime = null,
        DateTime? endTime = null, 
        EntityChangeType? changeType = null, 
        string entityId = null,
        string entityTypeFullName = null, 
        CancellationToken cancellationToken = default)
    {
        return await AuditLogRepository.GetEntityChangeCountAsync(
            auditLogId,
            startTime,
            endTime,
            changeType,
            entityId,
            entityTypeFullName,
            cancellationToken);
    }

    public async virtual Task<List<EntityChange>> GetListAsync(
        string sorting = null,
        int maxResultCount = 50, 
        int skipCount = 0, 
        Guid? auditLogId = null,
        DateTime? startTime = null, 
        DateTime? endTime = null, 
        EntityChangeType? changeType = null, 
        string entityId = null,
        string entityTypeFullName = null, 
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var entityChanges = await AuditLogRepository.GetEntityChangeListAsync(
            sorting,
            maxResultCount,
            skipCount,
            auditLogId,
            startTime,
            endTime,
            changeType,
            entityId,
            entityTypeFullName,
            includeDetails,
            cancellationToken);

        return ObjectMapper.Map<List<Volo.Abp.AuditLogging.EntityChange>, List<EntityChange>>(entityChanges);
    }

    public async virtual Task<EntityChangeWithUsername> GetWithUsernameAsync(
        Guid entityChangeId, 
        CancellationToken cancellationToken = default)
    {
        var entityChangeWithUsername = await AuditLogRepository.GetEntityChangeWithUsernameAsync(
            entityChangeId,
            cancellationToken);

        return ObjectMapper.Map<Volo.Abp.AuditLogging.EntityChangeWithUsername, EntityChangeWithUsername>(entityChangeWithUsername);
    }

    public async virtual Task<List<EntityChangeWithUsername>> GetWithUsernameAsync(
        string entityId, 
        string entityTypeFullName,
        CancellationToken cancellationToken = default)
    {
        var entityChangesWithUsername = await AuditLogRepository.GetEntityChangesWithUsernameAsync(
          entityId,
          entityTypeFullName,
          cancellationToken);

        return ObjectMapper.Map<List<Volo.Abp.AuditLogging.EntityChangeWithUsername>, List<EntityChangeWithUsername>>(entityChangesWithUsername);
    }
}
