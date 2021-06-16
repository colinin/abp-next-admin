using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundWorkers.Hangfire
{
    [Dependency(ReplaceServices = true)]
    public class HangfireBackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
    {
        protected IServiceProvider ServiceProvider { get; }

        public HangfireBackgroundWorkerManager(
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Add(IBackgroundWorker worker)
        {
            var timer = worker.GetType()
                .GetProperty("Timer", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(worker);

            if (timer == null)
            {
                return;
            }

            var period = timer.GetType()
                 .GetProperty("Period", BindingFlags.Public | BindingFlags.Instance)?
                 .GetValue(timer)?
                 .To<int>();

            if (!period.HasValue)
            {
                return;
            }

            var adapterType = typeof(HangfireBackgroundWorkerAdapter<>).MakeGenericType(worker.GetType());
            var workerAdapter = ServiceProvider.GetRequiredService(adapterType) as IHangfireBackgroundWorkerAdapter;

            RecurringJob.AddOrUpdate(
                recurringJobId: worker.GetType().FullName,
                methodCall: () => workerAdapter.ExecuteAsync(),
                cronExpression: CronGenerator.FormMilliseconds(period.Value));
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
