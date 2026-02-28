using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Json;

namespace LINGYUN.Abp.BackgroundTasks;

public class BackgroundJobAdapter<TArgs> : IJobRunnable
{
    public ILogger<BackgroundJobAdapter<TArgs>> Logger { protected get; set; }

    protected AbpBackgroundJobOptions Options { get; }
    protected IBackgroundJobExecuter JobExecuter { get; }

    public BackgroundJobAdapter(
        IOptions<AbpBackgroundJobOptions> options,
        IBackgroundJobExecuter jobExecuter)
    {
        JobExecuter = jobExecuter;
        Options = options.Value;

        Logger = NullLogger<BackgroundJobAdapter<TArgs>>.Instance;
    }

    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        object jobArgs = null;
        if (context.TryGetString(nameof(TArgs), out var argsJson))
        {
            var jsonSerializer = context.GetRequiredService<IJsonSerializer>();
            jobArgs = jsonSerializer.Deserialize<TArgs>(argsJson);
        }

        var jobConfiguration = Options.GetJob<TArgs>();

        var jobContext = new JobExecutionContext(
            context.ServiceProvider,
            jobConfiguration.JobType,
            jobArgs, 
            context.CancellationToken);
        await JobExecuter.ExecuteAsync(jobContext);
    }
}
