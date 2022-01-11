using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobExceptionNotifier
{
    Task NotifyAsync([NotNull] JobExceptionNotificationContext context);
}
