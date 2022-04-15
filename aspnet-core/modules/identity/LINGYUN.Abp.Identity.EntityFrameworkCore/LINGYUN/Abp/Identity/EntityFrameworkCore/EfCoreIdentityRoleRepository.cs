using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace LINGYUN.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityRoleRepository : Volo.Abp.Identity.EntityFrameworkCore.EfCoreIdentityRoleRepository, IIdentityRoleRepository
    {
        public EfCoreIdentityRoleRepository(
            IDbContextProvider<IIdentityDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<IdentityRole>> GetListByIdListAsync(
            List<Guid> roleIds,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            )
        {
            return await (await GetDbSetAsync()).IncludeDetails(includeDetails)
                .Where(role => roleIds.Contains(role.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            Guid id, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = from roleOU in dbContext.Set<OrganizationUnitRole>()
                        join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails) on roleOU.OrganizationUnitId equals ou.Id
                        where roleOU.RoleId == id
                        select ou;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            IEnumerable<string> roleNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = from roleOU in dbContext.Set<OrganizationUnitRole>()
                        join role in dbContext.Roles on roleOU.RoleId equals role.Id
                        join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails) on roleOU.OrganizationUnitId equals ou.Id
                        where roleNames.Contains(role.Name)
                        select ou;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesInOrganizationsListAsync(
            List<Guid> organizationUnitIds, 
            CancellationToken cancellationToken = default)
        {
            var query = from roleOu in (await GetDbContextAsync()).Set<OrganizationUnitRole>()
                        join user in (await GetDbSetAsync()) on roleOu.RoleId equals user.Id
                        where organizationUnitIds.Contains(roleOu.OrganizationUnitId)
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesInOrganizationUnitAsync(
            Guid organizationUnitId, 
            CancellationToken cancellationToken = default)
        {
            var query = from roleOu in (await GetDbContextAsync()).Set<OrganizationUnitRole>()
                        join user in (await GetDbSetAsync()) on roleOu.RoleId equals user.Id
                        where roleOu.OrganizationUnitId == organizationUnitId
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesInOrganizationUnitWithChildrenAsync(
            string code, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = from roleOU in dbContext.Set<OrganizationUnitRole>()
                        join user in (await GetDbSetAsync()) on roleOU.RoleId equals user.Id
                        join ou in dbContext.Set<OrganizationUnit>() on roleOU.OrganizationUnitId equals ou.Id
                        where ou.Code.StartsWith(code)
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
