using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.Notifications;
using System.Threading.Tasks;

namespace LY.MicroService.RealtimeMessage.BackgroundJobs;

public class NotificationCleanupJob : IJobRunnable
{
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
