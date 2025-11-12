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
                InactiveIdentitySessionCleanupJob.Paramters)
            // TODO: 实现用户过期清理作业需要增加用户会话实体
            //new JobDefinition(
            //    InactiveIdentityUserCleanupJob.Name,
            //    typeof(InactiveIdentityUserCleanupJob),
            //    LocalizableStatic.Create("InactiveIdentityUserCleanupJob"))
            );
    }
}
