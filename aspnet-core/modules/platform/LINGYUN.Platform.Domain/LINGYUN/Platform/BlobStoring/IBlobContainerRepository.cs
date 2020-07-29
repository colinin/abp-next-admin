using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.BlobStoring
{
    public interface IBlobContainerRepository : IBasicRepository<BlobContainer, Guid>
    {
        Task<bool> ContainerExistsAsync(string name, CancellationToken cancellationToken = default);
        
        Task<BlobContainer> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        
        Task<long> GetCountAsync(string filter = "", CancellationToken cancellationToken = default);

        Task<List<BlobContainer>> GetPagedListAsync(string filter = "", string sorting = nameof(BlobContainer.Name),
            int skipCount = 1, int maxResultCount = 10, CancellationToken cancellationToken = default);
    }
}
