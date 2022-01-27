using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobEventTrigger : IJobEventTrigger, ITransientDependency
{
    protected IJobEventProvider EventProvider { get; }

    public JobEventTrigger(IJobEventProvider jobEventProvider)
    {
        EventProvider = jobEventProvider;
    }

    public async virtual Task OnJobBeforeExecuted(JobEventContext context)
    {
        var jobEventList = EventProvider.GetAll();
        if (!jobEventList.Any())
        {
            return;
        }

        var index = 0;
        var taskList = new Task[jobEventList.Count];
        foreach (var jobEvent in jobEventList)
        {
            taskList[index] = jobEvent.OnJobBeforeExecuted(context);
            index++;
        }

        await Task.WhenAll(taskList);
    }

    public async virtual Task OnJobAfterExecuted(JobEventContext context)
    {
        var jobEventList = EventProvider.GetAll();
        if (!jobEventList.Any())
        {
            return;
        }

        var index = 0;
        var taskList = new Task[jobEventList.Count];
        foreach (var jobEvent in jobEventList)
        {
            taskList[index] = jobEvent.OnJobAfterExecuted(context);
            index++;
        }

        await Task.WhenAll(taskList);
    }
}
