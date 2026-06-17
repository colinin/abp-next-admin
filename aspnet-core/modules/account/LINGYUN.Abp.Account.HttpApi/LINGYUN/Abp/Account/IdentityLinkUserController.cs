using LINGYUN.Abp.Account.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area("account")]
[Route("api/account/link-users")]
public class IdentityLinkUserController(IIdentityLinkUserAppService _service) : AbpControllerBase, IIdentityLinkUserAppService
{
    [HttpPost]
    [Route("generate-link-login-token")]
    public virtual Task<string> GenerateLinkLoginTokenAsync()
    {
        return _service.GenerateLinkLoginTokenAsync();
    }

    [HttpPost]
    [Route("generate-link-token")]
    public virtual Task<string> GenerateLinkTokenAsync()
    {
        return _service.GenerateLinkTokenAsync();
    }

    [HttpGet]
    public virtual Task<ListResultDto<LinkUserDto>> GetListAsync()
    {
        return _service.GetListAsync();
    }

    [HttpPost]
    [Route("link")]
    public virtual Task LinkAsync(LinkUserInput input)
    {
        return _service.LinkAsync(input);
    }

    [HttpPost]
    [Route("unlink")]
    public virtual Task UnlinkAsync(UnLinkUserInput input)
    {
        return _service.UnlinkAsync(input);
    }

    [HttpPost]
    [Route("verify-link-user")]
    public virtual Task<VerifyLinkUserDto> VerifyLinkUserAsync(VerifyLinkUserInput input)
    {
        return _service.VerifyLinkUserAsync(input);
    }

    [HttpPost]
    [Route("verify-link-login-token")]
    public virtual Task<bool> VerifyLinkLoginTokenAsync(VerifyLinkTokenInput input)
    {
        return _service.VerifyLinkLoginTokenAsync(input);
    }

    [HttpPost]
    [Route("verify-link-token")]
    public virtual Task<bool> VerifyLinkTokenAsync(VerifyLinkTokenInput input)
    {
        return _service.VerifyLinkTokenAsync(input);
    }
}
