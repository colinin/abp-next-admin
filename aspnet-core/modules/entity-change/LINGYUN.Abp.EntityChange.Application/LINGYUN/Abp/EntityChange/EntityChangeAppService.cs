using LINGYUN.Abp.AuditLogging;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.EntityChange;

public abstract class EntityChangeAppService<TEntity> : ApplicationService, IEntityChangeAppService
    where TEntity : class
{
    protected virtual string GetListPolicy { get; set; }

    protected IEntityChangeStore EntityChangeStore { get; }

    protected EntityChangeAppService(IEntityChangeStore entityChangeStore)
    {
        EntityChangeStore = entityChangeStore;
    }

    public async virtual Task<PagedResultDto<EntityChangeDto>> GetListAsync(EntityChangeGetListInput input)
    {
        if (!GetListPolicy.IsNullOrWhiteSpace())
        {
            await AuthorizationService.AuthorizeAsync(GetListPolicy);
        }

        var entityTypeFullName = typeof(TEntity).FullName;

        var totalCount = await EntityChangeStore.GetCountAsync(
            input.AuditLogId, input.StartTime, input.EndTime,
            input.ChangeType, input.EntityId, entityTypeFullName);

        var entities = await EntityChangeStore.GetListAsync(
            input.Sorting, input.MaxResultCount, input.SkipCount,
            input.AuditLogId, input.StartTime, input.EndTime,
            input.ChangeType, input.EntityId, entityTypeFullName);

        return new PagedResultDto<EntityChangeDto>(totalCount,
            ObjectMapper.Map<List<AuditLogging.EntityChange>, List<EntityChangeDto>>(entities));
    }
}
