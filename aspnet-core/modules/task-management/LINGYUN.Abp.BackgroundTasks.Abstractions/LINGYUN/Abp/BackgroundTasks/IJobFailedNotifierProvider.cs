using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobFailedNotifierProvider
{
    Task NotifyErrorAsync([NotNull] JobEventContext context);
}
