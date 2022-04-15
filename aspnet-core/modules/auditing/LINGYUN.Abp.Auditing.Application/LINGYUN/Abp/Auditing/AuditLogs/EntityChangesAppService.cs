using LINGYUN.Abp.Auditing.Features;
using LINGYUN.Abp.Auditing.Permissions;
using LINGYUN.Abp.AuditLogging;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Auditing.AuditLogs;

[Authorize(AuditingPermissionNames.AuditLog.Default)]
[RequiresFeature(AuditingFeatureNames.Logging.AuditLog)]
public class EntityChangesAppService : AuditingApplicationServiceBase, IEntityChangesAppService
{
    protected IEntityChangeStore EntityChangeStore { get; }

    public EntityChangesAppService(
        IEntityChangeStore entityChangeStore)
    {
        EntityChangeStore = entityChangeStore;
    }

    public async virtual Task<EntityChangeDto> GetAsync(Guid id)
    {
        var entityChange = await EntityChangeStore.GetAsync(id);

        return ObjectMapper.Map<EntityChange, EntityChangeDto>(entityChange);
    }

    public async virtual Task<PagedResultDto<EntityChangeDto>> GetListAsync(EntityChangeGetByPagedDto input)
    {
        var totalCount = await EntityChangeStore.GetCountAsync(
            input.AuditLogId, input.StartTime, input.EndTime,
            input.ChangeType, input.EntityId, input.EntityTypeFullName);

        var entityChanges = await EntityChangeStore.GetListAsync(
            input.Sorting, input.MaxResultCount, input.SkipCount,
            input.AuditLogId, input.StartTime, input.EndTime,
            input.ChangeType, input.EntityId, input.EntityTypeFullName);

        return new PagedResultDto<EntityChangeDto>(totalCount,
            ObjectMapper.Map<List<EntityChange>, List<EntityChangeDto>>(entityChanges));
    }

    public async virtual Task<EntityChangeWithUsernameDto> GetWithUsernameAsync(Guid id)
    {
        var entityChangeWithUsername = await EntityChangeStore.GetWithUsernameAsync(id);

        return ObjectMapper.Map<EntityChangeWithUsername, EntityChangeWithUsernameDto>(entityChangeWithUsername);
    }

    public async virtual Task<ListResultDto<EntityChangeWithUsernameDto>> GetWithUsernameAsync(EntityChangeGetWithUsernameDto input)
    {
        var entityChangeWithUsernames = await EntityChangeStore.GetWithUsernameAsync(
            input.EntityId, input.EntityTypeFullName);

        return new ListResultDto<EntityChangeWithUsernameDto>(
            ObjectMapper.Map<List<EntityChangeWithUsername>, List<EntityChangeWithUsernameDto>>(entityChangeWithUsernames));
    }
}
