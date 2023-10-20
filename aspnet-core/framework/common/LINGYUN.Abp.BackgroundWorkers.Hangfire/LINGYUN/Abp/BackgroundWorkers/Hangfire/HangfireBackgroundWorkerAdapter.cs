using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;

namespace LINGYUN.Abp.BackgroundWorkers.Hangfire
{
    public class HangfireBackgroundWorkerAdapter<TWorker> : BackgroundWorkerBase, IHangfireBackgroundWorkerAdapter
        where TWorker : IBackgroundWorker
    {
        private readonly MethodInfo _doWorkAsyncMethod;
        private readonly MethodInfo _doWorkMethod;

        public HangfireBackgroundWorkerAdapter()
        {
            _doWorkAsyncMethod = typeof(TWorker).GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            _doWorkMethod = typeof(TWorker).GetMethod("DoWork", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public async virtual Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var worker = (IBackgroundWorker)ServiceProvider.GetService(typeof(TWorker));
            var workerContext = new PeriodicBackgroundWorkerContext(ServiceProvider, cancellationToken);

            switch (worker)
            {
                case AsyncPeriodicBackgroundWorkerBase asyncWorker:
                    {
                        if (_doWorkAsyncMethod != null)
                        {
                            await (Task)_doWorkAsyncMethod.Invoke(asyncWorker, new object[] { workerContext });
                        }

                        break;
                    }
                case PeriodicBackgroundWorkerBase syncWorker:
                    {
                        if (_doWorkMethod != null)
                        {
                            _doWorkMethod.Invoke(syncWorker, new object[] { workerContext });
                        }

                        break;
                    }
            }
        }
    }
}
