using LINGYUN.Abp.BackgroundTasks;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        var store = context.GetRequiredService<INotificationStore>();

        await store.DeleteNotificationAsync(count);
    }
}
