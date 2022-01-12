using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

public class JobExecutedEvent : JobEventBase<JobExecutedEvent>, ITransientDependency
{
    protected override async Task OnJobAfterExecutedAsync(JobEventContext context)
    {
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();

        var job = await store.FindAsync(context.EventData.Key);
        if (job != null)
        {
            job.TriggerCount += 1;
            job.LastRunTime = context.EventData.RunTime;
            job.NextRunTime = context.EventData.NextRunTime;
            job.Result = context.EventData.Result ?? "OK";
            job.Status = JobStatus.Running;

            // 一次性任务执行一次后标记为已完成
            if (job.JobType == JobType.Once)
            {
                job.Status = JobStatus.Completed;
            }

            // 任务异常后可重试
            if (context.EventData.Exception != null)
            {
                job.TryCount += 1;
                job.IsAbandoned = false;
                // 将任务标记为运行中, 会被轮询重新进入队列
                job.Status = JobStatus.FailedRetry;
                job.Result = GetExceptionMessage(context.EventData.Exception);

                // 多次异常后需要重新计算优先级
                if (job.TryCount <= (job.MaxTryCount / 2) &&
                    job.TryCount > (job.MaxTryCount / 3))
                {
                    job.Priority = JobPriority.BelowNormal;
                }
                else if (job.TryCount > (job.MaxTryCount / 1.5))
                {
                    job.Priority = JobPriority.Low;
                }

                if (job.TryCount >= job.MaxTryCount)
                {
                    job.Status = JobStatus.Stopped;
                    job.IsAbandoned = true;
                    job.NextRunTime = null;
                    await RemoveJobAsync(context, job);
                    // 重试达到上限发布异常通知
                    await NotifierAsync(context, job);
                }
            }
            else
            {
                // 所有任务达到上限则标记已完成
                if (job.MaxCount > 0 && job.TriggerCount >= job.MaxCount)
                {
                    job.Status = JobStatus.Completed;
                    job.NextRunTime = null;

                    await RemoveJobAsync(context, job);
                }
            }

            await store.StoreAsync(job);
        }
    }

    private async Task RemoveJobAsync(JobEventContext context, JobInfo jobInfo)
    {
        var jobScheduler = context.ServiceProvider.GetRequiredService<IJobScheduler>();
        await jobScheduler.RemoveAsync(jobInfo);
    }

    private async Task NotifierAsync(JobEventContext context, JobInfo jobInfo)
    {
        try
        {
            var notifier = context.ServiceProvider.GetRequiredService<IJobExceptionNotifier>();
            var exceptionContext = new JobExceptionNotificationContext(jobInfo, context.EventData.Exception);

            await notifier.NotifyAsync(exceptionContext);
        }
        catch (Exception ex)
        {
            Logger.LogWarning($"An exception thow with job exception notify: {ex.Message}");
        }
    }

    private string GetExceptionMessage(Exception exception)
    {
        if (exception.InnerException != null)
        {
            return exception.Message + "  =>   " + GetExceptionMessage(exception.InnerException);
        }

        return exception.Message;
    }
}
