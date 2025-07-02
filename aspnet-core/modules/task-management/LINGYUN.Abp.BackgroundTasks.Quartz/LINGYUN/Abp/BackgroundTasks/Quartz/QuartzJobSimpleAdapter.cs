using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

public class QuartzJobSimpleAdapter<TJobRunnable> : IJob
    where TJobRunnable : IJobRunnable
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    public QuartzJobSimpleAdapter(
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
    }

    public async virtual Task Execute(IJobExecutionContext context)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var clock = scope.ServiceProvider.GetRequiredService<IClock>();
        var jobExecuter = scope.ServiceProvider.GetRequiredService<IJobRunnableExecuter>();

        context.Put(nameof(JobEventData.RunTime), clock.Now);

        var jobContext = new JobRunnableContext(
            typeof(TJobRunnable),
            scope.ServiceProvider,
            context.MergedJobDataMap.ToImmutableDictionary(),
            getCache: context.Get,
            setCache: context.Put,
            cancellationToken: context.CancellationToken);

        await jobExecuter.ExecuteAsync(jobContext);

        context.Result = jobContext.Result;
    }
}
