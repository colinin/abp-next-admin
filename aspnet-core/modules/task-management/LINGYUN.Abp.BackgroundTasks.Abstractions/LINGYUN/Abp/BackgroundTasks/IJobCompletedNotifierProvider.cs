using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobCompletedNotifierProvider
{
    Task NotifyComplateAsync([NotNull] JobEventContext context);
}
