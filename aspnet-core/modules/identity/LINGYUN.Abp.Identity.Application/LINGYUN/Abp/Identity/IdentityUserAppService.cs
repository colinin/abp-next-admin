using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    [Authorize(IdentityPermissions.Users.ManageOrganizationUnits)]
    public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
    {
        protected IdentityUserManager UserManager { get; }
        public IdentityUserAppService(
            IdentityUserManager userManager) 
        {
            UserManager = userManager;
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            var user = await UserManager.GetByIdAsync(id);

            var origanizationUnits =  await UserManager.GetOrganizationUnitsAsync(user);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(origanizationUnits));
        }

        public virtual async Task UpdateOrganizationUnitsAsync(Guid id, IdentityUserOrganizationUnitUpdateDto input)
        {
            var user = await UserManager.GetByIdAsync(id);

            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnitIds);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
