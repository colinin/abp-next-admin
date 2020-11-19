using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Default)]
    public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
    {
        protected IdentityUserManager UserManager { get; }
        public IdentityUserAppService(
            IdentityUserManager userManager) 
        {
            UserManager = userManager;
        }

        #region OrganizationUnit


        [Authorize(IdentityPermissions.Users.ManageOrganizationUnits)]
        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            var user = await UserManager.GetByIdAsync(id);

            var origanizationUnits = await UserManager.GetOrganizationUnitsAsync(user);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(origanizationUnits));
        }

        [Authorize(IdentityPermissions.Users.ManageOrganizationUnits)]
        public virtual async Task SetOrganizationUnitsAsync(Guid id, IdentityUserOrganizationUnitUpdateDto input)
        {
            var user = await UserManager.GetByIdAsync(id);

            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnitIds);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.Users.ManageOrganizationUnits)]
        public virtual async Task RemoveOrganizationUnitsAsync(Guid id, Guid ouId)
        {
            await UserManager.RemoveFromOrganizationUnitAsync(id, ouId);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #endregion

        #region Claim

        public virtual async Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
        {
            var user = await UserManager.GetByIdAsync(id);

            return new ListResultDto<IdentityClaimDto>(ObjectMapper.Map<ICollection<IdentityUserClaim>, List<IdentityClaimDto>>(user.Claims));
        }

        [Authorize(IdentityPermissions.Users.ManageClaims)]
        public virtual async Task AddClaimAsync(Guid id, IdentityUserClaimCreateDto input)
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
        public virtual async Task UpdateClaimAsync(Guid id, IdentityUserClaimUpdateDto input)
        {
            var user = await UserManager.GetByIdAsync(id);
            var oldClaim = new Claim(input.ClaimType, input.ClaimValue);
            var newClaim = new Claim(input.ClaimType, input.NewClaimValue);
            user.ReplaceClaim(oldClaim, newClaim);
            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.Users.ManageClaims)]
        public virtual async Task DeleteClaimAsync(Guid id, IdentityUserClaimDeleteDto input)
        {
            var user = await UserManager.GetByIdAsync(id);
            user.RemoveClaim(new Claim(input.ClaimType, input.ClaimValue));
            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #endregion

        [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Update)]
        public virtual async Task ChangePasswordAsync(Guid id, ChangePasswordInput input)
        {
            var user = await UserManager.GetByIdAsync(id);

            if (user.IsExternal)
            {
                throw new BusinessException(code: Volo.Abp.Identity.IdentityErrorCodes.ExternalUserPasswordChange);
            }

            if (user.PasswordHash == null)
            {
                (await UserManager.AddPasswordAsync(user, input.NewPassword)).CheckErrors();

                return;
            }

            (await UserManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword)).CheckErrors();
        }

        [Authorize(Volo.Abp.Identity.IdentityPermissions.Users.Update)]
        public virtual async Task ChangeTwoFactorEnabledAsync(Guid id, ChangeTwoFactorEnabledDto input)
        {
            var user = await UserManager.GetByIdAsync(id);

            (await UserManager.SetTwoFactorEnabledWithAccountConfirmedAsync(user, input.Enabled)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
