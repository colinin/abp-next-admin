using Hangfire;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundJobs.Hangfire
{
    [Dependency(ReplaceServices = true)]
    public class HangfireBackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        public virtual Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            if (!delay.HasValue)
            {
                return Task.FromResult(
                    BackgroundJob.Enqueue<HangfireJobExecutionAdapter<TArgs>>(
                        adapter => adapter.ExecuteAsync(args)
                    )
                );
            }
            else
            {
                return Task.FromResult(
                    BackgroundJob.Schedule<HangfireJobExecutionAdapter<TArgs>>(
                        adapter => adapter.ExecuteAsync(args),
                        delay.Value
                    )
                );
            }
        }
    }
}