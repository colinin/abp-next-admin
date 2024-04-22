using LINGYUN.Abp.EntityChange.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.EntityChange;

public abstract class EntityChangeController : AbpControllerBase, IEntityChangeAppService
{
    protected IEntityChangeAppService EntityChangeAppService { get; }

    protected EntityChangeController(IEntityChangeAppService entityChangeAppService)
    {
        EntityChangeAppService = entityChangeAppService;

        LocalizationResource = typeof(AbpEntityChangeResource);
    }

    [HttpGet]
    [Route("entity-changes")]
    public virtual Task<PagedResultDto<EntityChangeDto>> GetListAsync(EntityChangeGetListInput input)
    {
        return EntityChangeAppService.GetListAsync(input);
    }
}
