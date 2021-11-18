using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Volo.Abp.Identity.IdentityPermissions.Roles.Default)]
    public class IdentityRoleAppService : IdentityAppServiceBase, IIdentityRoleAppService
    {
        protected IIdentityRoleRepository IdentityRoleRepository { get; }
        protected OrganizationUnitManager OrganizationUnitManager { get; }
        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
        public IdentityRoleAppService(
            IIdentityRoleRepository roleRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            OrganizationUnitManager = organizationUnitManager;
            IdentityRoleRepository = roleRepository;
        }

        #region OrganizationUnit

        [Authorize(IdentityPermissions.Roles.ManageOrganizationUnits)]
        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            var origanizationUnits = await IdentityRoleRepository.GetOrganizationUnitsAsync(id);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(origanizationUnits));
        }

        [Authorize(IdentityPermissions.Roles.ManageOrganizationUnits)]
        public virtual async Task SetOrganizationUnitsAsync(Guid id, IdentityRoleAddOrRemoveOrganizationUnitDto input)
        {
            var origanizationUnits = await IdentityRoleRepository.GetOrganizationUnitsAsync(id, true);

            var notInRoleOuIds = input.OrganizationUnitIds.Where(ouid => !origanizationUnits.Any(ou => ou.Id.Equals(ouid)));

            foreach (var ouId in notInRoleOuIds)
            {
                await OrganizationUnitManager.AddRoleToOrganizationUnitAsync(id, ouId);
            }

            var removeRoleOriganzationUnits = origanizationUnits.Where(ou => !input.OrganizationUnitIds.Contains(ou.Id));
            foreach (var origanzationUnit in removeRoleOriganzationUnits)
            {
                origanzationUnit.RemoveRole(id);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.Roles.ManageOrganizationUnits)]
        public virtual async Task RemoveOrganizationUnitsAsync(Guid id, Guid ouId)
        {
            await OrganizationUnitManager.RemoveRoleFromOrganizationUnitAsync(id, ouId);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #endregion

        #region ClaimType

        public virtual async Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
        {
            var role = await IdentityRoleRepository.GetAsync(id);

            return new ListResultDto<IdentityClaimDto>(ObjectMapper.Map<ICollection<IdentityRoleClaim>, List<IdentityClaimDto>>(role.Claims));
        }

        [Authorize(IdentityPermissions.Roles.ManageClaims)]
        public virtual async Task AddClaimAsync(Guid id, IdentityRoleClaimCreateDto input)
        {
            var role = await IdentityRoleRepository.GetAsync(id);
            var claim = new Claim(input.ClaimType, input.ClaimValue);
            if (role.FindClaim(claim) != null)
            {
                throw new UserFriendlyException(L["RoleClaimAlreadyExists"]);
            }

            role.AddClaim(GuidGenerator, claim);
            await IdentityRoleRepository.UpdateAsync(role);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.Roles.ManageClaims)]
        public virtual async Task UpdateClaimAsync(Guid id, IdentityRoleClaimUpdateDto input)
        {
            var role = await IdentityRoleRepository.GetAsync(id);
            var oldClaim = role.FindClaim(new Claim(input.ClaimType, input.ClaimValue));
            if (oldClaim != null)
            {
                role.RemoveClaim(oldClaim.ToClaim());
                role.AddClaim(GuidGenerator, new Claim(input.ClaimType, input.NewClaimValue));

                await IdentityRoleRepository.UpdateAsync(role);

                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        [Authorize(IdentityPermissions.Roles.ManageClaims)]
        public virtual async Task DeleteClaimAsync(Guid id, IdentityRoleClaimDeleteDto input)
        {
            var role = await IdentityRoleRepository.GetAsync(id);
            role.RemoveClaim(new Claim(input.ClaimType, input.ClaimValue));

            await IdentityRoleRepository.UpdateAsync(role);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #endregion
    }
}
