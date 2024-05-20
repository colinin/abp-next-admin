using LINGYUN.Abp.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;
public class EfCoreRoleEntityRuleRepository : EfCoreRepository<IAbpDataProtectionManagementDbContext, RoleEntityRule, Guid>,
    IRoleEntityRuleRepository
{
    public EfCoreRoleEntityRuleRepository(
        IDbContextProvider<IAbpDataProtectionManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<RoleEntityRule> FindEntityRuleAsync(
        string roleName, 
        string entityTypeFullName, 
        DataAccessOperation operation = DataAccessOperation.Read, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.RoleName == roleName && x.EntityTypeFullName == entityTypeFullName && x.Operation == operation)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<RoleEntityRule>> GetListByEntityAsync(
        string entityTypeFullName,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.EntityTypeFullName == entityTypeFullName)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<RoleEntityRule> specification, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<RoleEntityRule>> GetCountAsync(
    ISpecification<RoleEntityRule> specification, 
        string sorting = nameof(RoleEntityRule.EntityTypeFullName), 
        int maxResultCount = 10, 
        int skipCount = 0, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(RoleEntityRule.EntityTypeFullName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
