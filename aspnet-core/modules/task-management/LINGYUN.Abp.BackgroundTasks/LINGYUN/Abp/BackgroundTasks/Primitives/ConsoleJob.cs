using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Primitives;

public class ConsoleJob : IJobRunnable
{
    public Task ExecuteAsync(JobRunnableContext context)
    {
        Console.WriteLine($"This message comes from the job: {GetType()}");
        return Task.CompletedTask;
    }
}
