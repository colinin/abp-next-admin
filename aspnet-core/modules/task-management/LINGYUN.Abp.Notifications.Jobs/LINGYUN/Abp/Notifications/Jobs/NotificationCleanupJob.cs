using LINGYUN.Abp.BackgroundTasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Notifications.Jobs;

public class NotificationCleanupJob : IJobRunnable
{
    #region Definition Paramters

    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
        new List<JobDefinitionParamter>
        {
            new JobDefinitionParamter(PropertyBatchCount, LocalizableStatic.Create("Notification:BatchCount"))
        };

    #endregion
    public const string Name = "NotificationCleanupJob";
    /// <summary>
    /// 每次清除记录大小
    /// </summary>
    public const string PropertyBatchCount = "BatchCount";

    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        var count = context.GetJobData<int>(PropertyBatchCount);
        var clock = context.GetRequiredService<IClock>();
        var store = context.GetRequiredService<INotificationStore>();
        var currentTenant = context.GetRequiredService<ICurrentTenant>();

        await store.DeleteExpritionNotificationAsync(currentTenant.Id, count, clock.Now);
    }
}
