using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

[RemoteService(true, Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area("identity")]
[ControllerName("User")]
[Route("api/identity/users")]
[Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Default)]
public class IdentityUserController : AbpControllerBase, IIdentityUserAppService
{
    protected IIdentityUserAppService UserAppService { get; }
    public IdentityUserController(
        IIdentityUserAppService userAppService)
    {
        UserAppService = userAppService;
    }

    #region OrganizationUnit

    [HttpGet]
    [Route("{id}/organization-units")]
    public async virtual Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
    {
        return await UserAppService.GetOrganizationUnitsAsync(id);
    }

    [HttpPut]
    [Route("{id}/organization-units")]
    [Authorize(IdentityPermissions.Users.ManageOrganizationUnits)]
    public async virtual Task SetOrganizationUnitsAsync(Guid id, IdentityUserOrganizationUnitUpdateDto input)
    {
        await UserAppService.SetOrganizationUnitsAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}/organization-units/{ouId}")]
    [Authorize(IdentityPermissions.Users.ManageOrganizationUnits)]
    public async virtual Task RemoveOrganizationUnitsAsync(Guid id, Guid ouId)
    {
        await UserAppService.RemoveOrganizationUnitsAsync(id, ouId);
    }

    #endregion

    #region Claim

    [HttpGet]
    [Route("{id}/claims")]
    public async virtual Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
    {
        return await UserAppService.GetClaimsAsync(id);
    }

    [HttpPost]
    [Route("{id}/claims")]
    [Authorize(IdentityPermissions.Users.ManageClaims)]
    public async virtual Task AddClaimAsync(Guid id, IdentityUserClaimCreateDto input)
    {
        await UserAppService.AddClaimAsync(id, input);
    }

    [HttpPut]
    [Route("{id}/claims")]
    [Authorize(IdentityPermissions.Users.ManageClaims)]
    public async virtual Task UpdateClaimAsync(Guid id, IdentityUserClaimUpdateDto input)
    {
        await UserAppService.UpdateClaimAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}/claims")]
    [Authorize(IdentityPermissions.Users.ManageClaims)]
    public async virtual Task DeleteClaimAsync(Guid id, IdentityUserClaimDeleteDto input)
    {
        await UserAppService.DeleteClaimAsync(id, input);
    }

    #endregion

    [HttpPut]
    [Route("change-password")]
    [Authorize(IdentityPermissions.Users.ResetPassword)]
    public async virtual Task ChangePasswordAsync(Guid id, IdentityUserSetPasswordInput input)
    {
        await UserAppService.ChangePasswordAsync(id, input);
    }

    [HttpPut]
    [Route("change-two-factor")]
    [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Update)]
    public async virtual Task ChangeTwoFactorEnabledAsync(Guid id, TwoFactorEnabledDto input)
    {
        await UserAppService.ChangeTwoFactorEnabledAsync(id, input);
    }

    [HttpPut]
    [Route("{id}/lock/{seconds}")]
    [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Update)]
    public async virtual Task LockAsync(Guid id, int seconds)
    {
        await UserAppService.LockAsync(id, seconds);
    }

    [HttpPut]
    [Route("{id}/unlock")]
    [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Update)]
    public async virtual Task UnLockAsync(Guid id)
    {
        await UserAppService.UnLockAsync(id);
    }
}
