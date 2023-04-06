using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

[DisableAuditing]
[DisableJobAction]
public class BackgroundPollingJob : IJobRunnable
{
    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundTasksOptions>>().Value;
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();
        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();

        context.TryGetMultiTenantId(out var tenantId);

        using (currentTenant.Change(tenantId))
        {
            var waitingJobs = await store.GetWaitingListAsync(
                options.MaxJobFetchCount, context.CancellationToken);

            if (!waitingJobs.Any())
            {
                return;
            }

            /* changes: 2023-04-06
             * 作业轮询接口会轮询所有节点作业
             * 之前的设计为通过 IJobPublisher 发布到作业调度器
             * 而作业在运行中不是当前节点作业会被忽略, 造成作业实际上永远不会被执行
             * 
             * 改进方法为新建一个接口 IJobDispatcher, 轮询到的非当前节点作业通过它来调度
             * 而实现者为分布式事件作业调度器, 只有目标节点才会接收作业并启动
             * 
             */

            // 当前节点作业发布
            var scheduleJobs = waitingJobs.Where(job => string.Equals(job.NodeName, options.NodeName) || job.NodeName.IsNullOrWhiteSpace());
            var jobPublisher = context.ServiceProvider.GetRequiredService<IJobPublisher>();
            foreach (var job in scheduleJobs)
            {
                await jobPublisher.PublishAsync(job, context.CancellationToken);
            }

            // 非当前节点作业调度
            var dispatchJobs = waitingJobs.Where(job => !job.NodeName.IsNullOrWhiteSpace() && !string.Equals(job.NodeName, options.NodeName));
            if (dispatchJobs.Any())
            {
                var jobDispatcher = context.ServiceProvider.GetRequiredService<IJobDispatcher>();
                foreach (var jobByGroup in dispatchJobs.GroupBy(job => job.NodeName))
                {
                    await jobDispatcher.DispatchAsync(
                        jobByGroup,
                        jobByGroup.Key,
                        tenantId);
                }
            }
        }
    }
}
