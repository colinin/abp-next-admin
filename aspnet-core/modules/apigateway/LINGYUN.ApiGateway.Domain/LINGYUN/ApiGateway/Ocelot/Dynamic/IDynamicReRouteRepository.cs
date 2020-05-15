using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IDynamicReRouteRepository : IBasicRepository<DynamicReRoute, int>
    {
        Task<DynamicReRoute> GetByItemIdAsync(int itemId);

        Task<List<DynamicReRoute>> GetByAppIdAsync(string appId);
    }
}
