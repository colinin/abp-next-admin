using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

[Area("identity")]
[ControllerName("identity-sessions")]
[Route("api/identity/sessions")]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Authorize(IdentityPermissions.IdentitySession.Default)]
public class IdentitySessionController : AbpControllerBase, IIdentitySessionAppService
{
    private readonly IIdentitySessionAppService _servcie;

    public IdentitySessionController(IIdentitySessionAppService servcie)
    {
        _servcie = servcie;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentitySessionDto>> GetSessionsAsync(GetUserSessionsInput input)
    {
        return _servcie.GetSessionsAsync(input);
    }

    [Authorize(IdentityPermissions.IdentitySession.Revoke)]
    [HttpDelete]
    [Route("{sessionId}/revoke")]
    public virtual Task RevokeSessionAsync(string sessionId)
    {
        return _servcie.RevokeSessionAsync(sessionId);
    }
}
