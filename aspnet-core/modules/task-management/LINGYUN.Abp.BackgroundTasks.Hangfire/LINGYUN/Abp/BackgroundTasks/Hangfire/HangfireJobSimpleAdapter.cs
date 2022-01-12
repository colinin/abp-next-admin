using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Hangfire;

public class HangfireJobSimpleAdapter
{
    protected IServiceProvider ServiceProvider { get; }

    public HangfireJobSimpleAdapter(
        IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public async virtual Task<object> ExecuteAsync(Type jobRunnableType, IReadOnlyDictionary<string, object> jobData)
    {
        using var scope = ServiceProvider.CreateScope();
        var jobExecuter = scope.ServiceProvider.GetRequiredService<IJobRunnableExecuter>();

        var jobContext = new JobRunnableContext(
            jobRunnableType,
            ServiceProvider,
            jobData);

        await jobExecuter.ExecuteAsync(jobContext);

        return jobContext.Result;
    }
}
