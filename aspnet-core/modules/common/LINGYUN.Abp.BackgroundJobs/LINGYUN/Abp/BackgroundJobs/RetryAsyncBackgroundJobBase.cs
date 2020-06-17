using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;

namespace LINGYUN.Abp.BackgroundJobs
{
    public abstract class RetryAsyncBackgroundJobBase<TArgs> : IAsyncBackgroundJob<RetryAsyncBackgroundJobArgs<TArgs>>
    {
        public ILogger<RetryAsyncBackgroundJobBase<TArgs>> Logger { get; set; }

        protected IBackgroundJobManager BackgroundJobManager { get; }

        protected RetryAsyncBackgroundJobBase(
            IBackgroundJobManager backgroundJobManager)
        {
            BackgroundJobManager = backgroundJobManager;

            Logger = NullLogger<RetryAsyncBackgroundJobBase<TArgs>>.Instance;
        }

        public async Task ExecuteAsync(RetryAsyncBackgroundJobArgs<TArgs> args)
        {
            if (args.RetryCount > args.MaxRetryCount)
            {
                Logger.LogWarning("Job has failed and the maximum number of retries has been reached. The failure callback is about to enter");
                // 任务执行失败次数已达上限,调用用户定义回调,并不再执行
                await OnJobExecuteFailedAsync(args.JobArgs);
                return;
            }
            try
            {
                // 执行任务
                await ExecuteAsync(args.JobArgs, args.RetryCount);
                // 执行完成后回调
                await OnJobExecuteCompletedAsync(args.JobArgs);
            }
            catch(Exception ex)
            {
                Logger.LogWarning("Job execution has failed and a retry is imminent");
                Logger.LogWarning("Job running error:{0}", ex.Message);

                // 每次重试 间隔时间增加1.1倍
                var retryInterval = args.RetryIntervalMillisecond * 1.1;
                var retryJobArgs = new RetryAsyncBackgroundJobArgs<TArgs>(args.JobArgs,
                    args.RetryCount + 1, retryInterval, args.MaxRetryCount);

                Logger.LogDebug("Job task is queued for the next execution");

                // 计算优先级
                BackgroundJobPriority priority = BackgroundJobPriority.Normal;

                if (args.RetryCount <= (args.MaxRetryCount / 2) &&
                    args.RetryCount > (args.MaxRetryCount / 3))
                {
                    priority = BackgroundJobPriority.BelowNormal;
                }
                else if (args.RetryCount > (args.MaxRetryCount / 1.5))
                {
                    priority = BackgroundJobPriority.Low;
                }
                // 延迟入队,等待下一次运行
                await BackgroundJobManager.EnqueueAsync(retryJobArgs, priority, delay: TimeSpan.FromMilliseconds(retryInterval));
            }
        }

        protected abstract Task ExecuteAsync(TArgs args, int retryCount);

        protected virtual Task OnJobExecuteFailedAsync(TArgs args)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnJobExecuteCompletedAsync(TArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
