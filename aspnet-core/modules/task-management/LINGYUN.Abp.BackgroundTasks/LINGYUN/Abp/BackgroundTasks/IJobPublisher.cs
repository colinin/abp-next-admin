using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobPublisher
{
    Task<bool> PublishAsync(JobInfo job, CancellationToken cancellationToken = default);
}
