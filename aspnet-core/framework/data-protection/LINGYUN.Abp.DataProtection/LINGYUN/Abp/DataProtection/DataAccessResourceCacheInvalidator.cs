using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.DataProtection;

public class DataAccessResourceCacheInvalidator : IDistributedEventHandler<DataAccessResourceChangeEvent>, ITransientDependency
{
    private readonly IDataProtectedResourceStore _resourceStore;
    public DataAccessResourceCacheInvalidator(IDataProtectedResourceStore resourceStore)
    {
        _resourceStore = resourceStore;
    }
    public async virtual Task HandleEventAsync(DataAccessResourceChangeEvent eventData)
    {
        await _resourceStore.SetAsync(eventData.Resource);
    }
}
