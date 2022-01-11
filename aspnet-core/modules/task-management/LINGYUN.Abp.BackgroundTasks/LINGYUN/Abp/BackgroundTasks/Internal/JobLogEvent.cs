using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.BackgroundTasks.Internal;

/// <summary>
/// 存储任务日志
/// </summary>
/// <remarks>
/// 任务类型标记了<see cref="DisableAuditingAttribute"/> 特性则不会记录日志
/// </remarks>
public class JobLogEvent : JobEventBase<JobLogEvent>, ITransientDependency
{
    protected async override Task OnJobAfterExecutedAsync(JobEventContext context)
    {
        if (context.EventData.Type.IsDefined(typeof(DisableAuditingAttribute), true))
        {
            return;
        }
        var store = context.ServiceProvider.GetRequiredService<IJobStore>();

        await store.StoreLogAsync(context.EventData);
    }
}
