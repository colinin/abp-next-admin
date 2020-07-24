using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Versions
{
    public interface IVersionRepository : IBasicRepository<AppVersion, Guid>
    {
        Task<bool> ExistsAsync(PlatformType platformType, string version, CancellationToken cancellationToken = default);

        Task<AppVersion> GetByVersionAsync(PlatformType platformType, string version, CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(PlatformType platformType, string filter = "", CancellationToken cancellationToken = default);

        Task<List<AppVersion>> GetPagedListAsync(PlatformType platformType, string filter = "", string soring = nameof(AppVersion.CreationTime), bool includeDetails = true, int skipCount = 1, int maxResultCount = 10, CancellationToken cancellationToken = default);

        Task<AppVersion> GetLatestVersionAsync(PlatformType platformType, CancellationToken cancellationToken = default);
    }
}
