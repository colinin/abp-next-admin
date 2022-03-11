using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Saas.Editions;

[Controller]
[Authorize(AbpSaasPermissions.Editions.Default)]
[RemoteService(Name = AbpSaasRemoteServiceConsts.RemoteServiceName)]
[Area(AbpSaasRemoteServiceConsts.ModuleName)]
[Route("api/saas/editions")]
public class EditionController : AbpSaasControllerBase, IEditionAppService
{
    protected IEditionAppService EditionAppService { get; }

    public EditionController(IEditionAppService editionAppService)
    {
        EditionAppService = editionAppService;
    }

    [HttpPost]
    [Authorize(AbpSaasPermissions.Editions.Create)]
    public virtual Task<EditionDto> CreateAsync(EditionCreateDto input)
    {
        return EditionAppService.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(AbpSaasPermissions.Editions.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return EditionAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<EditionDto> GetAsync(Guid id)
    {
        return EditionAppService.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<EditionDto>> GetListAsync(EditionGetListInput input)
    {
        return EditionAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(AbpSaasPermissions.Editions.Update)]
    public virtual Task<EditionDto> UpdateAsync(Guid id, EditionUpdateDto input)
    {
        return EditionAppService.UpdateAsync(id, input);
    }
}
