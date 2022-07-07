using LINGYUN.Abp.BackgroundTasks.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.TaskManagement;

[Dependency(ReplaceServices = true)]
public class JobActionStore : IJobActionStore, ITransientDependency
{
    private readonly IBackgroundJobActionRepository _backgroundJobActionRepository;

    public JobActionStore(
        IBackgroundJobActionRepository backgroundJobActionRepository)
    {
        _backgroundJobActionRepository = backgroundJobActionRepository;
    }

    public async virtual Task<List<JobAction>> GetActionsAsync(string id, CancellationToken cancellationToken = default)
    {
        var jobActions = await _backgroundJobActionRepository.GetListAsync(id, true, cancellationToken);

        return jobActions.Select(action => new JobAction
        {
            Name = action.Name,
            Paramters = action.Paramters
        }).ToList();
    }
}
