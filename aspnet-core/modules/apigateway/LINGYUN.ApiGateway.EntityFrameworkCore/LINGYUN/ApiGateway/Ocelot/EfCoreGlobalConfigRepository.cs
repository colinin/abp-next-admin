using LINGYUN.ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class EfCoreGlobalConfigRepository : ApiGatewayEfCoreRepositoryBase<ApiGatewayDbContext, GlobalConfiguration, int>, IGlobalConfigRepository
    {
        public EfCoreGlobalConfigRepository(IDbContextProvider<ApiGatewayDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {

        }

        public virtual async Task<GlobalConfiguration> GetByItemIdAsync(long itemId)
        {
            var globalConfiguration = await (await WithDetailsAsync()).Where(x => x.ItemId.Equals(itemId)).FirstOrDefaultAsync();
            if(globalConfiguration == null)
            {
                throw new EntityNotFoundException(typeof(GlobalConfiguration));
            }
            return globalConfiguration;
        }

        public virtual async Task<GlobalConfiguration> GetByAppIdAsync(string appId)
        {
            return await (await WithDetailsAsync())
                .Where(g => g.AppId.Equals(appId))
                .FirstOrDefaultAsync();
        }

        public virtual async Task<(List<GlobalConfiguration> Globals, long TotalCount)> GetPagedListAsync(string filter = "", string sorting = "",
            int skipCount = 1, int maxResultCount = 10)
        {
            var globals = await (await WithDetailsAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(), g => g.AppId.Contains(filter) ||
                          g.BaseUrl.Contains(filter) || g.DownstreamScheme.Contains(filter))
                .OrderBy(g => sorting ?? g.BaseUrl)
                .EfPageBy(skipCount, maxResultCount)
                .ToListAsync();
            var total = await (await GetQueryableAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(), g => g.AppId.Contains(filter) ||
                          g.BaseUrl.Contains(filter) || g.DownstreamScheme.Contains(filter))
                .LongCountAsync();

            return ValueTuple.Create(globals, total);
        }

        public override async Task<IQueryable<GlobalConfiguration>> WithDetailsAsync()
        {
            return await WithDetailsAsync(
                x => x.HttpHandlerOptions,
                x => x.LoadBalancerOptions,
                x => x.QoSOptions,
                x => x.RateLimitOptions,
                x => x.ServiceDiscoveryProvider);
        }

        [Obsolete("将在abp框架移除之后删除")]
        public override IQueryable<GlobalConfiguration> WithDetails()
        {
            return WithDetails(
                x => x.HttpHandlerOptions,
                x => x.LoadBalancerOptions,
                x => x.QoSOptions,
                x => x.RateLimitOptions,
                x => x.ServiceDiscoveryProvider);
        }
    }
}
