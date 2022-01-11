using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Json;

namespace LINGYUN.Abp.BackgroundTasks;

public class BackgroundJobAdapter<TArgs> : IJobRunnable
{
    public ILogger<BackgroundJobAdapter<TArgs>> Logger { protected get; set; }

    protected AbpBackgroundJobOptions Options { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IBackgroundJobExecuter JobExecuter { get; }

    public BackgroundJobAdapter(
        IOptions<AbpBackgroundJobOptions> options,
        IBackgroundJobExecuter jobExecuter,
        IServiceScopeFactory serviceScopeFactory)
    {
        JobExecuter = jobExecuter;
        ServiceScopeFactory = serviceScopeFactory;
        Options = options.Value;

        Logger = NullLogger<BackgroundJobAdapter<TArgs>>.Instance;
    }

    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        object args = null;
        if (context.TryGetString(nameof(TArgs), out var argsJson))
        {
            var jsonSerializer = context.GetRequiredService<IJsonSerializer>();
            args = jsonSerializer.Deserialize<TArgs>(argsJson);
        }

        var jobType = Options.GetJob(typeof(TArgs)).JobType;
        var jobContext = new JobExecutionContext(scope.ServiceProvider, jobType, args);
        await JobExecuter.ExecuteAsync(jobContext);
    }
}
