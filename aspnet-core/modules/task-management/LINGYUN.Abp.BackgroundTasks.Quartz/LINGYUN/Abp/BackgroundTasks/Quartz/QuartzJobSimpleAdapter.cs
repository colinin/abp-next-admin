using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Collections.Immutable;
using System.Threading.Tasks;

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
        var jobExecuter = scope.ServiceProvider.GetRequiredService<IJobRunnableExecuter>();

        var jobContext = new JobRunnableContext(
            typeof(TJobRunnable),
            scope.ServiceProvider,
            context.MergedJobDataMap.ToImmutableDictionary(),
            getCache: (key) => context.Get(key),
            setCache: context.Put);

        await jobExecuter.ExecuteAsync(jobContext);

        context.Result = jobContext.Result;
    }
}
