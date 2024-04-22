using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;

namespace LINGYUN.Abp.BackgroundWorkers.Hangfire
{
    public interface IHangfireBackgroundWorkerAdapter : IBackgroundWorker
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
