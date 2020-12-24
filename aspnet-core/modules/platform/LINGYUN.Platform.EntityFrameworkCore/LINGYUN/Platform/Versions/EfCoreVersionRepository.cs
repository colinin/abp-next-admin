using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.Versions
{
    public class EfCoreVersionRepository : EfCoreRepository<IPlatformDbContext, AppVersion, Guid>, IVersionRepository, ITransientDependency
    {
        public EfCoreVersionRepository(
            IDbContextProvider<IPlatformDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<long> GetCountAsync(PlatformType platformType, string filter = "", CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(x => (platformType | x.PlatformType) == x.PlatformType)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Version.Contains(filter) || x.Title.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<AppVersion>> GetPagedListAsync(PlatformType platformType, string filter = "", string soring = nameof(AppVersion.CreationTime), bool includeDetails = true, int skipCount = 1, int maxResultCount = 10, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeIf(includeDetails, x => x.Files)
                .Where(x => (platformType | x.PlatformType) == x.PlatformType)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Version.Contains(filter) || x.Title.Contains(filter))
                .OrderBy($"{nameof(AppVersion.CreationTime)} DESC")
                .ThenBy(soring ?? nameof(AppVersion.Version))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }


        public virtual async Task<bool> ExistsAsync(PlatformType platformType, string version, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AnyAsync(x => (platformType | x.PlatformType) == x.PlatformType && x.Version.Equals(version), GetCancellationToken(cancellationToken));
        }

        public virtual async Task<AppVersion> GetByVersionAsync(PlatformType platformType, string version, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(x => x.Files)
                .Where(x => (platformType | x.PlatformType) == x.PlatformType && x.Version.Equals(version))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<AppVersion> GetLatestVersionAsync(PlatformType platformType, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(x => x.Files)
                .Where(x => (platformType | x.PlatformType) == x.PlatformType)
                .OrderBy($"{nameof(AppVersion.CreationTime)} DESC")
                .ThenBy(nameof(AppVersion.Version))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }
    }
}
