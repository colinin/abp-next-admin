using LINGYUN.Abp.BackgroundTasks;

namespace LINGYUN.Abp.Identity.Jobs;
public class IdentityJobDefinitionProvider : JobDefinitionProvider
{
    public override void Define(IJobDefinitionContext context)
    {
        context.Add(
            new JobDefinition(
                InactiveIdentitySessionCleanupJob.Name,
                typeof(InactiveIdentitySessionCleanupJob),
                LocalizableStatic.Create("InactiveIdentitySessionCleanupJob"),
                InactiveIdentitySessionCleanupJob.Paramters),
            new JobDefinition(
                InactiveIdentityUserNotifierJob.Name,
                typeof(InactiveIdentityUserNotifierJob),
                LocalizableStatic.Create("InactiveIdentityUserNotifierJob"),
                InactiveIdentityUserNotifierJob.Paramters),
            new JobDefinition(
                InactiveIdentityUserCleanupJob.Name,
                typeof(InactiveIdentityUserCleanupJob),
                LocalizableStatic.Create("InactiveIdentityUserCleanupJob"),
                InactiveIdentityUserCleanupJob.Paramters)
            );
    }
}
