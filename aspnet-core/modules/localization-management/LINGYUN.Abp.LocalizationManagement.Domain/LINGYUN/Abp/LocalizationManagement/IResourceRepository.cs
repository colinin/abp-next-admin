using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.LocalizationManagement
{
    public interface IResourceRepository : IRepository<Resource, Guid>
    {
        Task<bool> ExistsAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<Resource> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);
    }
}
