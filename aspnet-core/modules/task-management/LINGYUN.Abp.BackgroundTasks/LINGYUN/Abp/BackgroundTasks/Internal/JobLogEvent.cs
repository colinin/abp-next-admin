using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

/// <summary>
/// 存储任务日志
/// </summary>
/// <remarks>
/// 任务类型标记了<see cref="DisableAuditingAttribute"/> 特性则不会记录日志
/// </remarks>
public class JobLogEvent : JobEventBase<JobLogEvent>, ITransientDependency
{
    protected override Task<bool> CanAfterExecuted(JobEventContext context)
    {
        if (context.EventData.Type.IsDefined(typeof(DisableAuditingAttribute), true))
        {
            Logger.LogWarning("The job change event could not be processed because the job marked the DisableAuditing attribute!");
            return Task.FromResult(false);
        }
        return base.CanAfterExecuted(context);
    }

    protected async override Task OnJobAfterExecutedAsync(JobEventContext context)
    {
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();
        await store.StoreLogAsync(context.EventData);
    }
}
