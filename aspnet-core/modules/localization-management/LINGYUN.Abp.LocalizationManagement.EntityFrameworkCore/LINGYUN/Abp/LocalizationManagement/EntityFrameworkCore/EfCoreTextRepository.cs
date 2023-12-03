using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
    public class EfCoreTextRepository : EfCoreRepository<LocalizationDbContext, Text, int>,
        ITextRepository
    {
        public EfCoreTextRepository(
            IDbContextProvider<LocalizationDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async virtual Task<List<string>> GetExistsKeysAsync(
            string resourceName,
            string cultureName,
            IEnumerable<string> keys,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.ResourceName.Equals(resourceName) && x.CultureName.Equals(cultureName)
                    && keys.Contains(x.Key))
                .Select(x => x.Key)
                .Distinct()
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<Text> GetByCultureKeyAsync(
            string resourceName,
            string cultureName,
            string key,
            CancellationToken cancellationToken = default
            )
        {
            return await (await GetDbSetAsync())
                .Where(x => x.ResourceName.Equals(resourceName) && x.CultureName.Equals(cultureName) && x.Key.Equals(key))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<int> GetDifferenceCountAsync(
            string cultureName,
            string targetCultureName,
            string resourceName = null,
            bool? onlyNull = null,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await (await BuildTextDifferenceQueryAsync(
                    cultureName,
                    targetCultureName,
                    resourceName,
                    onlyNull,
                    filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<List<Text>> GetListAsync(
            string resourceName = null,
            string cultureName = null,
            CancellationToken cancellationToken = default)
        {
            //var languages = (await GetDbContextAsync()).Set<Language>();
            //var resources = (IQueryable<Resource>)(await GetDbContextAsync()).Set<Resource>();
            //if (!resourceName.IsNullOrWhiteSpace())
            //{
            //    resources = resources.Where(x => x.Name.Equals(resourceName));
            //}

            //var texts = await GetDbSetAsync();

            //return await (from txts in texts
            //       join r in resources
            //           on txts.ResourceName equals r.Name
            //       join lg in languages
            //           on txts.CultureName equals lg.CultureName
            //       where r.Enable && lg.Enable
            //       select txts)
            //     .ToListAsync(GetCancellationToken(cancellationToken));

            return await (await GetDbSetAsync())
                .WhereIf(!resourceName.IsNullOrWhiteSpace(), x => x.ResourceName.Equals(resourceName))
                .WhereIf(!cultureName.IsNullOrWhiteSpace(), x => x.CultureName.Equals(cultureName))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<List<TextDifference>> GetDifferencePagedListAsync(
            string cultureName,
            string targetCultureName,
            string resourceName = null,
            bool? onlyNull = null,
            string filter = null,
            string sorting = nameof(TextDifference.Key),
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            return await (await BuildTextDifferenceQueryAsync(
                    cultureName,
                    targetCultureName,
                    resourceName,
                    onlyNull,
                    filter,
                    sorting))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        protected async virtual Task<IQueryable<TextDifference>> BuildTextDifferenceQueryAsync(
            string cultureName,
            string targetCultureName,
            string resourceName = null,
            bool? onlyNull = null,
            string filter = null,
            string sorting = nameof(TextDifference.Key))
        {
            if (sorting.IsNullOrWhiteSpace())
            {
                sorting = nameof(TextDifference.Key);
            }

            var textQuery = (await GetDbSetAsync())
                .Where(x => x.CultureName.Equals(cultureName))
                .WhereIf(!resourceName.IsNullOrWhiteSpace(), x => x.ResourceName.Equals(resourceName))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Key.Contains(filter))
                .OrderBy(sorting);

            var targetTextQuery = (await GetDbSetAsync())
                .Where(x => x.CultureName.Equals(targetCultureName))
                .WhereIf(!resourceName.IsNullOrWhiteSpace(), x => x.ResourceName.Equals(resourceName));

            var query = from crtText in textQuery
                         join tgtText in targetTextQuery
                             on crtText.Key equals tgtText.Key
                             into tgt
                         from tt in tgt.DefaultIfEmpty()
                        where onlyNull.HasValue && onlyNull.Value 
                            ? tt.Value == null
                            : 1 == 1
                        select new TextDifference(
                             crtText.Id,
                             crtText.CultureName,
                             crtText.Key,
                             crtText.Value,
                             targetCultureName,
                             tt != null ? tt.Value : null,
                             crtText.ResourceName);
            return query;
        }
    }
}
