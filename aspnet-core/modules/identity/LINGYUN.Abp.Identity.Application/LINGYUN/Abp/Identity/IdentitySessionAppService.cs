using LINGYUN.Abp.Identity.Session;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

[Authorize(IdentityPermissions.IdentitySession.Default)]
public class IdentitySessionAppService : IdentityAppServiceBase, IIdentitySessionAppService
{
    private readonly IIdentitySessionManager _identitySessionManager;
    private readonly IIdentitySessionRepository _identitySessionRepository;

    public IdentitySessionAppService(
        IIdentitySessionManager identitySessionManager, 
        IIdentitySessionRepository identitySessionRepository)
    {
        _identitySessionManager = identitySessionManager;
        _identitySessionRepository = identitySessionRepository;
    }

    public async virtual Task<PagedResultDto<IdentitySessionDto>> GetSessionsAsync(GetUserSessionsInput input)
    {
        var totalCount = await _identitySessionRepository.GetCountAsync(
            input.UserId, input.Device, input.ClientId);
        var identitySessions = await _identitySessionRepository.GetListAsync(
            input.Sorting, input.MaxResultCount, input.SkipCount,
            input.UserId, input.Device, input.ClientId);

        return new PagedResultDto<IdentitySessionDto>(totalCount,
            ObjectMapper.Map<List<IdentitySession>, List<IdentitySessionDto>>(identitySessions));
    }

    [Authorize(IdentityPermissions.IdentitySession.Revoke)]
    public async virtual Task RevokeSessionAsync(string sessionId)
    {
        await _identitySessionManager.RevokeSessionAsync(sessionId);
    }
}
