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

namespace LINGYUN.Abp.BlobManagement.EntityFrameworkCore;

public class EfCoreBlobRepository : EfCoreRepository<IBlobManagementDbContext, Blob, Guid>, IBlobRepository
{
    public EfCoreBlobRepository(
        IDbContextProvider<IBlobManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<bool> ExistsAsync(
        string containerName,
        string fullName,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        return await (from container in dbContext.Set<BlobContainer>()
                      join blob in dbContext.Set<Blob>()
                          on container.Id equals blob.ContainerId
                      where container.Name == containerName && blob.FullName == fullName
                      select blob)
                .AnyAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<Blob?> FindByNameAsync(
        Guid containerId,
        string name,
        Guid? parentId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.ContainerId == containerId && x.Name == name && x.ParentId == parentId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<Blob?> FindByFullNameAsync(
        string fullName,
        Guid? containerId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.FullName == fullName)
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Blob>> GetFolderListAsync(
        IEnumerable<string> paths,
        Guid? containerId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.Type == BlobType.Folder && paths.Contains(x.FullName))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .OrderByDescending(x => x.FullName.Length)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<Blob> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Blob>> GetListAsync(
        ISpecification<Blob> specification,
        string? sorting = nameof(Blob.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(Blob.Name))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
