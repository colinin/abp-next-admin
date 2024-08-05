using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

[Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Default)]
public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
{
    protected IdentityUserManager UserManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    public IdentityUserAppService(
        IdentityUserManager userManager,
        IOptions<IdentityOptions> identityOptions) 
    {
        UserManager = userManager;
        IdentityOptions = identityOptions;
    }

    #region OrganizationUnit

    public async virtual Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
    {
        var user = await UserManager.GetByIdAsync(id);

        var origanizationUnits = await UserManager.GetOrganizationUnitsAsync(user);

        return new ListResultDto<OrganizationUnitDto>(
            ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(origanizationUnits));
    }

    [Authorize(IdentityPermissions.Users.ManageOrganizationUnits)]
    public async virtual Task SetOrganizationUnitsAsync(Guid id, IdentityUserOrganizationUnitUpdateDto input)
    {
        var user = await UserManager.GetByIdAsync(id);

        await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnitIds);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(IdentityPermissions.Users.ManageOrganizationUnits)]
    public async virtual Task RemoveOrganizationUnitsAsync(Guid id, Guid ouId)
    {
        await UserManager.RemoveFromOrganizationUnitAsync(id, ouId);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    #endregion

    #region Claim

    public async virtual Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
    {
        var user = await UserManager.GetByIdAsync(id);

        return new ListResultDto<IdentityClaimDto>(ObjectMapper.Map<ICollection<IdentityUserClaim>, List<IdentityClaimDto>>(user.Claims));
    }

    [Authorize(IdentityPermissions.Users.ManageClaims)]
    public async virtual Task AddClaimAsync(Guid id, IdentityUserClaimCreateDto input)
    {
        var user = await UserManager.GetByIdAsync(id);
        var claim = new Claim(input.ClaimType, input.ClaimValue);
        if (user.FindClaim(claim) != null)
        {
            throw new UserFriendlyException(L["UserClaimAlreadyExists"]);
        }
        user.AddClaim(GuidGenerator, claim);
        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(IdentityPermissions.Users.ManageClaims)]
    public async virtual Task UpdateClaimAsync(Guid id, IdentityUserClaimUpdateDto input)
    {
        var user = await UserManager.GetByIdAsync(id);
        var oldClaim = new Claim(input.ClaimType, input.ClaimValue);
        var newClaim = new Claim(input.ClaimType, input.NewClaimValue);
        user.ReplaceClaim(oldClaim, newClaim);
        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(IdentityPermissions.Users.ManageClaims)]
    public async virtual Task DeleteClaimAsync(Guid id, IdentityUserClaimDeleteDto input)
    {
        var user = await UserManager.GetByIdAsync(id);
        user.RemoveClaim(new Claim(input.ClaimType, input.ClaimValue));
        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    #endregion

    [Authorize(IdentityPermissions.Users.ResetPassword)]
    public async virtual Task ChangePasswordAsync(Guid id, IdentityUserSetPasswordInput input)
    {
        var user = await GetUserAsync(id);

        if (user.IsExternal)
        {
            throw new BusinessException(code: Volo.Abp.Identity.IdentityErrorCodes.ExternalUserPasswordChange);
        }

        if (user.PasswordHash == null)
        {
            (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();
        }
        else
        {
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);

            (await UserManager.ResetPasswordAsync(user, token, input.Password)).CheckErrors();
        }

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Update)]
    public async virtual Task ChangeTwoFactorEnabledAsync(Guid id, TwoFactorEnabledDto input)
    {
        var user = await GetUserAsync(id);

        (await UserManager.SetTwoFactorEnabledWithAccountConfirmedAsync(user, input.Enabled)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Update)]
    public async virtual Task LockAsync(Guid id, int seconds)
    {
        var user = await GetUserAsync(id);
        //if (!UserManager.SupportsUserLockout)
        //{
        //    throw new UserFriendlyException(L["Volo.Abp.Identity:UserLockoutNotEnabled"]);
        //}
        var endDate = new DateTimeOffset(Clock.Now).AddSeconds(seconds);
        (await UserManager.SetLockoutEndDateAsync(user, endDate)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Update)]
    public async virtual Task UnLockAsync(Guid id)
    {
        var user = await GetUserAsync(id);
        (await UserManager.SetLockoutEndDateAsync(user, null)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    protected async virtual Task<IdentityUser> GetUserAsync(Guid id)
    {
        await IdentityOptions.SetAsync();
        var user = await UserManager.GetByIdAsync(id);

        return user;
    }
}
