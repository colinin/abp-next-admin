using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Activities;

public interface IJobExecutedProvider
{
    Task NotifyErrorAsync([NotNull] JobActionExecuteContext context);
    Task NotifySuccessAsync([NotNull] JobActionExecuteContext context);
    Task NotifyComplateAsync([NotNull] JobActionExecuteContext context);
}
