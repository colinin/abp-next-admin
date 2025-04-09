using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Gdpr;

[Authorize]
[Controller]
[Area(GdprRemoteServiceConsts.ModuleName)]
[RemoteService(Name = GdprRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{GdprRemoteServiceConsts.ModuleName}/requests")]
public class GdprRequestController(
    IGdprRequestAppService service
) : AbpControllerBase,
    IGdprRequestAppService
{
    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return service.DeleteAsync(id);
    }

    [HttpDelete]
    [Route("personal-account")]
    public virtual Task DeletePersonalAccountAsync()
    {
        return service.DeletePersonalAccountAsync();
    }

    [HttpDelete]
    [Route("personal-data")]
    public virtual Task DeletePersonalDataAsync()
    {
        return service.DeletePersonalDataAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<GdprRequestDto> GetAsync(Guid id)
    {
        return service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<GdprRequestDto>> GetListAsync(GdprRequestGetListInput input)
    {
        return service.GetListAsync(input);
    }

    [HttpGet]
    [Route("personal-data/download/{requestId}")]
    public virtual Task<IRemoteStreamContent> DownloadPersonalDataAsync(Guid requestId)
    {
        return service.DownloadPersonalDataAsync(requestId);
    }

    [HttpPost]
    [Route("personal-data/prepare")]
    public virtual Task PreparePersonalDataAsync()
    {
        return service.PreparePersonalDataAsync();
    }
}
