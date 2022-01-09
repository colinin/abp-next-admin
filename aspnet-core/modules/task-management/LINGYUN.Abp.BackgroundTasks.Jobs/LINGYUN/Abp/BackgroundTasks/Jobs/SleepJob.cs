using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class SleepJob : IJobRunnable
{
    public async Task ExecuteAsync(JobRunnableContext context)
    {
        context.JobData.TryGetValue("Delay", out var sleep);

        Console.WriteLine($"Sleep {sleep ?? 20000} milliseconds.");

        await Task.Delay(sleep?.To<int>() ?? 20000);
    }
}
