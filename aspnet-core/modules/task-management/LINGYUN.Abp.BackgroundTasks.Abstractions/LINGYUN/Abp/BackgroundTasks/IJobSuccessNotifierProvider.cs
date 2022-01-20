using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobSuccessNotifierProvider
{
    Task NotifySuccessAsync([NotNull] JobEventContext context);
}
