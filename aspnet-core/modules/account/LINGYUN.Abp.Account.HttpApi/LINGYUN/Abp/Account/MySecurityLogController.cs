using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account;

[Authorize]
[Area("account")]
[ControllerName("SecurityLog")]
[Route($"/api/{AccountRemoteServiceConsts.ModuleName}/security-logs")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
public class MySecurityLogController : AbpControllerBase, IMySecurityLogAppService
{
    private readonly IMySecurityLogAppService _service;
    public MySecurityLogController(IMySecurityLogAppService service)
    {
        _service = service;
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<SecurityLogDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetListInput input)
    {
        return _service.GetListAsync(input);
    }
}
