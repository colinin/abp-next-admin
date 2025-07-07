using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.Timing;

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
        var clock = scope.ServiceProvider.GetRequiredService<IClock>();
        var jobExecuter = scope.ServiceProvider.GetRequiredService<IJobRunnableExecuter>();

        context.Put(nameof(JobEventData.RunTime), clock.Now);

        var jobContext = new JobRunnableContext(
            jobDefinition.JobType,
            scope.ServiceProvider,
            context.MergedJobDataMap.ToImmutableDictionary(),
            getCache: context.Get,
            setCache: context.Put,
            cancellationToken: context.CancellationToken);

        await jobExecuter.ExecuteAsync(jobContext);

        context.Result = jobContext.Result;
    }
}
