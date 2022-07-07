using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzJobSearchJobAdapter : IJob
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IJobDefinitionManager JobDefinitionManager { get; }

    public QuartzJobSearchJobAdapter(
        IServiceScopeFactory serviceScopeFactory,
        IJobDefinitionManager jobDefinitionManager)
    {
        ServiceScopeFactory = serviceScopeFactory;
        JobDefinitionManager = jobDefinitionManager;
    }

    public async virtual Task Execute(IJobExecutionContext context)
    {
        var jobType = context.MergedJobDataMap.GetString(nameof(JobInfo.Type));
        var jobDefinition = JobDefinitionManager.Get(jobType);

        using var scope = ServiceScopeFactory.CreateScope();
        var jobExecuter = scope.ServiceProvider.GetRequiredService<IJobRunnableExecuter>();

        var jobContext = new JobRunnableContext(
            jobDefinition.JobType,
            scope.ServiceProvider,
            context.MergedJobDataMap.ToImmutableDictionary(),
            getCache: (key) => context.Get(key),
            setCache: context.Put);

        await jobExecuter.ExecuteAsync(jobContext);

        context.Result = jobContext.Result;
    }
}
