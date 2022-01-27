using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobEventTrigger
{
    Task OnJobBeforeExecuted(JobEventContext context);

    Task OnJobAfterExecuted(JobEventContext context);
}
