using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class EfCoreIdentityUserRepository : Volo.Abp.Identity.EntityFrameworkCore.EfCoreIdentityUserRepository, IIdentityUserRepository
    {
        public EfCoreIdentityUserRepository(
            IDbContextProvider<IIdentityDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> IsPhoneNumberUedAsync(
            string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(false)
                .AnyAsync(user => user.PhoneNumber == phoneNumber,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> IsPhoneNumberConfirmedAsync(
            string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(false)
                .AnyAsync(user => user.PhoneNumber == phoneNumber && user.PhoneNumberConfirmed,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> IsNormalizedEmailConfirmedAsync(
           string normalizedEmail,
           CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(false)
                .AnyAsync(user => user.NormalizedEmail == normalizedEmail && user.EmailConfirmed,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityUser> FindByPhoneNumberAsync(
            string phoneNumber,
            bool isConfirmed = true,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(includeDetails)
               .Where(user => user.PhoneNumber == phoneNumber && user.PhoneNumberConfirmed == isConfirmed)
               .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByIdListAsync(
            List<Guid> userIds,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            )
        {
            return await DbSet.IncludeDetails(includeDetails)
                .Where(user => userIds.Contains(user.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            Guid id,
            string filter = null,
            bool includeDetails = false,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        )
        {
            var query = from userOU in DbContext.Set<IdentityUserOrganizationUnit>()
                        join ou in DbContext.OrganizationUnits.IncludeDetails(includeDetails) on userOU.OrganizationUnitId equals ou.Id
                        where userOU.UserId == id
                        select ou;

            return await query
                .WhereIf(!filter.IsNullOrWhiteSpace(), ou => ou.Code.Contains(filter) || ou.DisplayName.Contains(filter))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetUsersInOrganizationUnitCountAsync(
            Guid organizationUnitId,
            string filter = null,
            CancellationToken cancellationToken = default
        )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        where userOu.OrganizationUnitId == organizationUnitId
                        select user;
            return await query
                .WhereIf(!filter.IsNullOrWhiteSpace(), 
                    user => user.Name.Contains(filter) || user.UserName.Contains(filter) ||
                        user.Surname.Contains(filter) || user.Email.Contains(filter) ||
                        user.PhoneNumber.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
            Guid organizationUnitId,
            string filter = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        where userOu.OrganizationUnitId == organizationUnitId
                        select user;
            return await query
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    user => user.Name.Contains(filter) || user.UserName.Contains(filter) ||
                        user.Surname.Contains(filter) || user.Email.Contains(filter) ||
                        user.PhoneNumber.Contains(filter))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetUsersInOrganizationsListCountAsync(
            List<Guid> organizationUnitIds,
            string filter = null,
            CancellationToken cancellationToken = default
        )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        where organizationUnitIds.Contains(userOu.OrganizationUnitId)
                        select user;
            return await query
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    user => user.Name.Contains(filter) || user.UserName.Contains(filter) ||
                        user.Surname.Contains(filter) || user.Email.Contains(filter) ||
                        user.PhoneNumber.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
            List<Guid> organizationUnitIds,
            string filter = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        where organizationUnitIds.Contains(userOu.OrganizationUnitId)
                        select user;
            return await query
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    user => user.Name.Contains(filter) || user.UserName.Contains(filter) ||
                        user.Surname.Contains(filter) || user.Email.Contains(filter) ||
                        user.PhoneNumber.Contains(filter))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetUsersInOrganizationUnitWithChildrenCountAsync(
            string code,
            string filter = null,
            CancellationToken cancellationToken = default
        )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        join ou in DbContext.Set<OrganizationUnit>() on userOu.OrganizationUnitId equals ou.Id
                        where ou.Code.StartsWith(code)
                        select user;
            return await query
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    user => user.Name.Contains(filter) || user.UserName.Contains(filter) ||
                        user.Surname.Contains(filter) || user.Email.Contains(filter) ||
                        user.PhoneNumber.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitWithChildrenAsync(
            string code,
            string filter = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        join ou in DbContext.Set<OrganizationUnit>() on userOu.OrganizationUnitId equals ou.Id
                        where ou.Code.StartsWith(code)
                        select user;
            return await query
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    user => user.Name.Contains(filter) || user.UserName.Contains(filter) ||
                        user.Surname.Contains(filter) || user.Email.Contains(filter) ||
                        user.PhoneNumber.Contains(filter))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
