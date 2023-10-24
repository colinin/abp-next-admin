using LINGYUN.Abp.BackgroundTasks;

namespace LINGYUN.Abp.Elasticsearch.Jobs;

public class NotificationJobDefinitionProvider : JobDefinitionProvider
{
    public override void Define(IJobDefinitionContext context)
    {
        context.Add(
            new JobDefinition(
                ExpiredIndicesCleanupJob.Name,
                typeof(ExpiredIndicesCleanupJob),
                LocalizableStatic.Create("IndicesCleanupJob"),
                ExpiredIndicesCleanupJob.Paramters));
    }
}
