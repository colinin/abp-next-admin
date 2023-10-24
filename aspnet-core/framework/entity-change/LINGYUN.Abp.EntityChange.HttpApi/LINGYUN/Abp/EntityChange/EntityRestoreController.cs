using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINGYUN.Abp.EntityChange;

public abstract class EntityRestoreController : EntityChangeController, IEntityRestoreAppService
{
    protected IEntityRestoreAppService EntityRestoreAppService { get; }

    protected EntityRestoreController(IEntityRestoreAppService entityRestoreAppService)

        : base(entityRestoreAppService)
    {
        EntityRestoreAppService = entityRestoreAppService;
    }

    [HttpPut]
    [Route("entites-restore")]
    public virtual Task RestoreEntitesAsync(RestoreEntitiesInput input)
    {
        return EntityRestoreAppService.RestoreEntitesAsync(input);
    }

    [HttpPut]
    [Route("entity-restore")]
    [Route("{EntityId}/entity-restore")]
    [Route("{EntityId}/v/{EntityChangeId}/entity-restore")]
    public virtual Task RestoreEntityAsync(RestoreEntityInput input)
    {
        return EntityRestoreAppService.RestoreEntityAsync(input);
    }
}
