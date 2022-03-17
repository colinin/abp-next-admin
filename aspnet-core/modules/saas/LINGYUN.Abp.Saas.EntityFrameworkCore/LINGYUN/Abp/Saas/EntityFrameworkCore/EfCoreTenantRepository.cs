using LINGYUN.Abp.Saas.Editions;
using LINGYUN.Abp.Saas.Tenants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Saas.EntityFrameworkCore;

public class EfCoreTenantRepository : EfCoreRepository<ISaasDbContext, Tenant, Guid>, ITenantRepository
{
    public EfCoreTenantRepository(IDbContextProvider<ISaasDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public async override Task<Tenant> FindAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var tenantDbSet = dbContext.Set<Tenant>()
                .IncludeDetails(includeDetails);

        if (includeDetails)
        {
            var editionDbSet = dbContext.Set<Edition>();
            var queryable = from tenant in tenantDbSet
                            join edition in editionDbSet on tenant.EditionId equals edition.Id into eg
                            from e in eg.DefaultIfEmpty()
                            where tenant.Id.Equals(id)
                            orderby tenant.Id
                            select new
                            {
                                Tenant = tenant,
                                Edition = e,
                            };
            var result = await queryable
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

            if (result != null && result.Tenant != null)
            {
                result.Tenant.Edition = result.Edition;
            }

            return result?.Tenant;
        }

        return await tenantDbSet
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync(t => t.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<Tenant> FindByNameAsync(
        string name,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var tenantDbSet = dbContext.Set<Tenant>()
                .IncludeDetails(includeDetails);

        if (includeDetails)
        {
            var editionDbSet = dbContext.Set<Edition>();
            var queryable = from tenant in tenantDbSet
                    join edition in editionDbSet on tenant.EditionId equals edition.Id into eg
                    from e in eg.DefaultIfEmpty()
                    where tenant.Name.Equals(name)
                    orderby tenant.Id
                    select new
                    {
                        Tenant = tenant,
                        Edition = e,
                    };
            var result = await queryable
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
            if (result != null && result.Tenant != null)
            {
                result.Tenant.Edition = result.Edition;
            }

            return result?.Tenant;
        }

        return await tenantDbSet
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync(t => t.Name == name, GetCancellationToken(cancellationToken));
    }

    [Obsolete("Use FindByNameAsync method.")]
    public virtual Tenant FindByName(string name, bool includeDetails = true)
    {
        var tenantDbSet = DbContext.Set<Tenant>()
                .IncludeDetails(includeDetails);

        if (includeDetails)
        {
            var editionDbSet = DbContext.Set<Edition>();
            var queryable = from tenant in tenantDbSet
                            join edition in editionDbSet on tenant.EditionId equals edition.Id into eg
                            from e in eg.DefaultIfEmpty()
                            where tenant.Name.Equals(name)
                            orderby tenant.Id
                            select new
                            {
                                Tenant = tenant,
                                Edition = e,
                            };
            var result = queryable
                .FirstOrDefault();
            if (result != null && result.Tenant != null)
            {
                result.Tenant.Edition = result.Edition;
            }

            return result?.Tenant;
        }

        return tenantDbSet
            .OrderBy(t => t.Id)
            .FirstOrDefault(t => t.Name == name);
    }

    [Obsolete("Use FindAsync method.")]
    public virtual Tenant FindById(Guid id, bool includeDetails = true)
    {
        var tenantDbSet = DbContext.Set<Tenant>()
                .IncludeDetails(includeDetails);

        if (includeDetails)
        {
            var editionDbSet = DbContext.Set<Edition>();
            var queryable = from tenant in tenantDbSet
                            join edition in editionDbSet on tenant.EditionId equals edition.Id into eg
                            from e in eg.DefaultIfEmpty()
                            where tenant.Id.Equals(id)
                            orderby tenant.Id
                            select new
                            {
                                Tenant = tenant,
                                Edition = e,
                            };
            var result = queryable
                .FirstOrDefault();
            if (result != null && result.Tenant != null)
            {
                result.Tenant.Edition = result.Edition;
            }

            return result?.Tenant;
        }

        return tenantDbSet
            .OrderBy(t => t.Id)
            .FirstOrDefault(t => t.Id == id);
    }

    public virtual async Task<List<Tenant>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        if (includeDetails)
        {
            var dbContext = await GetDbContextAsync();
            var editionDbSet = dbContext.Set<Edition>();
            var tenantDbSet = dbContext.Set<Tenant>()
                .IncludeDetails(includeDetails);

            var queryable = tenantDbSet
               .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.Name.Contains(filter))
               .OrderBy(sorting.IsNullOrEmpty() ? nameof(Tenant.Name) : sorting);

            var combinedResult = await queryable
                .Join(
                    editionDbSet,
                    o => o.EditionId,
                    i => i.Id,
                    (tenant, edition) => new { tenant, edition })
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));

            return combinedResult.Select(s =>
            {
                s.tenant.Edition = s.edition;
                return s.tenant;
            }).ToList();
        }

        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.Name.Contains(filter))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(Tenant.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter)
            ).CountAsync(cancellationToken: cancellationToken);
    }

    [Obsolete("Use WithDetailsAsync method.")]
    public override IQueryable<Tenant> WithDetails()
    {
        return GetQueryable().IncludeDetails();
    }

    public override async Task<IQueryable<Tenant>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
