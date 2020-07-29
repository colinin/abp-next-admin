using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.BlobStoring
{
    public interface IBlobRepository : IBasicRepository<Blob, Guid>
    {
        Task<bool> BlobExistsAsync(string sha256, CancellationToken cancellationToken = default);

        Task<Blob> GetBySha256Async(string sha256, CancellationToken cancellationToken = default);

        Task CopyToAsync(Blob blob, Guid copyToContainerId, CancellationToken cancellationToken = default);

        Task MoveToAsync(Blob blob, Guid moveToContainerId, CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(Guid containerId, string filter = "", CancellationToken cancellationToken = default);

        Task<List<Blob>> GetPagedListAsync(Guid containerId, string filter = "", string sorting = nameof(Blob.Name), 
            int skipCount = 1, int maxResultCount = 10, CancellationToken cancellationToken = default);

    }
}
