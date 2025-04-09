using LINGYUN.Abp.DataProtection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.DataProtectionManagement;

public class DataAccessStrategyStateSynchronizer : IDistributedEventHandler<DataAccessResourceChangeEvent>, ITransientDependency
{
    private readonly ISubjectStrategyRepository _strategyRepository;

    public DataAccessStrategyStateSynchronizer(ISubjectStrategyRepository strategyRepository)
    {
        _strategyRepository = strategyRepository;
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(DataAccessResourceChangeEvent eventData)
    {
        if (eventData.IsEnabled)
        {
            var subjectStrategy = await _strategyRepository.FindBySubjectAsync(
               eventData.Resource.SubjectName,
               eventData.Resource.SubjectId);
            if (subjectStrategy != null)
            {
                subjectStrategy.Strategy = DataAccessStrategy.Custom;

                await _strategyRepository.UpdateAsync(subjectStrategy);
            }
        }
    }
}
