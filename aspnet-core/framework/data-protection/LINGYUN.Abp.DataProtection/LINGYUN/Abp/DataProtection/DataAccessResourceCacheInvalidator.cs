using LINGYUN.Abp.DataProtection.Stores;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.DataProtection;

public class DataAccessResourceCacheInvalidator : IDistributedEventHandler<DataAccessResourceChangeEvent>, ITransientDependency
{
    private readonly IDataProtectedResourceStore _resourceStore;
    private readonly IDataProtectedStrategyStateStore _strategyStateStore;
    public DataAccessResourceCacheInvalidator(
        IDataProtectedResourceStore resourceStore, 
        IDataProtectedStrategyStateStore strategyStateStore)
    {
        _resourceStore = resourceStore;
        _strategyStateStore = strategyStateStore;
    }
    public async virtual Task HandleEventAsync(DataAccessResourceChangeEvent eventData)
    {
        if (eventData.IsEnabled)
        {
            await _resourceStore.SetAsync(eventData.Resource);

            // 角色权限策略变更为自定义规则
            await _strategyStateStore.SetAsync(
                new DataAccessStrategyState(
                    eventData.Resource.SubjectName,
                    new string[] { eventData.Resource.SubjectId },
                    DataAccessStrategy.Custom));
        }
        else
        {
            await _resourceStore.RemoveAsync(eventData.Resource);
        }
    }
}
