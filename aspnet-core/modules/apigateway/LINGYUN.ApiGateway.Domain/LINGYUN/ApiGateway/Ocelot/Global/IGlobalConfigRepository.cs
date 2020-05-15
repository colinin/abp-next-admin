using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.ApiGateway.Ocelot
{
    public interface IGlobalConfigRepository : IBasicRepository<GlobalConfiguration, int>
    {
        Task<GlobalConfiguration> GetByAppIdAsync(string appId);
        Task<GlobalConfiguration> GetByItemIdAsync(long itemId);
        Task<(List<GlobalConfiguration> Globals, long TotalCount)> GetPagedListAsync(string filter = "", string sorting = "",
            int skipCount = 1, int maxResultCOunt = 10);
    }
}
