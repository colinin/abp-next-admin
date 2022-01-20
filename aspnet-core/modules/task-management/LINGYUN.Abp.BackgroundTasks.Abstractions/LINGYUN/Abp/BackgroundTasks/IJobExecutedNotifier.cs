using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobExecutedNotifier
{
    Task NotifyErrorAsync([NotNull] JobEventContext context);
    Task NotifySuccessAsync([NotNull] JobEventContext context);
    Task NotifyComplateAsync([NotNull] JobEventContext context);
}
