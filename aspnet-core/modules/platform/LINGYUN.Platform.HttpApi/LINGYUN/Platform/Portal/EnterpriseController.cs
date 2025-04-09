using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Portal;

[Area("platform")]
[Authorize(PlatformPermissions.Enterprise.Default)]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{PlatformRemoteServiceConsts.ModuleName}/enterprise")]
public class EnterpriseController : AbpControllerBase, IEnterpriseAppService
{
    private readonly IEnterpriseAppService _service;
    public EnterpriseController(IEnterpriseAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(PlatformPermissions.Enterprise.Create)]
    public virtual Task<EnterpriseDto> CreateAsync(EnterpriseCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(PlatformPermissions.Enterprise.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<EnterpriseDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<EnterpriseDto>> GetListAsync(EnterpriseGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(PlatformPermissions.Enterprise.Delete)]
    public virtual Task<EnterpriseDto> UpdateAsync(Guid id, EnterpriseUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}
