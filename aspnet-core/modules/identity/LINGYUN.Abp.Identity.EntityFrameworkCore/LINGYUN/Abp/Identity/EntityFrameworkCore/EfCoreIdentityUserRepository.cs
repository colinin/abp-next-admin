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
            return await (await GetDbSetAsync()).IncludeDetails(false)
                .AnyAsync(user => user.PhoneNumber == phoneNumber,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> IsPhoneNumberConfirmedAsync(
            string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).IncludeDetails(false)
                .AnyAsync(user => user.PhoneNumber == phoneNumber && user.PhoneNumberConfirmed,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> IsNormalizedEmailConfirmedAsync(
           string normalizedEmail,
           CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).IncludeDetails(false)
                .AnyAsync(user => user.NormalizedEmail == normalizedEmail && user.EmailConfirmed,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityUser> FindByPhoneNumberAsync(
            string phoneNumber,
            bool isConfirmed = true,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).IncludeDetails(includeDetails)
               .Where(user => user.PhoneNumber == phoneNumber && user.PhoneNumberConfirmed == isConfirmed)
               .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByIdListAsync(
            List<Guid> userIds,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            )
        {
            return await (await GetDbSetAsync()).IncludeDetails(includeDetails)
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
            var dbContext = await GetDbContextAsync();
            //var userUoDbSet = dbContext.Set<IdentityUserOrganizationUnit>();
            //var roleUoDbSet = dbContext.Set<OrganizationUnitRole>();
            //var userRoleDbSet = dbContext.Set<IdentityUserRole>();

            //var userUo = from usrUo in userUoDbSet
            //             join usr in dbContext.Users on usrUo.UserId equals usr.Id
            //             join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails)
            //                on usrUo.OrganizationUnitId equals ou.Id
            //             where usr.Id == id
            //             select ou;

            //var roleUo = from urol in userRoleDbSet
            //             join rol in dbContext.Roles on urol.RoleId equals rol.Id
            //             join rolUo in roleUoDbSet on rol.Id equals rolUo.RoleId
            //             join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails)
            //                on rolUo.OrganizationUnitId equals ou.Id
            //             where urol.UserId == id
            //             select ou;

            var query = from userOU in dbContext.Set<IdentityUserOrganizationUnit>()
                        join ro in dbContext.Set<IdentityUserRole>() on userOU.UserId equals ro.UserId
                        join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails)
                            on userOU.OrganizationUnitId equals ou.Id
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
            var dbContext = await GetDbContextAsync();
            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in (await GetDbSetAsync()) on userOu.UserId equals user.Id
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
            var dbContext = await GetDbContextAsync();
            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in (await GetDbSetAsync()) on userOu.UserId equals user.Id
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
            var dbContext = await GetDbContextAsync();
            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in (await GetDbSetAsync()) on userOu.UserId equals user.Id
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
            var dbContext = await GetDbContextAsync();
            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in (await GetDbSetAsync()) on userOu.UserId equals user.Id
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
            var dbContext = await GetDbContextAsync();
            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in (await GetDbSetAsync()) on userOu.UserId equals user.Id
                        join ou in dbContext.Set<OrganizationUnit>() on userOu.OrganizationUnitId equals ou.Id
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
            var dbContext = await GetDbContextAsync();
            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in (await GetDbSetAsync()) on userOu.UserId equals user.Id
                        join ou in dbContext.Set<OrganizationUnit>() on userOu.OrganizationUnitId equals ou.Id
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
