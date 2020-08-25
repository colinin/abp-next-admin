using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    [Authorize(IdentityPermissions.Roles.ManageOrganizationUnits)]
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

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            var origanizationUnits = await IdentityRoleRepository.GetOrganizationUnitsAsync(id);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(origanizationUnits));
        }

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
    }
}
