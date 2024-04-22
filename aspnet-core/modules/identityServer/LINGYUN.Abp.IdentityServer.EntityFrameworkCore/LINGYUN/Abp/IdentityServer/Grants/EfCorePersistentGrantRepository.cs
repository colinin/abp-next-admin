﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.IdentityServer.Grants;

namespace LINGYUN.Abp.IdentityServer.Grants
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(Volo.Abp.IdentityServer.Grants.IPersistentGrantRepository), 
        typeof(IPersistentGrantRepository),
        typeof(PersistentGrantRepository))]
    public class EfCorePersistentGrantRepository : PersistentGrantRepository, IPersistentGrantRepository
    {
        public EfCorePersistentGrantRepository(
            IDbContextProvider<IIdentityServerDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async virtual Task<long> GetCountAsync(string subjectId = null, string filter = null, CancellationToken cancellation = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!subjectId.IsNullOrWhiteSpace(), x => x.SubjectId.Equals(subjectId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                    x.Type.Contains(filter) || x.ClientId.Contains(filter) || x.Key.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellation));
        }

        public async virtual Task<List<PersistedGrant>> GetListAsync(string subjectId = null, string filter = null, string sorting = "CreationTime", int skipCount = 1, int maxResultCount = 10, CancellationToken cancellation = default)
        {
            if (sorting.IsNullOrWhiteSpace())
            {
                sorting = nameof(PersistedGrant.CreationTime);
            }
            return await (await GetDbSetAsync())
                .WhereIf(!subjectId.IsNullOrWhiteSpace(), x => x.SubjectId.Equals(subjectId))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                    x.Type.Contains(filter) || x.ClientId.Contains(filter) || x.Key.Contains(filter))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellation));
        }
    }
}
