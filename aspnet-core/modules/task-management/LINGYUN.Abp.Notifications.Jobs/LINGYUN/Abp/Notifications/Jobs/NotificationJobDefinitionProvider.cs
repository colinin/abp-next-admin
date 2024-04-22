using LINGYUN.Abp.BackgroundTasks;

namespace LINGYUN.Abp.Notifications.Jobs;

public class NotificationJobDefinitionProvider : JobDefinitionProvider
{
    public override void Define(IJobDefinitionContext context)
    {
        context.Add(
            new JobDefinition(
                NotificationCleanupJob.Name,
                typeof(NotificationCleanupJob),
                LocalizableStatic.Create("NotificationCleanupJob"),
                NotificationCleanupJob.Paramters));
    }
}
