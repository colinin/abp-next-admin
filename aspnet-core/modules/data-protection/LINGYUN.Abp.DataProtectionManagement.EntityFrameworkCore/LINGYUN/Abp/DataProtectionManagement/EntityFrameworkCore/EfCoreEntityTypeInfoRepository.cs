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
public class EfCoreEntityTypeInfoRepository : EfCoreRepository<IAbpDataProtectionManagementDbContext, EntityTypeInfo, Guid>, IEntityTypeInfoRepository
{
    public EfCoreEntityTypeInfoRepository(
        IDbContextProvider<IAbpDataProtectionManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<EntityTypeInfo> FindByTypeAsync(
        string typeFullName, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.TypeFullName == typeFullName)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<EntityTypeInfo> specification, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<EntityTypeInfo>> GetListAsync(
        ISpecification<EntityTypeInfo> specification,
        string sorting = nameof(EntityTypeInfo.Id),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(EntityTypeInfo.Id) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<IQueryable<EntityTypeInfo>> WithDetailsAsync()
    {
        return (await base.WithDetailsAsync()).Include(x => x.Properties);
    }
}
