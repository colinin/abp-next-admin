using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account;

[Authorize]
[Area("account")]
[ControllerName("Claim")]
[Route($"/api/{AccountRemoteServiceConsts.ModuleName}/my-claim")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
public class MyClaimController : AbpControllerBase, IMyClaimAppService
{
    private readonly IMyClaimAppService _service;

    public MyClaimController(
        IMyClaimAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("change-avatar")]
    [Obsolete("请使用 IMyProfileAppService.ChangePictureAsync")]
    public virtual Task ChangeAvatarAsync(ChangeAvatarInput input)
    {
        return _service.ChangeAvatarAsync(input);
    }

    [HttpGet]
    [Route("state/{claimType}")]
    public virtual Task<GetUserClaimStateDto> GetStateAsync(string claimType)
    {
        return _service.GetStateAsync(claimType);
    }

    [HttpDelete]
    [Route("reset/{claimType}")]
    public virtual Task ResetAsync(string claimType)
    {
        return _service.ResetAsync(claimType);
    }
}
