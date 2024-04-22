using Hangfire;
using JetBrains.Annotations;
using LINGYUN.Abp.BackgroundJobs.Hangfire;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs
{
    public static class IBackgroundJobManagerExtensions
    {
        /// <summary>
        /// 后台作业进入周期性队列
        /// </summary>
        /// <typeparam name="TArgs">作业参数类型</typeparam>
        /// <param name="backgroundJobManager">后台作业管理器</param>
        /// <param name="cron">Cron表达式</param>
        /// <param name="args">作业参数</param>
        /// <returns></returns>
        public static Task EnqueueAsync<TArgs>(
            this IBackgroundJobManager backgroundJobManager,
            [NotNull] string cron,
            TArgs args
        )
        {
            Check.NotNullOrWhiteSpace(cron, nameof(cron));
            Check.NotNull(args, nameof(args));

            var jobName = BackgroundJobNameAttribute.GetName<TArgs>();

            RecurringJob.AddOrUpdate<HangfireJobExecutionAdapter<TArgs>>(jobName, adapter => adapter.ExecuteAsync(args), cron);

            return Task.CompletedTask;
        }
    }
}
