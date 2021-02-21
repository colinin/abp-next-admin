using LINGYUN.ApiGateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class EfCoreDynamicReRouteRepository : ApiGatewayEfCoreRepositoryBase<ApiGatewayDbContext, DynamicReRoute, int>, IDynamicReRouteRepository
    {
        public EfCoreDynamicReRouteRepository(IDbContextProvider<ApiGatewayDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {

        }

        public async Task<DynamicReRoute> GetByItemIdAsync(int itemId)
        {
            return await (await GetQueryableAsync()).Where(x => x.DynamicReRouteId.Equals(itemId)).FirstOrDefaultAsync();
        }

        public async Task<List<DynamicReRoute>> GetByAppIdAsync(string appId)
        {
            return await (await WithDetailsAsync()).Where(x => x.AppId.Equals(appId)).ToListAsync();
        }

        public override async Task<IQueryable<DynamicReRoute>> WithDetailsAsync()
        {
            return await WithDetailsAsync(x => x.RateLimitRule); ;
        }

        public override IQueryable<DynamicReRoute> WithDetails()
        {
            return WithDetails(x => x.RateLimitRule);
        }
    }
}
