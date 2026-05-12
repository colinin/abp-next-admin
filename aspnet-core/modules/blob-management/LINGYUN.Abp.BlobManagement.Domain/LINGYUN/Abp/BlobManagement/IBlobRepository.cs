using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobRepository : IBasicRepository<Blob, Guid>
{
    Task<bool> ExistsAsync(
        string containerName,
        string fullName,
        CancellationToken cancellationToken = default);

    Task<Blob?> FindByNameAsync(
        Guid containerId,
        string name,
        Guid? parentId = null,
        CancellationToken cancellationToken = default);

    Task<Blob?> FindByFullNameAsync(
        string fullName,
        Guid? containerId = null,
        CancellationToken cancellationToken = default);

    Task<List<Blob>> GetFolderListAsync(
        IEnumerable<string> paths,
        Guid? containerId = null,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<Blob> specification,
        CancellationToken cancellationToken = default);

    Task<List<Blob>> GetListAsync(
        ISpecification<Blob> specification,
        string? sorting = nameof(Blob.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
